using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelExit : MonoBehaviour
{   
    public Action<Collider2D> TriggerEntered;
    public Action<Collider2D> TriggerExit;

    private const int PlayerLayer = 0;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("AAAAAAAAAA");
            TriggerEntered?.Invoke(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        TriggerExit?.Invoke(collider);
    }
}
