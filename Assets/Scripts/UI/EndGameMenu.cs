//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="EndGameMenu.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace UI
{
    using Events;

    using Level;

    using UnityEngine;
    using UnityEngine.UI;

    public class EndGameMenu : MonoBehaviour
    {
        public Text Jump;

        public GameObject MenuPanel;

        public Text Score;

        public Text Sections;

        protected void Awake()
        {
            SceneController.EndGameCanvas = gameObject;
        }

        private void DoEndGame(GameEvent arg0)
        {
            var e = arg0 as EndGame;
            if (e == null)
            {
                return;
            }

            if (MenuPanel != null)
            {
                MenuPanel.SetActive(true);
            }

            var stats = e.Value;
            if (stats != null)
            {
                Jump.text = stats.Jumps.ToString();
                Score.text = stats.Score.ToString();
                Sections.text = stats.FinalChunk.ToString();
            }

            // workaround for a menu input issue
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<EndGame>(DoEndGame);
        }

        private void OnEnable()
        {
            EventManager.AddListener<EndGame>(DoEndGame);
        }
    }
}