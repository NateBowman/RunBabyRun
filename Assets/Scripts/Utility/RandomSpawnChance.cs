//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="RandomSpawnChance.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Utility
{
    using NateTools.Utils;

    using UnityEngine;

    /// <summary>
    ///     Randomly spawn a prefab based on a chance percentage
    /// </summary>
    public class RandomSpawnChance : MonoBehaviour
    {
        /// <summary>
        ///     percentage chance to spawn the prefab
        /// </summary>
        [Range(0f, 1f)]
        public float ChanceToSpawn;

        /// <summary>
        ///     Prefab to spawn
        /// </summary>
        public GameObject Prefab;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
#endif

        private void Start()
        {
            if (Random.value < ChanceToSpawn)
            {
                (Instantiate(Prefab, transform.position, Quaternion.identity) as GameObject).SetParent(transform);
            }
        }
    }
}