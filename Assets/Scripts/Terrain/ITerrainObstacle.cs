//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ITerrainObstacle.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Terrain
{
    using UnityEngine;

    public interface ITerrainObstacle
    {
        Vector3 Center { get; }

        Vector3 EndPosition { get; }

        float Height { get; set; }

        Vector3 StartPosition { get; }

        TerrainType TerrainType { get; set; }

        float Width { get; set; }

        void SetUp();
    }
}