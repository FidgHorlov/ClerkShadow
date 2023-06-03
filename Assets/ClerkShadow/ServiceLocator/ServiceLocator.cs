using ClerkShadow.LocalizationSystem;
using UnityEngine;

namespace ClerkShadow.ServiceLocator
{
    public static class ServiceLocator
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            Service.Initialize();
            Service.Instance.Register<ILocalization>(new LocalizationService());
        }
    }
}