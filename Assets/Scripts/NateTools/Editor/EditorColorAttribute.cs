//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="EditorColorAttribute.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace NateTools.Editor
{
    using NateTools.Attributes;

    using UnityEditor;

    using UnityEngine;

    [CustomPropertyDrawer(typeof(ColorAttribute),true), CanEditMultipleObjects,]
    public class EditorColorAttribute : PropertyEditor<ColorAttribute>
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var colour = (attribute as ColorAttribute).BGColor;
            Utils.DoBackgroundColor(colour, () => base.OnGUI(position, property, label));
        }
    }
}