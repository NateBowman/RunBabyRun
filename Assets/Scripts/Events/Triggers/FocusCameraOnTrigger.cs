//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="FocusCameraOnTrigger.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events.Triggers
{
    using NateTools.Attributes;

    using UnityEngine;

    /// <summary>
    ///     Sets the focus of the scenes CameraController to this object then triggered
    /// </summary>
    public class FocusCameraOnTrigger : LayerMaskedTriggerEvent
    {
        /// <summary>
        ///     The target to focus the camera on
        /// </summary>
        [Comment("Focus the camera on the other GameObject, on this GameObject if empty")]
        public GameObject Target;

        /// <summary>
        ///     Cached CAmeraController
        /// </summary>
        private CameraController cam;

        /// <summary>
        ///     Cached camera controller mode for restoration after focus is over
        /// </summary>
        private CameraController.CameraMode mode = CameraController.CameraMode.Static;

        /// <summary>
        ///     Flag for making sure the action fires only once at a time in the case of multiple colliders on an object
        /// </summary>
        private bool triggered;

        /// <summary>
        ///     The event Execution routine
        /// </summary>
        protected override void FireEvent()
        {
            if (triggered)
            {
                if (cam)
                {
                    cam.FocusTarget = null;
                    cam.Mode = mode;
                }
                triggered = false;
            }
            else
            {
                if (cam)
                {
                    cam.FocusTarget = Target != null ? Target : gameObject;
                    mode = cam.Mode;
                    cam.Mode = CameraController.CameraMode.MultiFocus;
                }
                triggered = true;
            }

            base.FireEvent();
        }

        private void Start()
        {
            cam = FindObjectOfType<CameraController>();
        }
    }
}