//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="EnemySteamGenerator.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Entity
{
    using Player;

    using UnityEngine;

    public class EnemySteamGenerator : MonoBehaviour
    {
        public float encroachSpeed = 3f;

        public float MaxDistanceX = 10f;

        public float MinDistanceX = 1f;

        private GameObject player;

        // Use this for initialization
        private void Start()
        {
            player = FindObjectOfType<PlayerController>().gameObject;
            gameObject.transform.parent = null;
        }

        // Update is called once per frame
        private void Update()
        {
            var pos = transform.position;

            pos.y = player.transform.position.y;
            pos.x += encroachSpeed * Time.deltaTime;

            if (player)
            {
                var distance = player.transform.position.x - pos.x;

                if (distance < MinDistanceX)
                {
                    return;
                }

                if (distance > MaxDistanceX)
                {
                    pos.x = player.transform.position.x - MaxDistanceX;
                }
            }

            transform.position = pos;
        }
    }
}