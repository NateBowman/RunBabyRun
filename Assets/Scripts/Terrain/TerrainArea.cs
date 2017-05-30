//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="TerrainArea.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Terrain
{
    using UnityEngine;

    /// <summary>
    ///     Base class of any Area of game terrain
    /// </summary>
    public abstract class TerrainArea : MonoBehaviour
    {
        /// <summary>
        ///     The start <see cref="TerrainItem" /> of the Area
        /// </summary>
        public TerrainItemBase EndLocation;

        /// <summary>
        ///     The End <see cref="TerrainItem" /> of the Area
        /// </summary>
        public TerrainItemBase StartLocation;

        public void PositionStartAt(Vector3 position)
        {
            if (StartLocation != null)
            {
                transform.position = position + (transform.position - StartLocation.StartPosition);
            }
        }
    }
}