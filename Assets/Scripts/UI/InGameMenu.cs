//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="InGameMenu.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace UI
{
    using Level;

    using UnityEngine;

    public class InGameMenu : MonoBehaviour
    {
        protected void Awake()
        {
            SceneController.MenuCanvas = gameObject;
            gameObject.SetActive(false);
            Destroy(this);
        }
    }
}