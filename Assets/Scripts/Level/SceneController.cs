//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="SceneController.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Level
{
    using System;
    using System.Collections;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class SceneController : MonoBehaviour
    {
        public static CanvasGroup canvasGroup;

        /// <summary>
        ///     TODO: Not Yet Implemented
        /// </summary>
        public static GameObject EndGameCanvas;

        public static GameObject MenuCanvas;

        private static SceneNames _currentScene = SceneNames.MainMenu;

        private static SceneController _instance;

        private static Coroutine _loadingCoroutine;

        private static bool _menuActive = true;

        [NonSerialized]
        private bool firstUpdate = true;

        [NonSerialized]
        private bool menuButtonPressed = false;

        public enum SceneNames
        {
            MainMenu = 0,

            Play = 1,

            Credits = 2
        }

        public void CloseMenu()
        {
            if (_menuActive)
            {
                if (MenuCanvas != null)
                {
                    MenuCanvas.SetActive(true);
                }
                _menuActive = false;
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ExitToMainMenu()
        {
            _currentScene = SceneNames.MainMenu;
            RestartScene();
        }

        public void FadeFromBlack()
        {
            _instance.StartCoroutine(DoFadeAlpha(0f, 2f));
        }

        public void FadeFromBlack(Action callback)
        {
            _instance.StartCoroutine(DoFadeAlpha(0f, 2f, callback));
        }

        public void FadeToBlack()
        {
            _instance.StartCoroutine(DoFadeAlpha(1f, 2f));
        }

        public void FadeToBlack(Action callback)
        {
            _instance.StartCoroutine(DoFadeAlpha(1f, 2f, callback));
        }

        public void OpenEndMenu()
        {
            EndGameCanvas.SetActive(true);
        }

        public void OpenMenu()
        {
            MenuCanvas.SetActive(true);
            _menuActive = true;
        }

        public void OpenScene(int scene)
        {
            StartScene((SceneNames)scene);
            CloseMenu();
        }

        public void RestartScene()
        {
            StartScene(_currentScene);
            CloseMenu();
        }

        protected void Awake()
        {
            if (FindObjectsOfType<SceneController>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);
        }

        private IEnumerator DoFadeAlpha(float to, float time, Action callback = null)
        {
            var current = canvasGroup.alpha = 1 - to;

            var interval = 0f;

            while (interval < time)
            {
                canvasGroup.alpha = Mathf.Lerp(current, to, interval / (1f / time));
                yield return false;

                interval += Time.deltaTime;
            }

            if (callback != null)
            {
                callback.Invoke();
            }
        }

        private IEnumerator LoadLevel(int i)
        {
            yield return new WaitForSeconds(0.1f);

            var counter = i;
            while ((counter > 0) && (FindObjectOfType<LevelManager>() == null))
            {
                counter--;
                yield return null;
            }

            var level = FindObjectOfType<LevelManager>();

            if (level != null)
            {
                level.StartGame();
            }
            yield return true;
        }

        // Use this for initialization
        private void Start()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        private void StartScene(SceneNames scene)
        {
            FadeToBlack(
                () =>
                    {
                        StartSceneNow(scene);
                        FadeFromBlack();
                    });
        }

        private void StartSceneNow(SceneNames scene)
        {
            if (_loadingCoroutine != null)
            {
                _instance.StopCoroutine(_loadingCoroutine);
            }
            SceneManager.LoadScene((int)scene);
            _loadingCoroutine = _instance.StartCoroutine(LoadLevel(100));
            _currentScene = scene;
            if (_currentScene != SceneNames.MainMenu)
            {
                CloseMenu();
            }
        }

        private void Update()
        {
            if (_currentScene == SceneNames.MainMenu)
            {
                _menuActive = true;
            }

            var keydown = Input.GetKey(KeyCode.Escape);

            if (keydown && !menuButtonPressed && (_currentScene != SceneNames.MainMenu))
            {
                _menuActive = !_menuActive;
                menuButtonPressed = true;
            }
            menuButtonPressed = keydown;

            if (MenuCanvas != null)
            {
                MenuCanvas.SetActive(_menuActive);
            }

            if (!firstUpdate)
            {
                return;
            }

            FadeFromBlack();
            firstUpdate = false;
        }
    }
}