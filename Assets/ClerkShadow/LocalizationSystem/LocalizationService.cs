using System;
using System.Collections.Generic;
using ClerkShadow.Data;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ClerkShadow.LocalizationSystem
{
    public class LocalizationService : ILocalization
    {
        public event Action<Enums.Language> LanguageChanged;
        private const string MissingTextValue = "missingTextValue";

        private List<LocalizedStringTable> _localizationTables;
        private bool _wasInited;
        public Enums.Language CurrentLanguage { private set; get; }

        public void Init(List<LocalizedStringTable> localizationTables)
        {
            _localizationTables = localizationTables;
            _wasInited = true;
        }

        public void ChangeLanguage(Enums.Language language)
        {
            CurrentLanguage = language;
            LanguageChanged?.Invoke(language);

            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                string localeId = Enums.GetLocaleId(language);
                if (!locale.Identifier.Code.Equals(localeId))
                {
                    continue;
                }

                LocalizationSettings.SelectedLocale = locale;
                return;
            }

            Debug.Log($"Language couldn't be changed. Current language -> {LocalizationSettings.SelectedLocale.LocaleName}");
        }

        public string GetTranslatedValue(Enums.LocalizationTable tableName, string valueId)
        {
            StringTable resultTable = GetTable(tableName);
            if (resultTable == null)
            {
                return null;
            }

            string translatedId = resultTable[valueId] == null ? MissingTextValue : valueId;
            return resultTable[translatedId].GetLocalizedString(LocalizationSettings.SelectedLocale.Formatter);
        }

        private StringTable GetTable(Enums.LocalizationTable localizationTable)
        { 
            string tableName = Enums.GetLocalizationTable(localizationTable);
            
            foreach (LocalizedStringTable table in _localizationTables)
            {
                if (!table.GetTable().TableCollectionName.Contains(tableName))
                {
                    continue;
                }

                StringTable tableAsync = GetTableAsync(table);
                return tableAsync;
            }

            return null;
        }

        private StringTable GetTableAsync(LocalizedStringTable table)
        {
            AsyncOperationHandle<StringTable> task = table.GetTableAsync();
            return task.IsDone ? task.Result : null;
        }
    }
}