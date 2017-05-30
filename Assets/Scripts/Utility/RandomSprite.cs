//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="RandomSprite.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Utility
{
    using UnityEngine;

    /// <summary>
    ///     Sets the sprite to a random sprite from an array on start
    /// </summary>
    public class RandomSprite : MonoBehaviour
    {
        /// <summary>
        ///     Sprites to choose from
        /// </summary>
        public Sprite[] Sprites;

        // Use this for initialization
        private void Start()
        {
            if ((Sprites != null) && (Sprites.Length > 0))
            {
                var spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    var sprite = Sprites[Random.Range(0, Sprites.Length - 1)];
                    if (sprite != null)
                    {
                        spriteRenderer.sprite = sprite;
                    }
                }
            }
        }
    }
}