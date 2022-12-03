using UnityEngine;

namespace ClerkShadow.LevelScripts
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private float _levelDuration;
        public float LevelDuration => _levelDuration;

        [SerializeField] private LevelExit _exit;
        public LevelExit Exit => _exit;
    }
}
