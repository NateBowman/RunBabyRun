//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="GameDataEventInt.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    /// <summary>
    ///     GameDataEvent where the type of data sent is <see langword="int" />
    /// </summary>
    public abstract class GameDataEventInt : GameDataEvent<int>
    {
        protected GameDataEventInt(GameObject sender = null, int value = 0)
            : base(sender, value)
        {
        }
    }
}