using System;
using TMPro;
using UnityEngine;

namespace JimmysUnityUtilities.TextTextures
{
    /// <summary>
    /// Utility for rendering TextMeshPro objects to textures.
    /// </summary>
    public class TextTextureRenderer : IDisposable
    {
        // TODO: offer a choice between anti-aliasing or no anti aliasing
        // TODO: somehow address the fact that some text materials are affected by lighting...

        /// <summary>
        /// Creates a new <see cref="TextTextureRenderer"/> instance to render with. Make sure you call <see cref="Dispose"/> when you're finished rendering.
        /// </summary>
        /// <param name="layer"> The layer on which the rendering will take place. Be sure to use a layer with no other game objects, or those might be included in the render. </param>
        public static TextTextureRenderer Create(int layer)
        {
            var parentObject = new GameObject($"{nameof(TextTextureRenderer)} Rendering Object");
            var cameraObject = new GameObject("Render Camera");
            var textObject = new GameObject("Render Text");
            cameraObject.transform.parent = parentObject.transform;
            textObject.transform.parent = parentObject.transform;
            parentObject.SetLayerRecursively(layer);


            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.farClipPlane = 2f;
            camera.clearFlags = CameraClearFlags.Color;
            camera.cullingMask = 1 << layer;

            // We make the background color transparent, so the rendered images correctly have transparency.
            // The RGB components of the background will be blended slightly around the edges of the text. I think this is due to anti-aliasing.
            // I figure if we're going to have a tiny border of any color, solid black is the safest.
            camera.backgroundColor = new Color(0, 0, 0, 0);

            var text = textObject.AddComponent<TextMeshPro>();
            text.transform.localPosition = new Vector3(0, 0, 1);


            return new TextTextureRenderer(parentObject, camera, text);
        }


        private GameObject ParentObject { get; }
        private Camera RenderingCamera { get; }
        private TextMeshPro RenderingText { get; }

        private TextTextureRenderer(GameObject parentObject, Camera renderingCamera, TextMeshPro renderingText)
        {
            ParentObject = parentObject;
            RenderingCamera = renderingCamera;
            RenderingText = renderingText;
        }

        /// <summary>
        /// Cleans up all game objects used for the render. 
        /// This is implemented with the <see cref="IDisposable"/> interface, so you can also call this with the <see langword="using"/> pattern.
        /// </summary>
        public void Dispose()
        {
            UnityEngine.Object.Destroy(ParentObject);
        }


        /// <summary>
        /// Renders a TextMeshPro object to a <see cref="Texture2D"/>, but the resolution is dependent on the size of the object.
        /// This is intended when you need to render many different text objects of different sizes, but you want a consistent pixel density across them all.
        /// </summary>
        public Texture2D RenderWithConsistentPixelDensity(TMP_Text textObject, int pixelsPerUnityUnit)
            => RenderWithConsistentPixelDensity(TmpObjectVisualData.CreateFrom(textObject), pixelsPerUnityUnit);

        public Texture2D RenderWithConsistentPixelDensity(TmpObjectVisualData data, int pixelsPerUnityUnit)
        {
            int verticalResolution = Mathf.RoundToInt(data.Height * pixelsPerUnityUnit);
            return Render(data, verticalResolution);
        }

        /// <summary>
        /// Renders a TextMeshPro object to a <see cref="Texture2D"/>
        /// </summary>
        /// <param name="verticalResolution"> The render will have this many pixels on the Y axis. The X axis will be sized in accordance to the aspect ratio of <paramref name="textObject"/>. </param>
        public Texture2D Render(TMP_Text textObject, int verticalResolution = 512)
            => Render(TmpObjectVisualData.CreateFrom(textObject), verticalResolution);

        public Texture2D Render(TmpObjectVisualData data, int verticalResolution)
        {
            data.ApplyPropertiesTo(RenderingText);
            RenderingText.ForceMeshUpdate();

            float aspectRatio = data.Width / data.Height;
            int horizontalResolution = Mathf.RoundToInt(verticalResolution * aspectRatio);

            var renderTexture = RenderTexture.GetTemporary(horizontalResolution, verticalResolution, 24);

            RenderingCamera.orthographicSize = data.Height / 2f;
            RenderingCamera.targetTexture = renderTexture;
            RenderingCamera.Render();
            RenderingCamera.targetTexture = null;

            var renderedImage = renderTexture.ToTexture2D(supportTransparency: true);

            RenderTexture.ReleaseTemporary(renderTexture);
            return renderedImage;
        }
    }
}
