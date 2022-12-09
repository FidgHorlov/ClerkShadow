using System;
using ClerkShadow.Data;
using DG.Tweening;
using UnityEngine;

namespace ClerkShadow
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private AnimationManager _animationManager;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        [Header("Resizing")] [SerializeField] private float _targetSize;
        [SerializeField] private float _jumpForce;

#region Private fields

        private Transform _currentTransform;
        private Vector3 _defaultFlippedSize;
        private Vector3 _targetSizeVector;
        private Vector3 _currentSize;
        private Vector3 _defaultSize;
        private Vector3 _defaultPosition;

        private float _currentDuration; // keep the duration for resizing
        private float _resizeDurationLeft;
        private float _timeCounter;
        private bool _isLeftLooking;

        private bool _isJumpAllowed;
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
            _isJumpAllowed = true;
            _targetSizeVector = new Vector3(_targetSize, _targetSize, _targetSize);
            _defaultSize = _currentSize;
            _defaultFlippedSize = _defaultSize;
            _defaultFlippedSize.x *= -1f;
        }

        private void FixedUpdate()
        {
            if (!IsGameStarted)
            {
                return;
            }

            Movement();
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (CheckTheJumpPossibilities(other))
            {
                _isJumpAllowed = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (CheckTheJumpPossibilities(other))
            {
                _isJumpAllowed = false;
            }
        }

#endregion

        private bool CheckTheJumpPossibilities(Collision2D other)
        {
            return CheckTheTag(other, Constants.Tags.Ground) || CheckTheTag(other, Constants.Tags.Obstacle);
        }

        private bool CheckTheTag(Collision2D targetCollision, string targetTag)
        {
            return targetCollision.gameObject.CompareTag(targetTag);
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

            if (!Input.GetKeyDown(KeyCode.Space) || !_isJumpAllowed)
            {
                return;
            }

            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
            _animationManager.SetTrigger(Constants.AnimationState.Jump);
        }

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

        public void ResetPlayer()
        {
            _currentTransform.DOKill();
            _isScaling = false;
            _currentSize = _isLeftLooking ? _defaultFlippedSize : _defaultSize;
            _currentTransform.localScale = _currentSize;
            _currentTransform.position = _defaultPosition;
        }

        private TimeSpan _startResizing;

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
                .OnUpdate(() =>
                {
                    _currentSize = _currentTransform.localScale;
                })
                .OnComplete(() =>
                {
                    HideShadow();
                    _isScaling = false;
                    _currentTransform.DOKill();
                });
        }

        private void HideShadow()
        {
            IsLevelReset = true;
        }

#endregion
    }
}