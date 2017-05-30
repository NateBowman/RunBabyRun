//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="WrapScrollingItems.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Utility
{
    using System.Collections.Generic;
    using System.Linq;

    using NateTools.Utils;

    using UnityEngine;

    /// <summary>
    ///     Cheap scrolling terrain for the intro section
    ///     <remarks>All items must be the same sprite and size or it wont work properly</remarks>
    /// </summary>
    public class WrapScrollingItems : MonoBehaviour
    {
        [Range(-5f, 5f)]
        public float SpeedX = 1f;

        private List<SpriteRenderer> sprites;

        // Use this for initialization
        private void Start()
        {
            sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        }

        // Update is called once per frame
        private void Update()
        {
            foreach (var sprite in sprites)
            {
                sprite.gameObject.transform.position = sprite.gameObject.transform.position.Add(SpeedX * Time.deltaTime);
                if (sprite.gameObject.transform.position.x < -((sprite.sprite.bounds.size.x * sprites.Count) / 2f))
                {
                    sprite.gameObject.transform.position = sprite.gameObject.transform.position.Add(sprite.sprite.bounds.size.x * sprites.Count);
                }

                if (sprite.gameObject.transform.position.x > ((sprite.sprite.bounds.size.x * sprites.Count) / 2f))
                {
                    sprite.gameObject.transform.position = sprite.gameObject.transform.position.Add(-sprite.sprite.bounds.size.x * sprites.Count);
                }
            }
        }
    }
}