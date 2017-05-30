//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ScrollingBackground.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Terrain
{
    using NateTools.Utils;

    using UnityEngine;

    /// <summary>
    ///     Very simple movable image that will wrap around once it exits its parent ParallaxField bounds
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ScrollingBackground : MonoBehaviour
    {
        /// <summary>
        ///     Height of the sprite
        /// </summary>
        private float height;

        /// <summary>
        ///     Left over movement needed in subsequent frames
        /// </summary>
        private Vector2 movementRequired = Vector2.zero;

        /// <summary>
        ///     The parent field
        /// </summary>
        private ParallaxField parentField;

        /// <summary>
        ///     Width of the sprite
        /// </summary>
        private float width;

        /// <summary>
        ///     Move the Sprite
        /// </summary>
        /// <param name="offset">The distance to move the sprite</param>
        public void Move(Vector2 offset)
        {
            // add new movement to the left over from the last frame
            movementRequired += offset;

            // work with the full movement required
            var movementToExecute = movementRequired;

            // calculate pixel perfect movement factor
            var pixelsPerUnit = Camera.main.pixelHeight / (Camera.main.orthographicSize * 2);
            var unitsPerPixel = 1f / pixelsPerUnit;

            // lock the new position to the pixel grid
            movementToExecute.x = Mathf.Round(movementToExecute.x / unitsPerPixel) * unitsPerPixel;
            movementToExecute.y = Mathf.Round(movementToExecute.y / unitsPerPixel) * unitsPerPixel;

            // remove any movement we are going to do from total required
            movementRequired -= movementToExecute;

            // Move the GameObject to its new location
            transform.position = transform.position.Add(movementToExecute);

            var extraMovement = Vector2.zero;

            // Check for wrapping requirements
            if ((transform.position.x + width) < parentField.Bounds.min.x)
            {
                extraMovement.x = width * parentField.XCount;
            }
            else if (transform.position.x > parentField.Bounds.max.x)
            {
                extraMovement.x = -width * parentField.XCount;
            }

            if ((transform.position.y + height) < parentField.Bounds.min.y)
            {
                extraMovement.y = height * parentField.YCount;
            }
            else if (transform.position.y > parentField.Bounds.max.y)
            {
                extraMovement.y = -height * parentField.YCount;
            }

            // Add any extra movement required for wrapping
            transform.position = transform.position.Add(extraMovement);
        }

        /// <summary>
        ///     Set the sprite components sprite
        /// </summary>
        /// <param name="sprite">The sprite to assign</param>
        public void SetSprite(Sprite sprite)
        {
            // One shot on init, does not need caching
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // set the sprite and cache the dimensions
                spriteRenderer.sprite = sprite;
                width = spriteRenderer.sprite.bounds.size.x;
                height = spriteRenderer.sprite.bounds.size.y;
            }
        }

        /// <summary>
        ///     Assign this object a parent field
        /// </summary>
        /// <param name="parent">The parent</param>
        public void SetStartingParameters(ParallaxField parent)
        {
            parentField = parent;
        }
    }
}