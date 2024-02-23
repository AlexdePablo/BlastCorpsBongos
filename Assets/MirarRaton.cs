using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class MirarRaton : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset m_InputAsset;
    private InputAction m_MouseAction;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(-Vector3.up * 50f * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up * 50f * Time.deltaTime);

    }
}
