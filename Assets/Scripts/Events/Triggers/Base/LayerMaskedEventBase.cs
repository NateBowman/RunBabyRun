//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="LayerMaskedEventBase.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events.Triggers
{
    using NateTools.Utils;

    using UnityEngine;

    public abstract class LayerMaskedEventBase : MonoBehaviour
    {
        /// <summary>
        ///     Does the trigger have a cap on the number of executions
        /// </summary>
        public bool InfiniteExecutions = true;

        /// <summary>
        ///     The mask for object interaction restrictions
        /// </summary>
        public LayerMask Layer;

        /// <summary>
        ///     The number of FireEvents calls remaining
        /// </summary>
        [SerializeField]
        private int remainingExecutions;

        /// <summary>
        ///     Gets or sets the RemainingExecutions
        /// </summary>
        public int RemainingExecutions { get { return remainingExecutions; } set { remainingExecutions = Mathf.Clamp(value, 0, int.MaxValue); } }

        /// <summary>
        ///     Can the event be fired
        /// </summary>
        /// <returns>True if the event can be fired</returns>
        protected virtual bool CanFire()
        {
            return InfiniteExecutions || (RemainingExecutions > 0);
        }

        /// <summary>
        ///     The event Execution routine
        /// </summary>
        protected virtual void FireEvent()
        {
            if (!InfiniteExecutions)
            {
                RemainingExecutions--;
            }
        }

        /// <summary>
        ///     Does the layer provided match the required mask
        /// </summary>
        /// <param name="layerInt">The int value of the layer to check</param>
        /// <returns>True if the layer is in the mask</returns>
        protected bool MaskValidFor(int layerInt)
        {
            return Layer.Contains(layerInt);
        }
    }
}