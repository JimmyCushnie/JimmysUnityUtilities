using System;
using System.Collections.Generic;
using JimmysUnityUtilities.EqualityComparers;
using TMPro;
using UnityEngine;

namespace JimmysUnityUtilities.TextTextures
{
    /// <summary>
    /// Contains data that affects the visuals of TextMeshPro objects, for use in <see cref="TextTextureRenderer"/>.
    /// This class has functional <see cref="GetHashCode"/> and <see cref="Equals(object)"/> methods, so you can use it as keys to a dictionary if you want to render a bunch of text objects and avoid duplication.
    /// </summary>
    public class TmpObjectVisualData : IEquatable<TmpObjectVisualData>
    {
        // There are probably a few missing properties here. TMP has a lot going on...

        // Notice that these first two properties work slightly differently than the others.
        // While the others correspond to properties on a TMP_Text object, these correspond to properties of the attached RectTransform.
        public float Width { get; set; }
        public float Height { get; set; }


        public string text { get; set; }
        public TMP_Style textStyle { get; set; }
        public bool isRightToLeftText { get; set; }

        public TMP_FontAsset font { get; set; }
        public Material fontSharedMaterial { get; set; }
        public FontStyles fontStyle { get; set; }

        public float fontSize { get; set; }
        public bool enableAutoSizing { get; set; }
        public float fontSizeMin { get; set; }
        public float fontSizeMax { get; set; }

        public Color color { get; set; }
        public VertexGradient colorGradient { get; set; }
        public TMP_ColorGradient colorGradientPreset { get; set; }
        public Color32 faceColor { get; set; }
        public Color32 outlineColor { get; set; }
        public bool overrideColorTags { get; set; }

        public float characterSpacing { get; set; }
        public float lineSpacing { get; set; }
        public float wordSpacing { get; set; }
        public float paragraphSpacing { get; set; }

        public TextAlignmentOptions alignment { get; set; }
        public HorizontalAlignmentOptions horizontalAlignment { get; set; }
        public VerticalAlignmentOptions verticalAlignment { get; set; }

        public bool enableWordWrapping { get; set; }
        public float wordWrappingRatios { get; set; }
        public TextOverflowModes overflowMode { get; set; }

        public TextureMappingOptions horizontalMapping { get; set; }
        public TextureMappingOptions verticalMapping { get; set; }
        public Vector4 margin { get; set; }

        public bool richText { get; set; }
        public bool parseCtrlCharacters { get; set; }

        public TMP_SpriteAsset spriteAsset { get; set; }
        public TMP_StyleSheet styleSheet { get; set; }

        public bool enableKerning { get; set; }
        public bool extraPadding { get; set; }


        public static TmpObjectVisualData CreateFrom(TMP_Text source)
        {
            var data = new TmpObjectVisualData();

            data.Width = source.rectTransform.rect.width;
            data.Height = source.rectTransform.rect.height;


            data.text = source.text;
            data.textStyle = source.textStyle;
            data.isRightToLeftText = source.isRightToLeftText;

            data.font = source.font;
            data.fontSharedMaterial = source.fontSharedMaterial;
            data.fontStyle = source.fontStyle;

            data.fontSize = source.fontSize;
            data.enableAutoSizing = source.enableAutoSizing;
            data.fontSizeMin = source.fontSizeMin;
            data.fontSizeMax = source.fontSizeMax;

            data.color = source.color;
            data.colorGradient = source.colorGradient;
            data.colorGradientPreset = source.colorGradientPreset;
            data.faceColor = source.faceColor;
            data.outlineColor = source.outlineColor;
            data.overrideColorTags = source.overrideColorTags;

            data.characterSpacing = source.characterSpacing;
            data.lineSpacing = source.lineSpacing;
            data.wordSpacing = source.wordSpacing;
            data.paragraphSpacing = source.paragraphSpacing;

            data.alignment = source.alignment;
            data.horizontalAlignment = source.horizontalAlignment;
            data.verticalAlignment = source.verticalAlignment;

            data.enableWordWrapping = source.enableWordWrapping;
            data.wordWrappingRatios = source.wordWrappingRatios;
            data.overflowMode = source.overflowMode;

            data.horizontalMapping = source.horizontalMapping;
            data.verticalMapping = source.verticalMapping;
            data.margin = source.margin;

            data.richText = source.richText;
            data.parseCtrlCharacters = source.parseCtrlCharacters;

            data.spriteAsset = source.spriteAsset;
            data.styleSheet = source.styleSheet;

            data.enableKerning = source.enableKerning;
            data.extraPadding = source.extraPadding;

            return data;
        }

