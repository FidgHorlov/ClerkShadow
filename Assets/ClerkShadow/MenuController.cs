using System;
using System.Collections.Generic;
using ClerkShadow.Data;
using ClerkShadow.LocalizationSystem;
using ClerkShadow.SceneLoader;
using ClerkShadow.ServiceLocator;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace ClerkShadow
{
    public class MenuController : MonoBehaviour
    {
        private const float TimeForHideButtons = 1.5f;
        
        [SerializeField] private TMP_Dropdown _languageSelector;
        [SerializeField] private Button _startGameHelperButton;
        [SerializeField] private CanvasGroup _buttonsCanvasGroup;

#region Services

        private ILocalization _localization;
        private ILocalization Localization => _localization ??= Service.Instance.Get<ILocalization>();

        private ISceneService _sceneService;
        private ISceneService SceneService => _sceneService ??= Service.Instance.Get<ISceneService>();
        
#endregion

        private void Awake()
        {
            InitDropdown();
            SetupStartLanguage();
        }

        private void OnEnable()
        {
            _languageSelector.onValueChanged.AddListener(LanguageChangedEventHandler);
            _startGameHelperButton.onClick.AddListener(StartGameClickedEventHandler);
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Return))
            {
                return;
            }

            StartGame();
        }

        private void OnDisable()
        {
            _languageSelector.onValueChanged.RemoveListener(LanguageChangedEventHandler);
            _startGameHelperButton.onClick.RemoveListener(StartGameClickedEventHandler);
        }

        private void StartGameClickedEventHandler()
        {
            StartGame();
        }

        private void InitDropdown()
        {
            List<string> options = new List<string>();
            _languageSelector.ClearOptions();

            foreach (Enums.Language language in Enum.GetValues(typeof(Enums.Language)))
            {
                options.Add(Enums.GetEnumDescription(language));
            }

            _languageSelector.AddOptions(options);
        }
        
        private void StartGame()
        {
            SceneService.ShowLoader();
            _startGameHelperButton.interactable = false;
            _languageSelector.interactable = false;
            SaveLanguage();

            _buttonsCanvasGroup.DOFade(0f, TimeForHideButtons).OnComplete(() =>
            {
                SceneService.LoadScene(Enums.SceneName.Main);
            });
        }

        private void SetupStartLanguage()
        {
            Enums.Language localeName = Enums.GetLocaleEnum(RestoreLanguage());
            if (!Localization.CurrentLanguage.Equals(localeName))
            {
                _languageSelector.value = (int) localeName;
            }
        }
        
        private void SaveLanguage()
        {
            PlayerPrefs.SetString(Constants.PlayerPrefsName.Language, LocalizationSettings.SelectedLocale.Identifier.Code);
        }

        private string RestoreLanguage()
        {
            string localeId = PlayerPrefs.GetString(Constants.PlayerPrefsName.Language);
            if (string.IsNullOrEmpty(localeId))
            {
                localeId = Enums.GetLocaleId(Enums.Language.Ukrainian);
            }

            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeId);
            return localeId;
        }

        private void LanguageChangedEventHandler(int languageIndex)
        {
            Localization.ChangeLanguage((Enums.Language) languageIndex);
        }
    }
}