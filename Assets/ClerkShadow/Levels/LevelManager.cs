using System.Collections;
using System.Collections.Generic;
using ClerkShadow.Data;
using ClerkShadow.LocalizationSystem;
using ClerkShadow.ServiceLocator;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClerkShadow.Levels
{
    public class LevelManager : MonoBehaviour
    {
        private const string ChapterLocalizationId = "chapterName";

        private const float AnimationDuration = 0.5f;
        private const float LevelDelay = 4f;

        [SerializeField] private GameController _gameController;
        [SerializeField] private Sprite _finalSprite;

        [Header("Level UI info")]
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField] private TextMeshProUGUI _finalDescription;
        [SerializeField] private Image _backgroundColor;
        [Space] [SerializeField] private Image _chapterIcon;
        [SerializeField] private TextMeshProUGUI _levelCaption;
        [SerializeField] private TextMeshProUGUI _levelDescription;
        [Space] [SerializeField] private List<Level> _levelDataList;
        
        [Space] 
        [SerializeField] private Camera _camera;

        private Level _currentlyLoadedLevel;
        private Enums.Language _currentLanguage;
        private string _currentChapterName;

        private int _currentLevelID;
        private bool _wasFinalStage;

#region Services

        private ILocalization _localization;
        private ILocalization Localization => _localization ??= Service.Instance.Get<ILocalization>();

#endregion

        private void Start()
        {
            foreach (Level level in _levelDataList)
            {
                level.SetActive(false);
            }

            InitLevelNumbers();
            _currentChapterName = GetTranslatedChapterName();
            StartCoroutine(SetPicture());
        }

        private void InitLevelNumbers()
        {
            for (int levelIndex = 0; levelIndex < _levelDataList.Count; levelIndex++)
            {
                Level.LevelNumber levelNumber = Level.LevelNumber.Default;
                if (levelIndex == _levelDataList.Count - 2)
                {
                    levelNumber = Level.LevelNumber.BeforeLast;
                }

                if (levelIndex == _levelDataList.Count - 1)
                {
                    levelNumber = Level.LevelNumber.Last;
                }

                _levelDataList[levelIndex].CurrentLevelNumber = levelNumber;
            }
        }

        private IEnumerator SetPicture()
        {
            _currentLevelID = LoadLevelID();
            _currentlyLoadedLevel = _levelDataList[_currentLevelID];
            SetLevelData();
            ShowHideCanvas(true);
            yield return new WaitForSeconds(LevelDelay);
            ShowHideCanvas(false);
            LoadLevel();
        }

        private void SetIfFinalStage(bool isFinalStage)
        {
            _chapterIcon.gameObject.SetActive(isFinalStage);
            _levelCaption.gameObject.SetActive(isFinalStage);
            _levelDescription.gameObject.SetActive(isFinalStage);
            _finalDescription.gameObject.SetActive(!isFinalStage);
            _backgroundColor.color = isFinalStage ? Color.black : Color.white;
            _wasFinalStage = !isFinalStage;
        }
        
        private void SetLevelData()
        {
            if (_wasFinalStage)
            {
                SetIfFinalStage(true);
                return;
            }

            if (_currentlyLoadedLevel.LoadingSprite == null)
            {
                SetIfFinalStage(false);
                _finalDescription.text = _currentlyLoadedLevel.GetLevelDescription();
            }
            else
            {
                _chapterIcon.sprite = _currentlyLoadedLevel.LoadingSprite;
                _levelDescription.text = _currentlyLoadedLevel.GetLevelDescription();
                SetLevelCaption(_currentLevelID + 1);
            }
        }

        private void SetLevelCaption(int currentLevelID)
        {
            string chapter = _currentLanguage != Localization.CurrentLanguage ? _currentChapterName : GetTranslatedChapterName();
            _levelCaption.text = chapter + " " + currentLevelID;
        }

        private void ShowHideCanvas(bool isActive, float time = -1f)
        {
            if (isActive)
            {
                _canvasGroup.gameObject.SetActive(true);
            }

            float duration = time < 0 ? AnimationDuration : time;
            _canvasGroup.DOKill(_canvasGroup);
            _canvasGroup.DOFade(isActive ? 1f : 0f, duration)
                .SetId(_canvasGroup)
                .OnComplete(() =>
                {
                    if (!isActive)
                    {
                        _canvasGroup.gameObject.SetActive(false);
                    }
                })
                .SetUpdate(isIndependentUpdate: true);
        }

        private void LoadLevel()
        {
            _currentlyLoadedLevel = ShowLevel();
            _gameController.SetTimeForLevel(_currentlyLoadedLevel.LevelDuration);
            SubscribeOnComplete();
        }

        private Level ShowLevel()
        {
            _currentlyLoadedLevel.SetActive(false);
            _levelDataList[_currentLevelID].SetActive(true);
            return _levelDataList[_currentLevelID];
        }

        private void SubscribeOnComplete()
        {
            Debug.Log(_currentlyLoadedLevel.Exit);
            _currentlyLoadedLevel.Exit.TriggerEntered += OnLevelComplete;
        }

        private void UnsubscribeOnComplete()
        {
            _currentlyLoadedLevel.Exit.TriggerEntered -= OnLevelComplete;
        }

        private void OnLevelComplete(Collider2D obj)
        {
            Debug.Log(_currentlyLoadedLevel.gameObject);
            Destroy(_currentlyLoadedLevel.gameObject);
            _gameController.ResetGameState();
            _currentLevelID++;
            if (_currentLevelID > _levelDataList.Count)
            {
                _chapterIcon.sprite = _finalSprite;
                _currentLevelID = 0;
                SaveLevelID();
                UnsubscribeOnComplete();
                return;
            }

            SaveLevelID();
            StartCoroutine(SetPicture());
        }

        private string GetTranslatedChapterName() => Localization.GetTranslatedValue(Enums.LocalizationTable.Messages, ChapterLocalizationId);

        private void SaveLevelID()
        {
            PlayerPrefs.SetInt(Constants.PlayerPrefsName.CurrentLevel, _currentLevelID);
        }

        private int LoadLevelID()
        {
            int currentLevelId = PlayerPrefs.GetInt(Constants.PlayerPrefsName.CurrentLevel, _currentLevelID);
            if (currentLevelId < _levelDataList.Count)
            {
                return currentLevelId;
            }

            currentLevelId = 0;
            ResetLevelID();
            return currentLevelId;
        }

        private void ResetLevelID()
        {
            PlayerPrefs.SetInt(Constants.PlayerPrefsName.CurrentLevel, 0);
        }

#if UNITY_EDITOR
        public void SetCustomLevelEditor(int level)
        {
            PlayerPrefs.SetInt(Constants.PlayerPrefsName.CurrentLevel, level);   
        }

        public void ResetLevelEditor()
        {
            SetCustomLevelEditor(0);
        }
        
        public int GetCurrentLevelEditor() => PlayerPrefs.GetInt(Constants.PlayerPrefsName.CurrentLevel, _currentLevelID);

#endif
    }
}