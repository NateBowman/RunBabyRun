//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="DamagePlayerOnCollision.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events.Triggers
{
    public class DamagePlayerOnCollision : LayerMaskedCollisionEvent
    {
        /// <inheritdoc />
        protected override void FireEvent()
        {
            EventManager.Raise(new PlayerDamaged(gameObject));
            base.FireEvent();
        }
    }
}