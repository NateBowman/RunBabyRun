//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="ScoreText.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace UI
{
    using Events;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Sets The value of an attached text field to the score value
    /// </summary>
    public class ScoreText : MonoBehaviour
    {
        private Text scoreText;

        private int scoreValue;

        private void OnDisable()
        {
            EventManager.RemoveListener<ScoreChanged>(SetScore);
        }

        private void OnEnable()
        {
            EventManager.AddListener<ScoreChanged>(SetScore);
        }

        private void SetScore(GameEvent ge)
        {
            var e = ge as ScoreChanged;
            if (e != null)
            {
                scoreValue = e.Value;
            }
        }

        // Use this for initialization
        private void Start()
        {
            scoreText = GetComponent<Text>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (scoreText != null)
            {
                scoreText.text = string.Format("{0:D7}", scoreValue);
            }
        }
    }
}