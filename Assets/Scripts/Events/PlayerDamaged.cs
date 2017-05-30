//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="PlayerDamaged.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    /// <summary>
    ///     Event raised when the player dies
    /// </summary>
    public class PlayerDamaged : GameDataEventInt
    {
        public PlayerDamaged()
            : base(value: 1)
        {
        }

        public PlayerDamaged(GameObject sender = null, int value = 1)
            : base(sender, value)
        {
        }
    }
}