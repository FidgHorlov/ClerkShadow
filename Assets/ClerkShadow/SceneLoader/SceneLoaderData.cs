using ClerkShadow.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ClerkShadow.SceneLoader
{
    public class SceneLoaderData : MonoBehaviour
    {
        [field: SerializeField] protected internal Image LoaderImage { get; private set; }
        //[field: SerializeField] protected internal Sprite[] LoaderSprites { get; private set; }
        [field: SerializeField] protected internal CanvasGroup LoaderCanvasGroup { get; private set; }
        [field: SerializeField] protected internal Animator LoaderAnimator { get; private set; }
        
#region Services

        private ISceneService _sceneService;
        private ISceneService SceneService => _sceneService ??= Service.Instance.Get<ISceneService>();

#endregion

        private void Start()
        {
            SceneService.Init(this);
        }

        private void OnDisable()
        {
            SceneService.ForceStopAnimation();
        }

#if UNITY_EDITOR
        [ContextMenu("Start")]
        private void StartLoader()
        {
            SceneService.ShowLoader();
        }

        [ContextMenu("Stop")]
        private void StopLoader()
        {
            SceneService.ForceStopAnimation();
        }
        
#endif
    }
}