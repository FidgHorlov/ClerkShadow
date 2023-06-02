using System;
using ClerkShadow.Data;
using DG.Tweening;
using UnityEngine;

namespace ClerkShadow
{
    public class Character : MonoBehaviour
    {
        private const float CheckPosition = 0.2f;
        private const string GroundLayerMaskName = "JumpArea";
        
        [SerializeField] private Transform _groundCheck;
        [Space] [SerializeField] private AnimationManager _animationManager;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        [Header("Resizing")] [SerializeField] private float _targetSize;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _longJumpForce;

        
#region Private fields

        private LayerMask _groundLayerMask;
        private Transform _currentTransform;
        private Vector3 _defaultFlippedSize;
        private Vector3 _targetSizeVector;
        private Vector3 _currentSize;
        private Vector3 _defaultSize;
        private Vector3 _defaultPosition;
        private TimeSpan _startResizing;

        private float _currentDuration; // keep the duration for resizing
        private float _resizeDurationLeft;
        private float _timeCounter;
        private bool _isLeftLooking;

        private bool _isScaling;

#endregion

        public bool IsRunning { get; private set; }
        public bool IsLevelReset { get; set; }
        public bool IsGameStarted { get; set; }

#region Monobehaviour

        protected void Awake()
        {
            _currentTransform = transform;
            _defaultPosition = _currentTransform.position;
            _currentSize = _currentTransform.localScale;
            _targetSizeVector = new Vector3(_targetSize, _targetSize, _targetSize);
            _defaultSize = _currentSize;
            _defaultFlippedSize = _defaultSize;
            _defaultFlippedSize.x *= -1f;
            _groundLayerMask = LayerMask.GetMask(GroundLayerMaskName);
        }

        private void FixedUpdate()
        {
            if (!IsGameStarted)
            {
                return;
            }

            Movement();
        }

        private void Update()
        {
            if (!IsGameStarted)
            {
                return;
            }

            Jump();
        }

#endregion

        public void ResetPlayer()
        {
            _currentTransform.DOKill();
            _isScaling = false;
            _currentSize = _isLeftLooking ? _defaultFlippedSize : _defaultSize;
            _currentTransform.localScale = _currentSize;
            _currentTransform.position = _defaultPosition;
        }

        public void StartResize(float duration = -1f)
        {
            if (duration > 0)
            {
                _currentDuration = duration;
                _resizeDurationLeft = _currentDuration;
                _startResizing = DateTime.Now.TimeOfDay;
            }
            else
            {
                _resizeDurationLeft = _currentDuration - (DateTime.Now.TimeOfDay - _startResizing).Seconds;
            }

            _isScaling = true;
            _currentTransform
                .DOScale(_targetSizeVector, _resizeDurationLeft)
                .SetEase(Constants.DoTweenDefaultEase)
                .OnUpdate(() => { _currentSize = _currentTransform.localScale; })
                .OnComplete(() =>
                {
                    HideShadow();
                    _isScaling = false;
                    _currentTransform.DOKill();
                });
        }

#region Pose changing

        private void Movement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            if (horizontal < 0f)
            {
                if (!_isLeftLooking)
                {
                    _isLeftLooking = true;
                    Flip();
                }
            }
            else if (horizontal > 0f)
            {
                if (_isLeftLooking)
                {
                    _isLeftLooking = false;
                    Flip();
                }
            }

            _rigidbody2D.velocity = new Vector2(horizontal * _speed, _rigidbody2D.velocity.y);
            IsRunning = horizontal != 0;
            _animationManager.SetBool(Constants.AnimationState.Run, IsRunning);
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !IsPlayerInAir())
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
                //_rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
                _animationManager.SetTrigger(Constants.AnimationState.Jump);
                return;
            }

            if (Input.GetKeyUp(KeyCode.Space) && IsPlayerInAir())
            {
                
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _longJumpForce);
                //_animationManager.SetTrigger(Constants.AnimationState.Jump);
                // _rigidbody2D.AddForce(new Vector2(0f, _longJumpForce));
            }
        }

        private bool IsPlayerInAir() => _rigidbody2D.velocity.y > 0;

        private void Flip()
        {
            _currentSize.x *= -1f;
            _currentTransform.localScale = _currentSize;
            _targetSizeVector.x *= -1f;
            if (!_isScaling) return;

            _currentTransform.DOKill();
            _isScaling = false;
            StartResize();
        }

#endregion

        private void HideShadow()
        {
            IsLevelReset = true;
        }

        private bool IsGrounded() => Physics2D.OverlapCircle(_groundCheck.position, CheckPosition, _groundLayerMask);
    }
}