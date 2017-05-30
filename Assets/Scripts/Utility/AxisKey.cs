//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="AxisKey.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Utility
{
    using UnityEngine;

    /// <summary>
    ///     This is a wrapper class for boolean Axial input, it emulates the KeyUp/KeyDown properties of the Unity Input class
    /// </summary>
    public class AxisKey
    {
        public bool axisUp = false;

        public bool currentState = false;

        private bool axisDown = false;

        private bool lastState = false;

        public AxisKey(string axis)
        {
            Axis = axis;
        }

        public string Axis { get; private set; }

        public bool AxisDown { get { return axisDown; } }

        public bool AxisUp { get { return axisUp; } }

        public bool CurrentState { get { return currentState; } set { currentState = value; } }

        public void Update()
        {
            currentState = Input.GetAxis(Axis) > 0;

            if (lastState && !currentState)
            {
                axisDown = false;
                axisUp = true;
            }
            else if (!lastState && currentState)
            {
                axisDown = true;
                axisUp = false;
            }
            else
            {
                axisUp = axisDown = false;
            }

            lastState = currentState;
        }
    }
}