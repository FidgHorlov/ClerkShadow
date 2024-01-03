using ClerkShadow.Data;
using ClerkShadow.LocalizationSystem;
using ClerkShadow.SceneLoader;
using ClerkShadow.ServiceLocator;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ClerkShadow.Ui
{
    public class UiController : MonoBehaviour
    {
        private const float AnimationButtonTime = 1.5f;
        
        [SerializeField] private Button _menuButton;
        [SerializeField] private CanvasGroup _menuButtonCanvasGroup;
        [Header("Play|Pause button")]
        [SerializeField] private Button _playPauseButton;
        [SerializeField] private Image _playPauseIcon;
        [SerializeField] private Sprite _playSprite;
        [SerializeField] private Sprite _pauseSprite;

        private bool _isPaused = false;
        
        private void OnEnable()
        {
            _menuButton.onClick.AddListener(MenuButtonPressed);
            _playPauseButton.onClick.AddListener(PlayPausePressed);
            SetActiveMenuButton(false, true);
        }
        
        private void OnDisable()
        {
            _menuButton.onClick.RemoveListener(MenuButtonPressed);
            _playPauseButton.onClick.RemoveListener(PlayPausePressed);
            Time.timeScale = 1f;
        }

        private void MenuButtonPressed()
        {
            //SceneManager.LoadScene(Enums.GetSceneName(Enums.SceneName.Menu));
            Service.Instance.Get<ISceneService>().LoadScene(Enums.SceneName.Menu);
        }

        private void PlayPausePressed()
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0f : 1f;
            _playPauseIcon.sprite = _isPaused ? _playSprite : _pauseSprite;
            SetActiveMenuButton(_isPaused, false);
        }

        private void SetActiveMenuButton(bool isActive, bool immediately)
        {
            float targetAlpha =  isActive ? 1f : 0f;
            
            if (immediately)
            {
                _menuButtonCanvasGroup.gameObject.SetActive(isActive);
                _menuButtonCanvasGroup.alpha = targetAlpha;
                return;
            }

            if (isActive)
            {
                _menuButtonCanvasGroup.gameObject.SetActive(true);
            }
            
            _menuButtonCanvasGroup
                .DOFade(isActive ? 1f : 0f, AnimationButtonTime)
                .OnComplete(() =>
                {
                    if (!isActive)
                    {
                        _menuButtonCanvasGroup.gameObject.SetActive(false);
                    }
                })
                .SetUpdate(isIndependentUpdate: true);
        }
    }
}
