using ClerkShadow.ServiceLocator;
using UnityEngine;
using UnityEngine.UI;

namespace ClerkShadow.SceneLoader
{
    public class SceneLoaderData : MonoBehaviour
    {
        [field: SerializeField] protected internal Image LoaderImage { get; private set; }
        [field: SerializeField] protected internal Sprite[] LoaderSprites { get; private set; }
        [field: SerializeField] protected internal CanvasGroup LoaderCanvasGroup { get; private set; }
        
#region Services

        private ISceneService _sceneService;
        private ISceneService SceneService => _sceneService ??= Service.Instance.Get<ISceneService>();

#endregion

        private void Start()
        {
            SceneService.Init(this);
        }

        private void OnDestroy()
        {
            SceneService.ForceStopAnimation();
        }
    }
}