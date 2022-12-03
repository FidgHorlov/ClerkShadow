using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private float _timeForLevel;
    [SerializeField] private LightsController _lightsController;
    [SerializeField] private AudioSource _globalAudioSource;
    [Space]
    [SerializeField] private Character _characterShadow;
    [SerializeField] private CharacterBase _characterBase;

    [SerializeField] private Vector3 _characterStartPosition;
    [SerializeField] private Vector3 _shadowStartPosition;

    [Space] 
    [SerializeField] private List<AudioClip> _ambientAudioClips;
    
    private void Awake()
    {
        DOTween.SetTweensCapacity(25000, 10); 
    }
    
    public void ResetGameState()
    {
        _characterBase.transform.position = _characterStartPosition;
//        _characterShadow.transform.position = _shadowStartPosition;
        _characterShadow.ResetPlayer();
        _lightsController.ResetLights();
        SceneManager.LoadScene(1);
    }

    public void SetTimeForLevel(float timeForLevel)
    {
        _timeForLevel = timeForLevel;
        _characterShadow.StartResize(_timeForLevel);
        _lightsController.StartLighting(_timeForLevel);
        PlayAmbientAudio();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        _characterBase.RunAnimation(_characterShadow.IsRunning);
        
        if (_characterShadow.IsLevelReset)
        {
            _characterShadow.IsLevelReset = true;
            ResetGameState();
        }
    }

    private void PlayAmbientAudio()
    {
        int clip = UnityEngine.Random.Range(0, _ambientAudioClips.Count-1);
        _globalAudioSource.clip = _ambientAudioClips[clip];
        _globalAudioSource.Play();
    }
}
