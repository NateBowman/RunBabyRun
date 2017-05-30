//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="TerrainItemBase.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Terrain
{
    using NateTools.Attributes;
    using NateTools.Utils;

    using UnityEngine;

    /// <summary>
    ///     Base interface implementation as unity does not serialize interfaces
    /// </summary>
    public abstract class TerrainItemBase : MonoBehaviour, ITerrainObstacle
    {
        [SerializeField]
        [Range(0.0f, 15f)]
        protected float height;

        [SerializeField]
        protected TerrainType terrainType;

        [Range(0.0f, 15f)]
        [SerializeField]
        protected float width;

        [SerializeField]
        private Bounds bounds;

        public Bounds Bounds { get { return bounds; } private set { bounds = value; } }

        public Vector3 Center { get { return transform.position.Add(width / 2f, height / 2f); } }

        /// <inheritdoc />
        public Vector3 EndPosition { get { return transform.position.Add(width, height); } }

        public float Height { get { return height; } set { height = value; } }

        /// <inheritdoc />
        public Vector3 StartPosition { get { return transform.position.Add(0, height); } }

        public TerrainType TerrainType { get { return terrainType; } set { terrainType = value; } }

        public float Width { get { return width; } set { width = value; } }

        [Button]
        public void SetUp()
        {
            Bounds = new Bounds(Center, new Vector2(width, height));
            //Bounds.size = 

            var col = GetComponent<Collider2D>();
            if (col is BoxCollider2D)
            {
                col.offset = Center - transform.position;
                (col as BoxCollider2D).size = bounds.size;
            }
        }

        protected void OnDrawGizmos()
        {
            Gizmos.DrawSphere(StartPosition, 0.05f);
            Gizmos.DrawSphere(EndPosition, 0.05f);
        }
    }
}