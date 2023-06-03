using System;
using System.Collections.Generic;
using System.Linq;

namespace ClerkShadow.LocalizationSystem
{
    public class Enums
    {
        public enum Language
        {
            Ukrainian,
            English
        }

        public enum LocalizationTable
        {
            Menu,
            Main,
            Messages
        }

        public static string GetLocalizationTable(LocalizationTable table) => LocalizationTableNames.First(tableName => table.Equals(tableName.Key)).Value;

        private static readonly Dictionary<LocalizationTable, string> LocalizationTableNames = new Dictionary<LocalizationTable, string>()
        {
            {LocalizationTable.Menu, "Menu"},
            {LocalizationTable.Main, "Main"},
            {LocalizationTable.Messages, "Messages"}
        };
    }
}