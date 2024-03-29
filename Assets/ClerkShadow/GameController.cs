﻿using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClerkShadow
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LightsController _lightsController;
        [SerializeField] private AudioSource _globalAudioSource;
        [Space]
        [SerializeField] private Character _characterShadow;
        [SerializeField] private CharacterBase _characterBase;

        [SerializeField] private Vector3 _characterStartPosition;
        [SerializeField] private Vector3 _shadowStartPosition;

        [Space] 
        [SerializeField] private List<AudioClip> _ambientAudioClips;
        
        private float _timeForLevel;
    
        private void Awake()
        {
            DOTween.SetTweensCapacity(25000, 10); 
        }
    
        public void ResetGameState()
        {
            _characterBase.transform.position = _characterStartPosition; 
            _characterShadow.transform.position = _shadowStartPosition;
            _characterShadow.ResetPlayer();
            _lightsController.ResetLights();
            SceneManager.LoadScene(1);
        }

        public void SetTimeForLevel(float timeForLevel)
        {
            _timeForLevel = timeForLevel;
            _characterShadow.StartResize(_timeForLevel);
            _lightsController.StartLighting(_timeForLevel);
            _characterShadow.IsGameStarted = true;
            PlayAmbientAudio();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            _characterBase.RunAnimation(_characterShadow.IsRunning);

            if (!_characterShadow.IsLevelReset)
            {
                return;
            }
            
            _characterShadow.IsLevelReset = true;
            ResetGameState();
        }

        private void PlayAmbientAudio()
        {
            int clip = Random.Range(0, _ambientAudioClips.Count-1);
            _globalAudioSource.clip = _ambientAudioClips[clip];
            _globalAudioSource.Play();
        }
    }
}
