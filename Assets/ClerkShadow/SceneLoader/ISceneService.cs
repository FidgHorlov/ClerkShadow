using System;
using ClerkShadow.Data;
using ClerkShadow.ServiceLocator;

namespace ClerkShadow.SceneLoader
{
    public interface ISceneService : IService
    {
        void Init(SceneLoaderData sceneLoaderData);
        void LoadScene(Enums.SceneName sceneName);
        void ForceStopAnimation();
        void ShowLoader();
    }
}