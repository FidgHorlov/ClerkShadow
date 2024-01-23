using System;
using System.Collections;
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
            _buttonsCanvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            _languageSelector.onValueChanged.AddListener(LanguageChangedEventHandler);
            _startGameHelperButton.onClick.AddListener(StartGameClickedEventHandler);
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => Localization.WasInit);
            _buttonsCanvasGroup.DOFade(1f, 1f);
            InitDropdown();
            SetupStartLanguage();
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
            _buttonsCanvasGroup.DOFade(0f, TimeForHideButtons).OnComplete(() => StartCoroutine(nameof(LoadTargetScene)));
        }

        private IEnumerator LoadTargetScene()
        {
            yield return new WaitForSeconds(2f);
            SceneService.LoadScene(Enums.SceneName.Main);
        }

        private void SetupStartLanguage()
        {
            Enums.Language localeName = Enums.GetLocaleEnum(RestoreLanguage());
            int id = GetLanguageId(Enums.GetEnumDescription(localeName));
            if (id >= 0)
            {
                _languageSelector.SetValueWithoutNotify(id);
            }
            else
            {
                Debug.LogError("Can't parse language value");
            }

            Localization.ChangeLanguage(localeName);
        }

        private int GetLanguageId(string description) => _languageSelector.options.FindIndex(option => option.text == description);

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