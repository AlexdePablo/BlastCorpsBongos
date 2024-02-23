using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tractor : BlastController
{
    private Coroutine m_Derrape;
    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_RigidBody.centerOfMass = GameObject.FindGameObjectWithTag("CenterOfMass").transform.localPosition;
        Input = m_Input;
        Input.FindActionMap("PersonajesMov").FindAction("UpShift").started += UpShift;
        Input.FindActionMap("PersonajesMov").FindAction("DownShift").started += DownShift;
        Input.FindActionMap("PersonajesMov").FindAction("Accion").started += Derrape;
        Input.FindActionMap("PersonajesMov").FindAction("Accion").canceled += NoDerrape;
        Input.Enable();
        gearNum = 1;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBlastCorps>().Init();
        MarchaGUI = GameObject.FindGameObjectWithTag("MarchasGui").GetComponent<TextMeshProUGUI>();
        MarchaGUI.text = gearNum.ToString();
    }

    private void NoDerrape(InputAction.CallbackContext obj)
    {
        StartCoroutine(endDerrape());

    }

    private void Derrape(InputAction.CallbackContext obj)
    {
        WheelFrictionCurve wheelCollider = rueditas[2].colliderRueda.sidewaysFriction;
        wheelCollider.extremumValue = 1f;
        rueditas[2].colliderRueda.sidewaysFriction = wheelCollider;
        rueditas[3].colliderRueda.sidewaysFriction = wheelCollider;
    }

    private IEnumerator endDerrape()
    {
        WheelFrictionCurve wheelCollider = rueditas[2].colliderRueda.sidewaysFriction;
        wheelCollider.extremumValue = 8;
        rueditas[0].colliderRueda.sidewaysFriction = wheelCollider;
        rueditas[1].colliderRueda.sidewaysFriction = wheelCollider;
        rueditas[2].colliderRueda.sidewaysFriction = wheelCollider;
        rueditas[3].colliderRueda.sidewaysFriction = wheelCollider;
        yield return new WaitForSeconds(0.5f);
        wheelCollider = rueditas[2].colliderRueda.sidewaysFriction;
        wheelCollider.extremumValue = 2;
        rueditas[0].colliderRueda.sidewaysFriction = wheelCollider;
        rueditas[1].colliderRueda.sidewaysFriction = wheelCollider;
        wheelCollider.extremumValue = 3;
        rueditas[2].colliderRueda.sidewaysFriction = wheelCollider;
        rueditas[3].colliderRueda.sidewaysFriction = wheelCollider;
    }
    private void OnDestroy()
    {
        Input.FindActionMap("PersonajesMov").FindAction("UpShift").started -= UpShift;
        Input.FindActionMap("PersonajesMov").FindAction("DownShift").started -= DownShift;
        Input.FindActionMap("PersonajesMov").FindAction("Accion").started -= Derrape;
        Input.FindActionMap("PersonajesMov").FindAction("Accion").canceled -= NoDerrape;
        Input.Disable();
    }
}
