//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="GameDataEvent.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    /// <summary>
    ///     Generic <see langword="abstract" /> class to provide event data
    /// </summary>
    /// <typeparam name="T">The type of data that is sent with the raised event</typeparam>
    public abstract class GameDataEvent<T> : GameEvent
    {
        protected GameDataEvent(GameObject sender = null, T value = default(T))
            : base(sender)
        {
            Value = value;
        }

        /// <summary>
        ///     Gets the value of the data in the raised event
        /// </summary>
        public T Value { get; private set; }
    }
}