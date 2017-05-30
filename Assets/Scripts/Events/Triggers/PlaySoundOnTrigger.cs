//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="PlaySoundOnTrigger.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events.Triggers
{
    using UnityEngine;

    public class PlaySoundOnTrigger : LayerMaskedTriggerEvent
    {
        /// <inheritdoc />
        protected override void FireEvent()
        {
            var audioSrc = GetComponent<AudioSource>();
            if (audioSrc)
            {
                audioSrc.Play();
            }
            base.FireEvent();
        }
    }
}