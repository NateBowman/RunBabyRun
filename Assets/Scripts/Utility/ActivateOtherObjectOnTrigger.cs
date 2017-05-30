//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ActivateOtherObjectOnTrigger.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Utility
{
    using NateTools.Utils;

    using UnityEngine;

    [RequireComponent(typeof(Collider2D))]
    public class ActivateOtherObjectOnTrigger : MonoBehaviour
    {
        public bool DeactivateOnExit = true;

        public LayerMask Mask;

        public GameObject ObjectToActivate;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!Mask.Contains(other.gameObject.layer) || (ObjectToActivate == null))
            {
                return;
            }

            ObjectToActivate.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!DeactivateOnExit || (!Mask.Contains(other.gameObject.layer) || (ObjectToActivate == null)))
            {
                return;
            }

            ObjectToActivate.SetActive(false);
        }
    }
}