using System;
using UnityEngine;

namespace ClerkShadow.Levels
{
    [Serializable]
    [CreateAssetMenu(menuName ="FidgetLand/Create Level Data Asset", fileName = "Level")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private float _levelDuration;
        [SerializeField] private Sprite _loadingSprite;
        [Tooltip("The Level Id should be same as in the Localization Table")]
        [SerializeField] private string _levelId;

        public float LevelDuration => _levelDuration;
        public Sprite LoadingSprite => _loadingSprite;
        public string LevelId => _levelId;
    }
}