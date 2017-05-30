//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="PropertyEditor.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace NateTools.Editor
{
    using Terrain;

    using UnityEditor;

    using UnityEngine;

    public class PropertyEditor<TProperty> : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    [CustomPropertyDrawer(typeof(ITerrainObstacle))]
    public class ITerrainObstacleDrawer : PropertyEditor<ITerrainObstacle>
    {
        /// <inheritdoc />
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.ObjectField(position, property, label);
            //base.OnGUI(position, property, label);
        }
    }

}