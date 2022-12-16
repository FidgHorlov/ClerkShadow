using UnityEngine;

namespace ClerkShadow.LevelScripts
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private LevelExit _exit;
        
        public LevelExit Exit => _exit;
        public float LevelDuration => _levelData.LevelDuration;
        public Sprite LoadingSprite => _levelData.LoadingSprite;
        public string LevelDescription => _levelData.LevelDescription;

        public void SetActive(bool toActivate)
        {
            gameObject.SetActive(toActivate);
        }
    }
}
