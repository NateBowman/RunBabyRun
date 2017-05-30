//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="PlayerSpawned.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    /// <summary>
    ///     Event is raised when the player first spawns or re-spawns
    /// </summary>
    public class PlayerSpawned : GameEvent
    {
        public PlayerSpawned()
        {
        }

        public PlayerSpawned(GameObject sender)
            : base(sender)
        {
        }
    }
}