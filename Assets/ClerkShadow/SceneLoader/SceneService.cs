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
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        private const float LoaderAppearAnimation = 0.5f;
        private const int SceneLoadingTransitions = 200;

        private SceneLoaderData _sceneLoaderData;
        private Enums.SceneName _currentScene;

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

            AsyncOperation load = SceneManager.LoadSceneAsync(Enums.GetSceneName(sceneName));
            SceneManager.UnloadSceneAsync(Enums.GetSceneName(_currentScene));
            _currentScene = sceneName;
            HideAnimation(load);
        }

        private async void HideAnimation(AsyncOperation asyncOperation)
        {
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            ForceStopAnimation();
        }

        public void ForceStopAnimation()
        {
            HideLoader();
        }

        public void ShowLoader()
        {
            _sceneLoaderData.LoaderCanvasGroup.gameObject.SetActive(true);
            _sceneLoaderData.LoaderAnimator.enabled = true;
            RunLoaderAnimation();
            LoaderSetActive(true, null);
        }

        private void HideLoader(Action activateSceneCallback = null)
        {
            LoaderSetActive(false, () =>
            {
                _sceneLoaderData.LoaderAnimator.SetBool(IsRunning, false);
                _sceneLoaderData.LoaderAnimator.enabled = false;
                _sceneLoaderData.LoaderCanvasGroup.gameObject.SetActive(false);
                activateSceneCallback?.Invoke();
            });
        }

        private void RunLoaderAnimation()
        {
            _sceneLoaderData.LoaderAnimator.SetBool(IsRunning, true);
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