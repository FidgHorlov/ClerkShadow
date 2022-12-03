using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private AnimationManager _animationManager;
    
    private Transform _currentTransform;
    private Vector3 _defaultScale;
    private Vector3 _flippedScale;
    private Vector3 _tempPosition;
    private float _defaultPositionY;
    
    public void RunAnimation(bool toStart)
    {
        _animationManager.SetBool(Constants.AnimationState.Run, toStart);
    }

    public void StopAnimation()
    {
        _animationManager.SetTrigger(Constants.AnimationState.Idle);
    }

    private void Awake()
    {
        _currentTransform = transform;
        _defaultScale = _currentTransform.localScale;
        _flippedScale = _defaultScale;
        _flippedScale.x *= -1f;
        _defaultPositionY = _currentTransform.position.y;
    }

    private void Update()
    {
        _tempPosition = _targetTransform.position + _offset;
        _tempPosition.y = _defaultPositionY;
        _currentTransform.position = _tempPosition;
        _currentTransform.localScale = (_targetTransform.localScale.x < 0) 
            ? _flippedScale 
            : _defaultScale;
    }
}
