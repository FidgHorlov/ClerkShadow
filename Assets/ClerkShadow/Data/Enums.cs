using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ClerkShadow.Data
{
    public static class Enums
    {
        public enum Language
        {
            [Description("UA")] Ukrainian,
            [Description("ENG")] English
        }

        public enum LocalizationTable
        {
            Menu,
            Main,
            Messages
        }
        
        public enum SceneName
        {
            Menu,
            Main
        }
        
        private static readonly Dictionary<LocalizationTable, string> LocalizationTableNames = new Dictionary<LocalizationTable, string>()
        {
            {LocalizationTable.Menu, "Menu"},
            {LocalizationTable.Main, "Main"},
            {LocalizationTable.Messages, "Messages"}
        };

        private static readonly Dictionary<Language, string> LanguagesLocales = new Dictionary<Language, string>()
        {
            {Language.English, "en"},
            {Language.Ukrainian, "uk-UA"}
        };
        
        private static readonly Dictionary<SceneName, string> ScenesDictionary = new Dictionary<SceneName, string>()
        {
            {SceneName.Menu, "MenuScene"},
            {SceneName.Main, "MainScene"}
        };

        public static string GetLocalizationTable(LocalizationTable table) => LocalizationTableNames.First(tableName => table.Equals(tableName.Key)).Value;
        public static string GetLocaleId(Language language) => LanguagesLocales.First(languageName => language.Equals(languageName.Key)).Value;
        public static Language GetLocaleEnum(string localeId) => LanguagesLocales.First(languageName => localeId.Equals(languageName.Value)).Key;
        public static string GetSceneName(SceneName sceneName) => ScenesDictionary.First(scene => scene.Key.Equals(sceneName)).Value;
        public static SceneName GetSceneEnum(string sceneName) => ScenesDictionary.First(scene => scene.Value.Equals(sceneName)).Key;

        public static string GetEnumDescription(Enum value)
        {
            DescriptionAttribute[] da = (DescriptionAttribute[]) (value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }
}