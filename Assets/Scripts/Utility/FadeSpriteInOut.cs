//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="FadeSpriteInOut.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Utility
{
    using UnityEngine;

    public class FadeSpriteInOut : MonoBehaviour
    {
        private Color colour;

        private float lerptime;

        private SpriteRenderer spriteRenderer;

        private float timer;

        // Use this for initialization
        private void Start()
        {
            lerptime = Random.Range(1, 5f);
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                colour = spriteRenderer.color;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (spriteRenderer)
            {
                timer = timer % (lerptime);
                colour.a = Mathf.Lerp(0.2f, 0.8f, timer / (lerptime));
                spriteRenderer.color = colour;
                timer += Time.deltaTime;
            }
        }
    }
}