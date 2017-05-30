//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="GameDataEventStats.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using Level;

    using UnityEngine;

    public abstract class GameDataEventStats : GameDataEvent<GameStats>
    {
        protected GameDataEventStats(GameObject sender, GameStats value)
            : base(sender, value)
        {
        }
    }
}