using System;
using System.Collections.Generic;
using ClerkShadow.LocalizationSystem;
using ClerkShadow.ServiceLocator;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ClerkShadow
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _languageSelector;
        [SerializeField] private Button _startGameHelperButton;

#region Services

        private ILocalization _localization;
        private ILocalization Localization => _localization ??= Service.Instance.Get<ILocalization>();

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
            SaveLanguage();
            SceneManager.LoadScene(Enums.GetSceneName(Enums.SceneName.Main));
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
            PlayerPrefs.SetString("Language", LocalizationSettings.SelectedLocale.Identifier.Code);
        }

        private string RestoreLanguage()
        {
            string localeId = PlayerPrefs.GetString("Language");
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