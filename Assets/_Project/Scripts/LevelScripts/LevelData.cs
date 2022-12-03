using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] private Level[] _levels;
    public Level[] Levels => _levels;
}
