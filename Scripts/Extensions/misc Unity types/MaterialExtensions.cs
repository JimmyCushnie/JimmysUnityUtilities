using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class MaterialExtensions
    {
        // this one is just so the modifications to Vector1 line up with modifications to Vector2/3/4
        public static void SetVector(this Material material, string name, float value)
            => material.SetFloat(name, value);

        public static void SetVector(this Material material, string name, Vector2 value)
            => SetVector(material, name, value.x, value.y);

        public static void SetVector(this Material material, string name, float x, float y) 
            => material.SetVector(name, new Vector4(x, y, 0, 0));

        public static void SetVector(this Material material, string name, Vector3 value)
            => SetVector(material, name, value.x, value.y, value.z);

        public static void SetVector(this Material material, string name, float x, float y, float z) 
            => material.SetVector(name, new Vector4(x, y, z, 0));

        public static void SetVector(this Material material, string name, float x, float y, float z, float w)
            => material.SetVector(name, new Vector4(x, y, z, w));

        public static void SetBoolean(this Material material, string name, bool value)
            => SetVector(material, name, value ? 1 : 0);


        /// <summary>
        /// The Unity function for this (<see cref="Material(Material)"/>) does not copy shader keywords when cloning runtime-created materials. This method works properly, however.
        /// </summary>
        public static Material Clone(this Material template)
        {
            var clone = new Material(template);
            clone.shaderKeywords = template.shaderKeywords;
            return clone;
        }
    }
}