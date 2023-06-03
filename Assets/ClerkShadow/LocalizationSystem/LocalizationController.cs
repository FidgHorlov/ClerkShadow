using System.Collections.Generic;
using ClerkShadow.ServiceLocator;
using UnityEngine;
using UnityEngine.Localization;

namespace ClerkShadow.LocalizationSystem
{
    public class LocalizationController : MonoBehaviour
    {
        [SerializeField] private List<LocalizedStringTable> _localizationTables;

#region Services

        private ILocalization _localization;
        private ILocalization Localization => _localization ??= Service.Instance.Get<ILocalization>();

#endregion

        private void Start()
        {
            Localization.Init(_localizationTables);
        }
    }
}