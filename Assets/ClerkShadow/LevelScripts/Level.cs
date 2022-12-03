using UnityEngine;

namespace ClerkShadow.LevelScripts
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private float _levelDuration;
        [SerializeField] private Sprite _loadingSprite;
        [SerializeField] private LevelExit _exit;
        public float LevelDuration => _levelDuration;
        public LevelExit Exit => _exit;
        public Sprite LoadingSprite => _loadingSprite;

        public void SetActive(bool toActivate)
        {
            gameObject.SetActive(toActivate);
        }
    }
}
