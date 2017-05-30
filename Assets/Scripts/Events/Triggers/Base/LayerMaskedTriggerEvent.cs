//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="LayerMaskedTriggerEvent.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events.Triggers
{
    using System;

    using UnityEngine;

    /// <summary>
    ///     Base class that to fire a <see cref="GameEvent" /> on a collision type, with conditions
    /// </summary>
    public abstract class LayerMaskedTriggerEvent : LayerMaskedEventBase
    {
        public TriggerType TriggerOnType = TriggerType.Enter;

        /// <summary>
        ///     Type of trigger to execute on
        ///     <remarks>Quick and dirty solution rather than write Editor code</remarks>
        /// </summary>
        [Flags]
        public enum TriggerType
        {
            None = 0x0,

            Enter = 0x1,

            Exit = 0x2,

            EnterAndExit = 0x3,

            Stay = 0x4,

            StayAndEnter = 0x5,

            StayAndExit = 0x6,

            StayAndEnterAndExit = 0x7
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // where is enum.HasFlag? :'(
            if ((TriggerOnType & TriggerType.Enter) == TriggerType.Enter)
            {
                if (MaskValidFor(other.gameObject.layer))
                {
                    if (CanFire())
                    {
                        FireEvent();
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if ((TriggerOnType & TriggerType.Exit) == TriggerType.Exit)
            {
                if (MaskValidFor(other.gameObject.layer))
                {
                    if (CanFire())
                    {
                        FireEvent();
                    }
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if ((TriggerOnType & TriggerType.Stay) == TriggerType.Stay)
            {
                if (MaskValidFor(other.gameObject.layer))
                {
                    if (CanFire())
                    {
                        FireEvent();
                    }
                }
            }
        }
    }
}