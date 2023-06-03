using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ClerkShadow.LocalizationSystem
{
    public class LocalizationService : ILocalization
    {
        private List<LocalizedStringTable> _localizationTables;

        public Enums.Language CurrentLanguage { get; private set; }

        public void Init(List<LocalizedStringTable> localizationTables)
        {
            _localizationTables = localizationTables;
        }

        public void ChangeLanguage(Enums.Language language)
        {
            CurrentLanguage = language;
        }

        public string GetTranslatedValue(Enums.LocalizationTable tableName, string valueId)
        {
            StringTable resultTable = GetTable(tableName);
            return resultTable == null ? null : resultTable[valueId].GetLocalizedString(LocalizationSettings.SelectedLocale.Formatter);
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