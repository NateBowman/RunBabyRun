//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="EndOfTheWorld.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Entity
{
    using System.Collections;

    using UnityEngine;

    public class EndOfTheWorld : MonoBehaviour
    {
        public AudioSource ExplosionNoise;

        public ParticleSystem ExplosionParticles;

        public GameObject FactoryAudio;

        public float MaxPitch;

        public AudioSource PanicNoise;

        public float RampUpTime;

        public GameObject SteamEnemy;

        private IEnumerator DoEffects()
        {
            if (PanicNoise != null)
            {
                yield return StartCoroutine(RampPitch());
            }

            yield return StartCoroutine(Explode());

            SpawnSteamWave();
        }

        private IEnumerator Explode()
        {
            if (PanicNoise != null)
            {
                PanicNoise.Stop();
            }

            if (ExplosionNoise != null)
            {
                ExplosionNoise.Play();
                if (ExplosionParticles != null)
                {
                    ExplosionParticles.Play();
                }
                if (FactoryAudio != null)
                {
                    FactoryAudio.SetActive(false);
                }
                yield return new WaitWhile(() => ExplosionNoise.time < ExplosionNoise.clip.length / 2f);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(DoEffects());
        }

        private IEnumerator RampPitch()
        {
            if (PanicNoise != null)
            {
                var pitch = PanicNoise.pitch;

                float timeCounter = 0;

                while (timeCounter < RampUpTime)
                {
                    PanicNoise.pitch = Mathf.Lerp(pitch, MaxPitch, timeCounter * (1f / RampUpTime));
                    yield return false;

                    timeCounter += Time.deltaTime;
                }
            }
        }

        private void SpawnSteamWave()
        {
            SteamEnemy.SetActive(true);
        }

        private void Start()
        {
            gameObject.SetActive(false);
            if (SteamEnemy)
            {
                SteamEnemy.transform.parent = null;
            }
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}