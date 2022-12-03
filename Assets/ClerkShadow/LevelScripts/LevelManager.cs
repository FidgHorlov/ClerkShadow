using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClerkShadow.LevelScripts
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private GameController _gameController;
        [SerializeField] private Image _loaderScreen;
        [SerializeField] private List<Sprite> _loaderSprites;
        [SerializeField] private Sprite _finalSprite;

        private int _currentLevelID;
        private const float LevelDelay = 4f; 
        private Level _currentlyLoadedLevel;

        void Start()
        {
            //ResetLevelID();
            //LoadLevel();
            StartCoroutine(SetPicture());
        }

        private IEnumerator SetPicture()
        {
            _loaderScreen.gameObject.SetActive(true);
            _currentLevelID = LoadLevelID();
            _loaderScreen.sprite = _loaderSprites[_currentLevelID];
            yield return new WaitForSeconds(LevelDelay);
            _loaderScreen.gameObject.SetActive(false);
            LoadLevel();
        }

        private void LoadLevel()
        {
            _currentlyLoadedLevel = Instantiate(_levelData.Levels[_currentLevelID]);
            _gameController.SetTimeForLevel(_currentlyLoadedLevel.LevelDuration);
            SubscribeOnComplete();
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

        public void OnLevelComplete(Collider2D obj)
        {
            Debug.Log(_currentlyLoadedLevel.gameObject);
            Destroy(_currentlyLoadedLevel.gameObject);
            _gameController.ResetGameState();
            _currentLevelID++;
            if (_currentLevelID > 2)
            {
                _loaderScreen.sprite = _finalSprite;
                _currentLevelID = 0;
                SaveLevelID();
                return;
            }
        
            SaveLevelID();
            StartCoroutine(SetPicture());
        }

        private void SaveLevelID()
        {
            PlayerPrefs.SetInt("CurrentLevel", _currentLevelID);
        }

        private int LoadLevelID()
        {
            int currentLevelId = PlayerPrefs.GetInt("CurrentLevel", _currentLevelID);
            if (currentLevelId > 2)
            {
                currentLevelId = 2;
            }

            return currentLevelId;
        }

        private void ResetLevelID()
        {
            PlayerPrefs.SetInt("CurrentLevel", 0);
        }
    }
}
