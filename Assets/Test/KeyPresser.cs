using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPresser : MonoBehaviour
{
    [SerializeField] private KeyCode _keyToPress;
    private MeshRenderer _mr;

    private void Start()
    {
        _mr = GetComponent<MeshRenderer>();
        _mr.material.color = Color.blue;
    }
    private void Update()
    {
        if (Input.GetKey(_keyToPress))
        {
            _mr.material.color = Color.red;
        }
        if (Input.GetKeyUp(_keyToPress))
        {
            _mr.material.color = Color.blue;
        }

    }
}
