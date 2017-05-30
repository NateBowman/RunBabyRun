//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ScoreChanged.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    /// <summary>
    ///     An event raised when score has changed, data value is current score
    /// </summary>
    public class ScoreChanged : GameDataEventInt
    {
        public ScoreChanged()
        {
        }

        public ScoreChanged(GameObject sender, int points)
            : base(sender, points)
        {
        }
    }
}