        public void ApplyPropertiesTo(TMP_Text target)
        {
            target.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.Width);
            target.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.Height);


            target.text = this.text;
            target.textStyle = this.textStyle;
            target.isRightToLeftText = this.isRightToLeftText;

            target.font = this.font;
            target.fontSharedMaterial = this.fontSharedMaterial;
            target.fontStyle = this.fontStyle;

            target.fontSize = this.fontSize;
            target.enableAutoSizing = this.enableAutoSizing;
            target.fontSizeMin = this.fontSizeMin;
            target.fontSizeMax = this.fontSizeMax;

            target.color = this.color;
            target.colorGradient = this.colorGradient;
            target.colorGradientPreset = this.colorGradientPreset;
            target.faceColor = this.faceColor;
            target.outlineColor = this.outlineColor;
            target.overrideColorTags = this.overrideColorTags;

            target.characterSpacing = this.characterSpacing;
            target.lineSpacing = this.lineSpacing;
            target.wordSpacing = this.wordSpacing;
            target.paragraphSpacing = this.paragraphSpacing;

            target.alignment = this.alignment;
            target.horizontalAlignment = this.horizontalAlignment;
            target.verticalAlignment = this.verticalAlignment;

            target.enableWordWrapping = this.enableWordWrapping;
            target.wordWrappingRatios = this.wordWrappingRatios;
            target.overflowMode = this.overflowMode;

            target.horizontalMapping = this.horizontalMapping;
            target.verticalMapping = this.verticalMapping;
            target.margin = this.margin;

            target.richText = this.richText;
            target.parseCtrlCharacters = this.parseCtrlCharacters;

            target.spriteAsset = this.spriteAsset;
            target.styleSheet = this.styleSheet;

