//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="GameStats.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Level
{
    using System;

    [Serializable]
    public class GameStats
    {
        public int FinalChunk { get; set; }

        public int Jumps { get; set; }

        public int Score { get; set; }
    }
}