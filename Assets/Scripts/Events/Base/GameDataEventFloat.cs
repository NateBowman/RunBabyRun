//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="GameDataEventFloat.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    /// <summary>
    ///     GameDataEvent where the type of data sent is <see langword="float" />
    /// </summary>
    public abstract class GameDataEventFloat : GameDataEvent<float>
    {
        protected GameDataEventFloat(GameObject sender = null, float value = 0f)
            : base(sender, value)
        {
        }
    }
}