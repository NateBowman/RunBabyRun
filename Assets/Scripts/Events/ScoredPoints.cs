//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ScoredPoints.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    /// <summary>
    ///     An event raised when points are scored, data value is the points scored
    /// </summary>
    public class ScoredPoints : GameDataEventInt
    {
        public ScoredPoints()
        {
        }

        public ScoredPoints(GameObject sender, int points)
            : base(sender, points)
        {
        }
    }
}