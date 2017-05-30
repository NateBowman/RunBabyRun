//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="HealthChanged.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    public class HealthChanged : GameDataEventInt
    {
        public HealthChanged(GameObject sender, int value)
            : base(sender, value)
        {
        }
    }
}