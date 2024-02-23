using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TractorPutiaso : BlastController
{
    private Animator m_Animator;
    private bool golpeando;
    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_RigidBody.centerOfMass = GameObject.FindGameObjectWithTag("CenterOfMass").transform.localPosition;
        Input = m_Input;
        golpeando = false;
        m_Animator = GetComponent<Animator>();  
        Input.FindActionMap("PersonajesMov").FindAction("UpShift").started += UpShift;
        Input.FindActionMap("PersonajesMov").FindAction("DownShift").started += DownShift;
        Input.FindActionMap("PersonajesMov").FindAction("Accion").started += Putiaso;
        Input.Enable();
        gearNum = 1;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBlastCorps>().Init();
        MarchaGUI = GameObject.FindGameObjectWithTag("MarchasGui").GetComponent<TextMeshProUGUI>();
        MarchaGUI.text = gearNum.ToString();
    }

    private void Putiaso(InputAction.CallbackContext obj)
    {
        if (golpeando)
            return;
        else
            StartCoroutine(Golpe());
    }

    private IEnumerator Golpe()
    {
        golpeando = true;
        m_Animator.Play("Putiaso");
        yield return new WaitForSeconds(0.8f);
        golpeando= false;
    }
    private void OnDestroy()
    {
        Input.FindActionMap("PersonajesMov").FindAction("UpShift").started -= UpShift;
        Input.FindActionMap("PersonajesMov").FindAction("DownShift").started -= DownShift;
        Input.FindActionMap("PersonajesMov").FindAction("Accion").started -= Putiaso;
        Input.Disable();
    }
}
