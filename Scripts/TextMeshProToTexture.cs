using System;
using TMPro;
using UnityEngine;

namespace JimmysUnityUtilities
{
    /// <summary>
    /// Utility for rendering TextMeshPro objects to textures.
    /// </summary>
    public class TextMeshProToTexture : IDisposable
    {
        // TODO: offer a choice between anti-aliasing or no anti aliasing

        /// <summary>
        /// Creates a new <see cref="TextMeshProToTexture"/> instance to render with. Make sure you call <see cref="Dispose"/> when you're finished rendering.
        /// </summary>
        /// <param name="layer"> The layer on which the rendering will take place. Be sure to use a layer with no other game objects, or those might be included in the render. </param>
        public static TextMeshProToTexture Create(int layer)
        {
            var parentObject = new GameObject($"{nameof(TextMeshProToTexture)} Rendering Object");
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


            return new TextMeshProToTexture(parentObject, camera, text);
        }


        private GameObject ParentObject { get; }
        private Camera RenderingCamera { get; }
        private TextMeshPro RenderingText { get; }

        private TextMeshProToTexture(GameObject parentObject, Camera renderingCamera, TextMeshPro renderingText)
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
        /// <param name="text"> The TextMeshPro object to render. </param>
        /// <param name="pixelsPerUnityUnit"> The number of pixels to use per Unity unit. Note that  </param>
        /// <returns></returns>
        public Texture2D RenderWithConsistentPixelDensity(TMP_Text text, int pixelsPerUnityUnit)
        {
            float height = text.rectTransform.rect.height;
            int verticalResolution = Mathf.RoundToInt(height * pixelsPerUnityUnit);

            return Render(text, verticalResolution);
        }

        /// <summary>
        /// Renders a TextMeshPro object to a <see cref="Texture2D"/>
        /// </summary>
        /// <param name="text"> The TextMeshPro object to render. </param>
        /// <param name="verticalResolution"> The render will have this many pixels on the Y axis. The X axis will be sized in accordance to the aspect ratio of <paramref name="text"/>. </param>
        public Texture2D Render(TMP_Text text, int verticalResolution = 512)
        {
            (float width, float height) = (text.rectTransform.rect.width, text.rectTransform.rect.height);

            RenderingText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            RenderingText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            // We never touch the original text object - we just copy its properties to another object for the render
            CopyTextProperties(text, RenderingText);
            RenderingText.ForceMeshUpdate();

            float aspectRatio = width / height;
            int horizontalResolution = Mathf.RoundToInt(verticalResolution * aspectRatio);

            var renderTexture = RenderTexture.GetTemporary(horizontalResolution, verticalResolution, 24);

            RenderingCamera.orthographicSize = height / 2f;
            RenderingCamera.targetTexture = renderTexture;
            RenderingCamera.Render();
            RenderingCamera.targetTexture = null;

            var renderedImage = renderTexture.ToTexture2D(supportTransparency: true);

            RenderTexture.ReleaseTemporary(renderTexture);
            return renderedImage;


            void CopyTextProperties(TMP_Text source, TMP_Text target)
            {
                // There should probably be some more properties here. If a render looks wrong, it's probably because
                // some property was not copied from the original to the rendering text.

                target.text = source.text;
                target.textStyle = source.textStyle;
                target.isRightToLeftText = source.isRightToLeftText;

                target.font = source.font;
                target.fontSharedMaterial = source.fontSharedMaterial;
                target.fontStyle = source.fontStyle;

                target.fontSize = source.fontSize;
                target.enableAutoSizing = source.enableAutoSizing;
                target.fontSizeMin = source.fontSizeMin;
                target.fontSizeMax = source.fontSizeMax;

                target.color = source.color;
                target.colorGradient = source.colorGradient;
                target.colorGradientPreset = source.colorGradientPreset;
                target.faceColor = source.faceColor;
                target.outlineColor = source.outlineColor;
                target.overrideColorTags = source.overrideColorTags;

                target.characterSpacing = source.characterSpacing;
                target.lineSpacing = source.lineSpacing;
                target.wordSpacing = source.wordSpacing;
                target.paragraphSpacing = source.paragraphSpacing;

                target.alignment = source.alignment;
                target.horizontalAlignment = source.horizontalAlignment;
                target.verticalAlignment = source.verticalAlignment;

                target.enableWordWrapping = source.enableWordWrapping;
                target.wordWrappingRatios = source.wordWrappingRatios;
                target.overflowMode = source.overflowMode;

                target.horizontalMapping = source.horizontalMapping;
                target.verticalMapping = source.verticalMapping;

                target.margin = source.margin;

                target.richText = source.richText;
                target.parseCtrlCharacters = source.parseCtrlCharacters;

                target.spriteAsset = source.spriteAsset;
                target.styleSheet = source.styleSheet;

                target.enableKerning = source.enableKerning;
                target.extraPadding = source.extraPadding;
            }
        }
    }
}
