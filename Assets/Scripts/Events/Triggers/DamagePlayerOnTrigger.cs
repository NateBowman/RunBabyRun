//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="DamagePlayerOnTrigger.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events.Triggers
{
    public class DamagePlayerOnTrigger : LayerMaskedTriggerEvent
    {
        public int Damage = 1;

        /// <inheritdoc />
        protected override void FireEvent()
        {
            EventManager.Raise(new PlayerDamaged(gameObject, Damage));
            base.FireEvent();
        }
    }
}