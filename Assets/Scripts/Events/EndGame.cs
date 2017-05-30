//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="EndGame.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using Level;

    using UnityEngine;

    public class EndGame : GameDataEventStats
    {
        public EndGame(GameObject sender, GameStats stats)
            : base(sender, stats)
        {
        }
    }
}