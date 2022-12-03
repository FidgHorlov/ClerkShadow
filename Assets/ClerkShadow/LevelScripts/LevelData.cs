using UnityEngine;

namespace ClerkShadow.LevelScripts
{
    public class LevelData : MonoBehaviour
    {
        [SerializeField] private Level[] _levels;
        public Level[] Levels => _levels;
    }
}
