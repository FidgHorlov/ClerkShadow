using UnityEngine;

namespace ClerkShadow
{
    [RequireComponent(typeof(Animator))]
    public class AnimationManager : MonoBehaviour
    {
        private Animator _animator;
        private bool _isRunning;
    
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetTrigger(string nameOfState)
        {
            _animator.SetTrigger(nameOfState);
        }
    
        public void SetBool(string nameOfState, bool toActivate)
        {
            if (_isRunning.Equals(toActivate))
            {
                return;
            }
        
            _isRunning = toActivate;
            _animator.SetBool(nameOfState, toActivate);
        }
    }
}
