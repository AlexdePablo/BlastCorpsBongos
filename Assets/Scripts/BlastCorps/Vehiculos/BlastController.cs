using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using static BlastController;

[RequireComponent(typeof(Rigidbody))]

public abstract class BlastController : MonoBehaviour
{

    [SerializeField]
    protected Ruedas[] rueditas = new Ruedas[4];
    [SerializeField]
    protected InputActionAsset m_Input;
    protected InputActionAsset Input;
    [SerializeField]
    protected float torque;
    [SerializeField]
    protected float angleSteer;
    protected Rigidbody m_RigidBody;
    [SerializeField]
    protected int ForceBuff;
    [SerializeField]
    protected int gearNum;
    protected TextMeshProUGUI MarchaGUI;
    private bool speedBuffCD = false;
    [Serializable]
    public struct Ruedas
    {
        public WheelCollider colliderRueda;
        public Transform transformRueda;
    }

    // Update is called once per frame
    protected void Update()
    {
        Vector2 movimiento = Input.FindActionMap("PersonajesMov").FindAction("Movimiento").ReadValue<Vector2>();

        AnimateWheels();

        Movement(movimiento.y);

        rueditas[0].colliderRueda.steerAngle = rueditas[1].colliderRueda.steerAngle = angleSteer * movimiento.x;
    }
    protected void UpShift(InputAction.CallbackContext obj)
    {
        gearNum = 1;
        MarchaGUI.text = gearNum.ToString();
    }
    protected void DownShift(InputAction.CallbackContext obj)
    {
        gearNum = -1;
        MarchaGUI.text = gearNum.ToString();
    }
    protected void Movement(float y)
    {
        if (y > 0)
        {
            foreach (Ruedas rueda in rueditas)
            {
                rueda.colliderRueda.motorTorque = torque * gearNum;
                rueda.colliderRueda.brakeTorque = 0;
            }
        }
        else if(y < 0)
        {
            foreach (Ruedas rueda in rueditas)
            {
                rueda.colliderRueda.brakeTorque = torque * 3;
                rueda.colliderRueda.motorTorque = 0;
            }
        }
        else if(y == 0)
        {
            foreach (Ruedas rueda in rueditas)
            {
                rueda.colliderRueda.brakeTorque = torque * 0.5f;
                rueda.colliderRueda.motorTorque = 0;
            }
        }
    }
    public void SpeedBuff()
    {
        if (!speedBuffCD)
        {
            m_RigidBody.AddForce(transform.forward * ForceBuff, ForceMode.Impulse);
            StartCoroutine(SpeedBuffCoolDown());
        }

    }
    private IEnumerator SpeedBuffCoolDown()
    {
        speedBuffCD = true;
        yield return new WaitForSeconds(2);
        speedBuffCD = false;
    }
    protected void AnimateWheels()
    {
        Vector3 wheelPosition;
        Quaternion wheelRotation;

        for (int i = 0; i < 2; i++)
        {
            rueditas[i].colliderRueda.GetWorldPose(out wheelPosition, out wheelRotation);
            rueditas[i].transformRueda.transform.position = wheelPosition;
            rueditas[i].transformRueda.transform.rotation = wheelRotation;
        }
    }
}
