//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ScorePointsOnTrigger.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events.Triggers
{
    public class ScorePointsOnTrigger : LayerMaskedTriggerEvent
    {
        public int PointValue = 1;

        /// <inheritdoc />
        protected override void FireEvent()
        {
            EventManager.Raise(new ScoredPoints(gameObject, PointValue));
            base.FireEvent();
        }
    }
}