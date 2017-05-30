//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="EditorCommentAttribute.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace NateTools.Editor
{
    using NateTools.Attributes;

    using UnityEditor;

    using UnityEngine;

    [CustomPropertyDrawer(typeof(CommentAttribute))]
    [CanEditMultipleObjects]
    public class EditorCommentAttribute : DecoratorDrawer
    {
        private const int extraHeight = 8;

        public static int GetCommentHeight(string comment, MessageType commentType)
        {
            var minHeight = 38;
            if (commentType == MessageType.None)
            {
                minHeight = 17;
            }

            GUIStyle style = "HelpBox";
            return Mathf.Max((int)style.CalcHeight(new GUIContent(comment), Screen.width), minHeight);
        }

        public override float GetHeight()
        {
            return GetCommentHeight(
                       ((CommentAttribute)attribute).Comment,
                       (MessageType)((CommentAttribute)attribute).Type) + extraHeight;
        }

        public override void OnGUI(Rect position)
        {
            position.y += extraHeight;
            var attr = attribute as CommentAttribute;
            var r = position;
            r.height -= extraHeight;
            EditorGUI.HelpBox(r, attr.Comment, (MessageType)attr.Type);
        }
    }
}