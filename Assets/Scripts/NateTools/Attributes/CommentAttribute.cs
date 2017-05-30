//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="CommentAttribute.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace NateTools.Attributes
{
    using System;

    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CommentAttribute : PropertyAttribute
    {
        public CommentAttribute(string comment, CommentType commentType = CommentType.Info, int order = 1001)
        {
            Comment = comment;
            this.order = order;
            Type = commentType;
        }

        public string Comment { get; set; }

        public CommentType Type { get; set; }
    }
}