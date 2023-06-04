using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ClerkShadow.LocalizationSystem
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
        
        public static string GetEnumDescription(Enum value)
        {
            DescriptionAttribute[] da = (DescriptionAttribute[]) (value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }
}