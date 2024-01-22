using System.Collections;
using System.Collections.Generic;
using ClerkShadow.ServiceLocator;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace ClerkShadow.LocalizationSystem
{
    public class LocalizationController : MonoBehaviour
    {
        [SerializeField] private List<LocalizedStringTable> _localizationTables;

#region Services

        private ILocalization _localization;
        private ILocalization Localization => _localization ??= Service.Instance.Get<ILocalization>();

#endregion

        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            Localization.Init(_localizationTables);
        }
    }
}