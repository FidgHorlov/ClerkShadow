using System;
using System.Threading.Tasks;
using ClerkShadow.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClerkShadow.SceneLoader
{
    public class SceneService : ISceneService
    {
        private const float LoaderAppearAnimation = 0.5f;
        private const int LoaderAnimation = 150;
        private const int SceneLoadingTransitions = 200;

        private SceneLoaderData _sceneLoaderData;
        private Enums.SceneName _currentScene;
        private bool _isAnimationPaused;

        public void Init(SceneLoaderData sceneLoaderData)
        {
            _sceneLoaderData = sceneLoaderData;
            HideLoaderImmediately();
        }

        public void LoadScene(Enums.SceneName sceneName)
        {
            if (_currentScene == Enums.SceneName.Menu)
            {
                ShowLoader();
            }

            HideLoader();
            SceneManager.LoadSceneAsync(Enums.GetSceneName(sceneName));
            SceneManager.UnloadSceneAsync(Enums.GetSceneName(_currentScene));
            _currentScene = sceneName;
        }

        public void ForceStopAnimation()
        {
            _isAnimationPaused = true;
        }

        public void ShowLoader()
        {
            _sceneLoaderData.LoaderCanvasGroup.gameObject.SetActive(true);
            RunLoaderAnimation();
            LoaderSetActive(true, () =>
            {
                _isAnimationPaused = false;
            });
        }

        private void HideLoader(Action activateSceneCallback = null)
        {
            DOTween.Sequence().SetDelay(SceneLoadingTransitions).OnComplete(() =>
            {
                LoaderSetActive(false, () =>
                {
                    _isAnimationPaused = true;
                    _sceneLoaderData.LoaderCanvasGroup.gameObject.SetActive(false);
                    activateSceneCallback?.Invoke();
                });
            });
        }

        private async void RunLoaderAnimation()
        {
            int spriteIndex = 0;
            while (!_isAnimationPaused)
            {
                await Task.Delay(LoaderAnimation);
                spriteIndex++;
                if (spriteIndex == _sceneLoaderData.LoaderSprites.Length - 1)
                {
                    spriteIndex = 0;
                }

                _sceneLoaderData.LoaderImage.sprite = _sceneLoaderData.LoaderSprites[spriteIndex];
            }
        }

        private void LoaderSetActive(bool isActive, Action loaderFinishCallback)
        {
            DOTween.Kill(_sceneLoaderData.LoaderCanvasGroup);
            _sceneLoaderData.LoaderCanvasGroup
                .DOFade(isActive ? 1f : 0f, LoaderAppearAnimation)
                .OnComplete(() => loaderFinishCallback?.Invoke())
                .SetId(_sceneLoaderData.LoaderCanvasGroup);
        }

        private void HideLoaderImmediately()
        {
            _sceneLoaderData.LoaderCanvasGroup.alpha = 0f;
            _sceneLoaderData.LoaderCanvasGroup.gameObject.SetActive(false);
        }
    }
}