//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="HideOnEndgame.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using UnityEngine;

    public class HideOnEndgame : MonoBehaviour
    {
        private void HideMe(GameEvent arg0)
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<EndGame>(HideMe);
        }

        private void OnEnable()
        {
            EventManager.AddListener<EndGame>(HideMe);
        }
    }
}