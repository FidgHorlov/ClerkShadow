using System;
using UnityEngine;

namespace ClerkShadow.LevelScripts
{
    [Serializable]
    [CreateAssetMenu(menuName ="FidgetLand/Create Level Data Asset", fileName = "Level")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private float _levelDuration;
        [SerializeField] private Sprite _loadingSprite;
        [SerializeField] [Multiline] private string _levelDescription;

        public float LevelDuration => _levelDuration;
        public Sprite LoadingSprite => _loadingSprite;
        public string LevelDescription => _levelDescription;
    }
}