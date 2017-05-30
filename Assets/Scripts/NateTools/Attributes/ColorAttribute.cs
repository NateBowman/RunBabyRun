//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ColorAttribute.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace NateTools.Attributes
{
    using System;

    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public class ColorAttribute : PropertyAttribute
    {
        public Color BGColor;

        public ColorAttribute(int r, int g, int b)
            : this(1010)
        {
            BGColor = new Color(r, g, b);
        }

        public ColorAttribute(float r, float g, float b)
            : this(1010)
        {
            BGColor = new Color(r, g, b);
        }

        public ColorAttribute(int order)
        {
            this.order = order;
        }
    }
}