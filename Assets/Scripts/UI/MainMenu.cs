//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="MainMenu.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace UI
{
    using System;
    using System.Collections;

    using Level;

    using UnityEngine;

    public class MainMenu : MonoBehaviour
    {
        private int _lt = -10;

        private SceneController sceneController;

        // Update is called once per frame
        [NonSerialized]
        private bool firstUpdate = true;

        private IEnumerator FadeIn()
        {
            var canvasGroup = gameObject.GetComponent<CanvasGroup>();

            var current = canvasGroup.alpha = 0;

            var interval = 0f;

            while (canvasGroup.alpha != 1)
            {
                canvasGroup.alpha = Mathf.Lerp(current, 1f, interval / 2f);
                yield return false;

                interval += Time.deltaTime;
            }
        }

        // Use this for initialization
        private void Start()
        {
            sceneController = FindObjectOfType<SceneController>();
        }

        private void Update()
        {
            if (!firstUpdate)
            {
                return;
            }

            sceneController.FadeFromBlack(() => StartCoroutine(FadeIn()));
            firstUpdate = false;
        }
    }
}