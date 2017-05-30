//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ButtonAttribute.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace NateTools.Attributes
{
    using System;

    using UnityEngine;

    /// <summary>
    ///     Activates a method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ButtonAttribute : PropertyAttribute
    {
        public ButtonAttribute(int order = 2000)
        {
            this.order = order;
        }

        public MethodDrawOrder DrawLocation { get; set; }
    }
}