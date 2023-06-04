using System;
using System.Collections.Generic;
using ClerkShadow.ServiceLocator;
using UnityEngine.Localization;

namespace ClerkShadow.LocalizationSystem
{
    public interface ILocalization : IService
    {
        event Action<Enums.Language> LanguageChanged;
        Enums.Language CurrentLanguage { get; }
        void Init(List<LocalizedStringTable> localizationTables);
        void ChangeLanguage(Enums.Language language);
        string GetTranslatedValue(Enums.LocalizationTable tableName, string valueId);
    }
}
