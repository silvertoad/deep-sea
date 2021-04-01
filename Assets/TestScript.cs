using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private float _timeout = 1;

    private float _time;

    private void Update()
    {
        _time -= Time.deltaTime;
    }

    [ContextMenu("Action")]
    public void Action()
    {
        if (_time <= 0)
        {
            _time = _timeout;
            Debug.Log("Do action!");
        }
    }
}