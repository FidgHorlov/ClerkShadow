using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private float _levelDuration;
    public float LevelDuration => _levelDuration;

    [SerializeField] private LevelExit _exit;
    public LevelExit Exit => _exit;
}
