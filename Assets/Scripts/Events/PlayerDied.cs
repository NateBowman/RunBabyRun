//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="PlayerDied.cs">
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
    public class PlayerDied : GameEvent
    {
        public PlayerDied()
        {
        }

        public PlayerDied(GameObject sender)
            : base(sender)
        {
        }
    }
}