//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="Utils.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace NateTools.Editor
{
    using System;

    using UnityEditor;

    using UnityEngine;

    public class Utils
    {
        public static Rect DoHorizontal(Action del)
        {
            var r = EditorGUILayout.BeginHorizontal();
            del.Invoke();
            EditorGUILayout.EndHorizontal();
            return r;
        }

        public static Rect DoVertical(Action del)
        {
            var r = EditorGUILayout.BeginVertical();
            del.Invoke();
            EditorGUILayout.EndVertical();
            return r;
        }

        public static void DoBackgroundColor(Color col, Action del)
        {
            var old = GUI.backgroundColor;
            GUI.backgroundColor = col;
            del.Invoke();
            GUI.backgroundColor = old;
        }

        public static void DoForegroundColor(Color col, Action del)
        {
            var old = GUI.contentColor;

            GUI.contentColor = col;
            del.Invoke();
            GUI.contentColor = old;
        }

    }
}