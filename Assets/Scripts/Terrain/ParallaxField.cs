//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ParallaxField.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Terrain
{
    using System.Collections.Generic;

    using NateTools.Attributes;
    using NateTools.Utils;

    using UnityEngine;

    public class ParallaxField : MonoBehaviour
    {
        /// <summary>
        ///     The prefab of the background tiles
        /// </summary>
        public GameObject BackgroundPrefab;

        /// <summary>
        ///     The bounds of the wrapping effect
        /// </summary>
        public Bounds Bounds;

        /// <summary>
        ///     The amount of parallax, negative multiplier of camera movement
        /// </summary>
        public float ParallaxFactor = 0.2f;

        /// <summary>
        ///     The sprite to set on the background tiles
        /// </summary>
        public Sprite Sprite;

        /// <summary>
        ///     The number of background tiles on the X-Axis
        /// </summary>
        public float XCount;

        /// <summary>
        ///     The number of background tiles on the Y-Axis
        /// </summary>
        public float YCount;

        /// <summary>
        ///     Cached list of all the ScrollingBackground objects
        /// </summary>
        private List<ScrollingBackground> backgrounds;

        /// <summary>
        ///     Cached camera
        /// </summary>
        private Camera cam;

        /// <summary>
        ///     The position the camera was in last frame
        /// </summary>
        private Vector3 lastCameraPosition;

        /// <summary>
        ///     Move all the background objects by an amount
        /// </summary>
        /// <param name="offset">Vector distance to move</param>
        public void MoveAllBackgrounds(Vector3 offset)
        {
            foreach (var parallaxBackground in backgrounds)
            {
                parallaxBackground.Move(offset);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(Bounds.center, Bounds.size);
            }
        }
#endif

        /// <summary>
        ///     Set up the initial background objects and set positions
        /// </summary>
        private void SetBackgroundPosition()
        {
#if UNITY_EDITOR
            gameObject.DestroyAllChildrenImmediate("BG");

#endif
            gameObject.DestroyAllChildren("BG");

            if (!Sprite)
            {
                return;
            }

            var spriteDimentions = Sprite.bounds.size;

            var orthographicUnitsPerPixel = 1f / (cam.pixelHeight / (cam.orthographicSize * 2f));

            var screenDimentions = new Vector2(cam.pixelWidth * orthographicUnitsPerPixel, cam.pixelHeight * orthographicUnitsPerPixel);

            var requiredTiles = new Vector2(Mathf.Ceil(screenDimentions.x / spriteDimentions.x) + 2, Mathf.Ceil(screenDimentions.y / spriteDimentions.y) + 2);
            XCount = requiredTiles.x;
            YCount = requiredTiles.y;

            Bounds = new Bounds(transform.position, (Vector2.Scale(requiredTiles, spriteDimentions) / 3f) * 2f);

            backgrounds = new List<ScrollingBackground>();

            for (var x = 0; x < requiredTiles.x; x++)
            {
                for (var y = 0; y < requiredTiles.y; y++)
                {
                    if (BackgroundPrefab != null)
                    {
                        var a = (x * spriteDimentions.x) - ((requiredTiles.x / 2f) * spriteDimentions.x);
                        var b = (y * spriteDimentions.y) - ((requiredTiles.y / 2f) * spriteDimentions.y);

                        var parallaxBackground =
                            Instantiate(BackgroundPrefab)
                                .SetParent(transform)
                                .SetLocalPosition(new Vector3(a, b))
                                .SetName(string.Format("BG: {0},{1}", x, y))
                                .GetComponent<ScrollingBackground>();

                        if (parallaxBackground != null)
                        {
                            parallaxBackground.SetSprite(Sprite);
                            parallaxBackground.SetStartingParameters(this);
                            backgrounds.Add(parallaxBackground);
                        }
                    }
                }
            }
        }

        // Use this for initialization
        private void Start()
        {
            if (!cam)
            {
                cam = Camera.main;
            }

            if (cam)
            {
                lastCameraPosition = cam.transform.position;
            }

            SetBackgroundPosition();
        }

        /// <summary>
        ///     Test function for the Editor
        /// </summary>
        [Button]
        private void Test()
        {
            Start();
            SetBackgroundPosition();
        }

        // Update is called once per frame
        private void Update()
        {
            if (cam.transform.position != lastCameraPosition)
            {
                Bounds.center = transform.position;
                MoveAllBackgrounds((cam.transform.position - lastCameraPosition) * (-ParallaxFactor));
            }

            lastCameraPosition = cam.transform.position;
        }
    }
}