            target.enableKerning = this.enableKerning;
            target.extraPadding = this.extraPadding;
        }



        public bool Equals(TmpObjectVisualData other)
        {
            // ! commented next to items where we aren't really doing a proper comparison and there could be issues
            return Width == other.Width
                && Height == other.Height
                
                && text == other.text
                && textStyle == other.textStyle //!
                && isRightToLeftText == other.isRightToLeftText

                && font == other.font //!
                && fontSharedMaterial == other.fontSharedMaterial //!
                && fontStyle == other.fontStyle

                && fontSize == other.fontSize
                && enableAutoSizing == other.enableAutoSizing
                && fontSizeMin == other.fontSizeMin
                && fontSizeMax == other.fontSizeMax

                && color.Equals(other.color)
                && VertexGradientEqualityComparer.Instance.Equals(colorGradient, other.colorGradient)
                && TMP_ColorGradientEqualityComparer.Instance.Equals(colorGradientPreset, other.colorGradientPreset)
                && Color32EqualityComparer.Instance.Equals(faceColor, other.faceColor)
                && Color32EqualityComparer.Instance.Equals(outlineColor, other.outlineColor)
                && overrideColorTags == other.overrideColorTags

                && characterSpacing == other.characterSpacing
                && lineSpacing == other.lineSpacing
                && wordSpacing == other.wordSpacing
                && paragraphSpacing == other.paragraphSpacing

                && alignment == other.alignment
                && horizontalAlignment == other.horizontalAlignment
                && verticalAlignment == other.verticalAlignment

                && enableWordWrapping == other.enableWordWrapping
                && wordWrappingRatios == other.wordWrappingRatios
                && overflowMode == other.overflowMode

                && horizontalMapping == other.horizontalMapping
                && verticalMapping == other.verticalMapping
                && margin == other.margin

                && richText == other.richText
                && parseCtrlCharacters == other.parseCtrlCharacters

                && spriteAsset == other.spriteAsset //!
                && styleSheet == other.styleSheet //!

                && enableKerning == other.enableKerning
                && extraPadding == other.extraPadding;
        }

        public override bool Equals(object obj)
        {
            return obj is TmpObjectVisualData other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = -814685956;
                hashCode = hashCode * -1521134295 + Width.GetHashCode();
                hashCode = hashCode * -1521134295 + Height.GetHashCode();
                hashCode = hashCode * -1521134295 + text.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<TMP_Style>.Default.GetHashCode(textStyle);
                hashCode = hashCode * -1521134295 + isRightToLeftText.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<TMP_FontAsset>.Default.GetHashCode(font);
                hashCode = hashCode * -1521134295 + EqualityComparer<Material>.Default.GetHashCode(fontSharedMaterial);
                hashCode = hashCode * -1521134295 + fontStyle.GetHashCode();
                hashCode = hashCode * -1521134295 + fontSize.GetHashCode();
                hashCode = hashCode * -1521134295 + enableAutoSizing.GetHashCode();
                hashCode = hashCode * -1521134295 + fontSizeMin.GetHashCode();
                hashCode = hashCode * -1521134295 + fontSizeMax.GetHashCode();
                hashCode = hashCode * -1521134295 + color.GetHashCode();
                hashCode = hashCode * -1521134295 + colorGradient.GetHashCode();
                hashCode = hashCode * -1521134295 + TMP_ColorGradientEqualityComparer.Instance.GetHashCode(colorGradientPreset);
                hashCode = hashCode * -1521134295 + faceColor.GetHashCode();
                hashCode = hashCode * -1521134295 + outlineColor.GetHashCode();
                hashCode = hashCode * -1521134295 + overrideColorTags.GetHashCode();
                hashCode = hashCode * -1521134295 + characterSpacing.GetHashCode();
                hashCode = hashCode * -1521134295 + lineSpacing.GetHashCode();
                hashCode = hashCode * -1521134295 + wordSpacing.GetHashCode();
                hashCode = hashCode * -1521134295 + paragraphSpacing.GetHashCode();
                hashCode = hashCode * -1521134295 + alignment.GetHashCode();
                hashCode = hashCode * -1521134295 + horizontalAlignment.GetHashCode();
                hashCode = hashCode * -1521134295 + verticalAlignment.GetHashCode();
                hashCode = hashCode * -1521134295 + enableWordWrapping.GetHashCode();
                hashCode = hashCode * -1521134295 + wordWrappingRatios.GetHashCode();
                hashCode = hashCode * -1521134295 + overflowMode.GetHashCode();
                hashCode = hashCode * -1521134295 + horizontalMapping.GetHashCode();
                hashCode = hashCode * -1521134295 + verticalMapping.GetHashCode();
                hashCode = hashCode * -1521134295 + margin.GetHashCode();
                hashCode = hashCode * -1521134295 + richText.GetHashCode();
                hashCode = hashCode * -1521134295 + parseCtrlCharacters.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<TMP_SpriteAsset>.Default.GetHashCode(spriteAsset);
                hashCode = hashCode * -1521134295 + EqualityComparer<TMP_StyleSheet>.Default.GetHashCode(styleSheet);
                hashCode = hashCode * -1521134295 + enableKerning.GetHashCode();
                hashCode = hashCode * -1521134295 + extraPadding.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(TmpObjectVisualData left, TmpObjectVisualData right) => EqualityComparer<TmpObjectVisualData>.Default.Equals(left, right);

        public static bool operator !=(TmpObjectVisualData left, TmpObjectVisualData right) => !(left == right);
    }
}
