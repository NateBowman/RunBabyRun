//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="GameEvent.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    /// <summary>
    ///     A basic game event that contains no data other than the sender
    /// </summary>
    public abstract class GameEvent
    {
        private readonly GameObject sender;

        protected GameEvent(GameObject sender)
        {
            this.sender = sender;
        }

        protected GameEvent()
        {
        }

        /// <summary>
        ///     Gets the GameObject that raised the event
        /// </summary>
        public GameObject Sender { get { return sender; } }
    }
}