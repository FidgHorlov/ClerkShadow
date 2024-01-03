using ClerkShadow.Data;
using ClerkShadow.LocalizationSystem;
using ClerkShadow.ServiceLocator;
using UnityEngine;

namespace ClerkShadow.LevelScripts
{
    public class Level : MonoBehaviour
    {
        internal enum LevelNumber
        {
            Default,
            BeforeLast,
            Last
        }

        private const string BeforeLastChapterLocalizationId = "beforeLastChapter";
        private const string LastChapterLocalizationId = "lastChapter";

        [SerializeField] private LevelData _levelData;
        [SerializeField] private LevelExit _exit;

        private string _levelDescription;
        private Enums.Language _currentLanguage;

        public LevelExit Exit => _exit;
        public Sprite LoadingSprite => _levelData.LoadingSprite;
        public float LevelDuration => _levelData.LevelDuration;
        internal LevelNumber CurrentLevelNumber { get; set; }

#region Services

        private ILocalization _localization;
        private ILocalization Localization => _localization ??= Service.Instance.Get<ILocalization>();

#endregion

        public void SetActive(bool toActivate)
        {
            gameObject.SetActive(toActivate);
        }

        public string GetLevelDescription()
        {
            if (!string.IsNullOrEmpty(_levelDescription) && _currentLanguage == Localization.CurrentLanguage)
            {
                return _levelDescription;
            }

            string levelId = CurrentLevelNumber switch
            {
                LevelNumber.Last => LastChapterLocalizationId,
                LevelNumber.BeforeLast => BeforeLastChapterLocalizationId,
                _ => _levelData.LevelId
            };

            _levelDescription = Localization.GetTranslatedValue(Enums.LocalizationTable.Messages, levelId);
            _currentLanguage = Localization.CurrentLanguage;
            return _levelDescription;
        }
    }
}