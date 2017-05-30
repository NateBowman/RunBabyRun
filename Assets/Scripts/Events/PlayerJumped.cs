//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="PlayerJumped.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    /// <summary>
    ///     An Event Raised when the player jumps
    /// </summary>
    public class PlayerJumped : GameEvent
    {
        public PlayerJumped(GameObject sender)
            : base(sender)
        {
        }

        public PlayerJumped()
        {
        }
    }
}