//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="EditorButtonAttribute.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace NateTools.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using NateTools;
    using NateTools.Attributes;

    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public class EditorButtonAttribute : Editor
    {
        public override void OnInspectorGUI()
        {
            var mono = target as MonoBehaviour;

            if (mono != null)
            {
                var methods =
                    mono.GetType()
                        .GetMembers(
                            BindingFlags.Instance | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public
                            | BindingFlags.NonPublic)
                        .Where(o => Attribute.IsDefined(o, typeof(ButtonAttribute)))
                        .ToList();

                EditorGUI.BeginChangeCheck();
                serializedObject.Update();
                var iterator = serializedObject.GetIterator();

                Utils.DoHorizontal(() => DrawButtonsBeforeOrderNumber(methods, MethodDrawOrder.Top));
                for (var enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                {
                    EditorGUI.BeginDisabledGroup(iterator.propertyPath == "m_Script");

                    EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);

                    EditorGUI.EndDisabledGroup();
                }

                Utils.DoHorizontal(() => DrawButtonsBeforeOrderNumber(methods, MethodDrawOrder.Bottom));
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }

        private void DrawButtonsBeforeOrderNumber(List<MemberInfo> methods, MethodDrawOrder order)
        {
            for (var i = methods.Count - 1; i >= 0; i--)
            {
                var memberInfo = methods[i];
                var propertyAttribute =
                    Attribute.GetCustomAttribute(memberInfo, typeof(ButtonAttribute)) as ButtonAttribute;

                var colourAttr = Attribute.GetCustomAttribute(memberInfo, typeof(ColorAttribute)) as ColorAttribute;

                Utils.DoBackgroundColor(
                    colourAttr != null ? colourAttr.BGColor : GUI.backgroundColor,
                    () =>
                        {
                            if ((propertyAttribute != null) && (propertyAttribute.DrawLocation == order))
                            {
                                if (GUILayout.Button(memberInfo.Name))
                                {
                                    foreach (var monoBehavior in targets.Cast<MonoBehaviour>())
                                    {
                                        var method = memberInfo as MethodInfo;
                                        if (method != null)
                                        {
                                            method.Invoke(monoBehavior, null);
                                        }
                                    }
                                }

                                methods.Remove(memberInfo);
                            }
                        });
            }
        }
    }
}