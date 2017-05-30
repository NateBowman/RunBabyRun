//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ChunkEntered.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    public class ChunkEntered : GameEvent
    {
        public ChunkEntered(GameObject sender)
            : base(sender)
        {
        }
    }
}