using DG.Tweening;
using UnityEngine;

namespace ClerkShadow
{
    public class LightsController : MonoBehaviour
    {
        [SerializeField] private Light _light;
        [SerializeField] private float _targetLightPosY;
        [SerializeField] private float _maxLightIntensity;
        [SerializeField] private Color _endColor; 
        [Space]
        [SerializeField] private Transform _followTarget;
        private Color _defaultColor;
        private float _minimalLightIntensity;
        private Vector3 _defaultLightPos;
        private Transform _currentTransform;

        private void Start()
        {
            _minimalLightIntensity = _light.intensity;
            _defaultColor = _light.color;
            _currentTransform = transform;
            _defaultLightPos = _currentTransform.localPosition;
        }

        private void Update()
        {
            _currentTransform.position = _followTarget.position;
        }

        public void StartLighting(float duration)
        {
            _light.DOColor(_endColor, duration);
            _light.DOIntensity(_maxLightIntensity, duration).OnComplete(() => _light.DOKill());
            _currentTransform.DOMoveY(_targetLightPosY, duration).OnComplete(() => _currentTransform.DOKill());
        }

        public void ResetLights()
        {
            _light.DOKill();
            _currentTransform.DOKill();
            _light.intensity = _minimalLightIntensity;
            _light.color = _defaultColor;
            _currentTransform.localPosition = _defaultLightPos;
        }
    
#region  Obsolete
//    [SerializeField] private float _intensityGrowthSpeed = 5f;
//    [SerializeField] private Color _startColor;
//    private float _currentLightIntensity;
//    private void Update()
//    {
//        if (_currentLightIntensity >= _maxLightIntensity)
//        {
//            return;
//        }
//
//        UpdateLightIntensity();
//        UpdateLightColor();
//    }
//
//    private void UpdateLightIntensity()
//    {
//        _currentLightIntensity += Time.deltaTime * _intensityGrowthSpeed * 0.1f;
//        _light.intensity = _currentLightIntensity;
//    }
//
//    private void UpdateLightColor()
//    {
//        _light.color = Color.Lerp(_startColor, _endColor,
//            Mathf.InverseLerp(_minimalLightIntensity, _maxLightIntensity, _currentLightIntensity));
//    }
#endregion
    }
}