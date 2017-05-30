//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="Section.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Terrain
{
    using Events;
    using Events.Triggers;

    using NateTools.Attributes;
    using NateTools.Utils;

    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

#endif

    public class Section : TerrainArea
    {
        public float Difficulty = 1;

        public SectionType SectionType = SectionType.Flat;

        private Vector3 LocalToWorldPoint(Vector3 localPoint)
        {
            return transform.position + localPoint;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                EventManager.Raise(new ChunkEntered(gameObject.transform.parent.gameObject));
            }
        }

        private void Start()
        {
            // Add a trigger the size of the terrain
            var col = gameObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true;

            var min = new Vector2(Mathf.Min(StartLocation.StartPosition.x, EndLocation.EndPosition.x), Mathf.Min(StartLocation.StartPosition.y, EndLocation.EndPosition.y));
            var max = new Vector2(Mathf.Max(StartLocation.StartPosition.x, EndLocation.EndPosition.x), Mathf.Max(StartLocation.StartPosition.y, EndLocation.EndPosition.y));
            var distance = max - min;

            // offset to middle of the platform
            col.offset = (min - (Vector2)transform.position) + (distance / 2f);

            // size to a section inset by 1 (x axis) to prevent triggering on edge clipping
            col.size = (distance + Vector2.up) - Vector2.right;

            // Add points for landing on it
            var pts = gameObject.AddComponent<ScorePointsOnTrigger>();
            pts.Layer = LayerMask.GetMask("Player");
            pts.PointValue = (int)Mathf.Ceil(Difficulty) * 10;
            pts.InfiniteExecutions = false;
            pts.RemainingExecutions = 1;
            pts.TriggerOnType = LayerMaskedTriggerEvent.TriggerType.Enter;

            // TODO: Set spawn 
        }

        private void Update()
        {
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            var color = Color.yellow;
            if (transform.parent)
            {
                Color.HSVToRGB(transform.GetSiblingIndex() / (float)transform.parent.childCount, 0.5f, 1f);
            }
            color.a = 0.5f;
            Gizmos.color = color;

            var b = new Bounds(transform.position, Vector3.zero);

            b.Encapsulate(StartLocation.StartPosition);
            b.Encapsulate(EndLocation.EndPosition);
            foreach (Transform t in transform)
            {
                foreach (Transform t1 in t)
                {
                    b.Encapsulate(t1.position);
                }

                b.Encapsulate(t.position);
            }

            // Draw bounds
            Gizmos.DrawCube(b.center, b.size);
            Gizmos.DrawWireCube(b.center, b.size);

            // Draw start location gizmo
            if (StartLocation != null)
            {
                Gizmos.DrawWireSphere(StartLocation.StartPosition, 0.1f);
                Gizmos.DrawSphere(StartLocation.StartPosition, 0.1f);
            }

            //draw end location gizmo
            if (EndLocation != null)
            {
                Gizmos.DrawWireSphere(EndLocation.EndPosition, 0.1f);
                Gizmos.DrawSphere(EndLocation.EndPosition, 0.1f);
            }
        }

        [Button]
        private void Recalculate()
        {
            foreach (Transform t in transform)
            {
                var terrainObstacle = t.GetComponent<ITerrainObstacle>();
                if (terrainObstacle != null)
                {
                    terrainObstacle.SetUp();
                }
            }

            CalculateDifficulty();
            SceneView.RepaintAll();
        }

        private void CalculateDifficulty()
        {
            Difficulty = 0;
            var children = transform.ChildrenToList();
            children.Sort((child1, child2) => (int)Mathf.Sign(child1.position.x - child2.position.x));

            for (var i = 0; i < (children.Count - 1); i++)
            {
                if ((children[i + 1].position.x - children[i].position.x) > 1.1f)
                {
                    // this is a gap
                    Difficulty += 1;
                }

                if ((children[i + 1].position.y - children[i].position.y) > 0.1f)
                {
                    // this is a jump up
                    Difficulty += 1;
                }

                if ((children[i + 1].position.y - children[i].position.y) < -0.1f)
                {
                    // this is a jump down
                    Difficulty += 0.5f;
                }
            }
        }
#endif
    }
}