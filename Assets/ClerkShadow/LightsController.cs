using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ClerkShadow
{
    public class LightsController : MonoBehaviour
    {
        private const float MaximumSize = 5f;
        
        [SerializeField] private Image _spotImage;
        [SerializeField] private Color _endColor; 
        [Space]
        [SerializeField] private Transform _followTarget;
        
        private Color _defaultColor;
        private Vector3 _defaultLightPos;
        private Transform _spotTransform;

        private void Start()
        {
            _defaultColor = _spotImage.color;
            _spotTransform = _spotImage.transform;
            _defaultLightPos = _spotTransform.localPosition;
        }

        private void Update()
        {
            _spotTransform.position = _followTarget.position;
        }

        public void StartLighting(float duration)
        {
            _spotImage.DOKill(_spotImage.sprite);
            _spotImage.DOColor(_endColor, duration).SetId(_spotImage.sprite);

            _spotTransform.DOKill(_spotTransform);
            _spotTransform.DOScale(MaximumSize, duration).SetId(_spotTransform);
        }

        public void ResetLights()
        {
            _spotImage.DOKill(_spotImage.sprite);
            _spotTransform.DOKill(_spotTransform);
            _spotTransform.DOKill(_spotTransform.transform);
            
            _spotImage.color = _defaultColor;
            _spotTransform.localPosition = _defaultLightPos;
        }
    }
}