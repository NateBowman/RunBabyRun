//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="PlaySoundOnParticleEmission.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Sound
{
    using UnityEngine;

    [RequireComponent(typeof(ParticleSystem), typeof(AudioSource))]
    public class PlaySoundOnParticleEmission : MonoBehaviour
    {
        /// <summary>
        ///     Cached audio source
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        ///     The number of active particles last frame
        /// </summary>
        private int lastParticleCount = 0;

        /// <summary>
        ///     Cached particle system
        /// </summary>
        private ParticleSystem particleSys;

        /// <summary>
        ///     Initialization of fields
        /// </summary>
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            particleSys = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            // check if there is a new particle emitted
            if (particleSys.particleCount > lastParticleCount)
            {
                audioSource.Play();
            }

            // reset particle count to current
            lastParticleCount = particleSys.particleCount;
        }
    }
}