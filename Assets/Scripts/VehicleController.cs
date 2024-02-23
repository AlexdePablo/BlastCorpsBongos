using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using static VehicleController;

public class VehicleController : MonoBehaviour
{
    [SerializeField]
    private float acceleration = 0.1f;
    [SerializeField]
    private float breakingForce = 300f;
    [SerializeField]
    private float maxTurnAngle = 15;

    [SerializeField]
    private AnimationCurve torqueCurve;
    [SerializeField]
    private AnimationCurve HPCurve;
    [SerializeField]
    private float maxTorque = 500.0f; // Torque máximo del motor
    [SerializeField]
    private float maxRPM = 7000.0f;   // RPM máximas del motor

    private float currentRPM;
    private int currentGear;

    [SerializeField]
    private TextMeshProUGUI GearText;
    [SerializeField]
    private TextMeshProUGUI RevolutionsText;


    private float currentTurnAngle = 0f;
    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;

    [SerializeField]
    private List<Wheel> wheels;
    [SerializeField]
    private Traction m_Traction;
    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }
    [SerializeField]
    private List<Gear> m_Gears;

    private Gear m_ActualGear;

    [Serializable]
    public struct Gear
    {
        public float radioBrazoDePalanca;
        public float maxSpeedGear;
        public float gearRPMMultiplier;
    }
    public enum Axel
    {
        FRONT, REAR
    }
    public enum Traction
    {
        FRONT_WHEEL, REAR_WHEEL, TOTAL
    }
    private Rigidbody m_RigidBody;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        currentGear = 1;
        m_ActualGear = m_Gears[currentGear];
        updateGearGui();

    }

    private void updateGearGui()
    {
        switch (currentGear)
        {
            case 0:
                GearText.text = "R";
                break;
            case 1:
                GearText.text = "N";
                break;
            case 2:
                GearText.text = "1";
                break;
            case 3:
                GearText.text = "2";
                break;
            case 4:
                GearText.text = "3";
                break;
            case 5:
                GearText.text = "4";
                break;
        }
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentAcceleration = acceleration * Input.GetAxis("Vertical");
        if (currentAcceleration > 0)
            Accelerate();
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space))
            currentBrakeForce = breakingForce;
        else
            currentBrakeForce = 0f;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            UpShift();
        if (Input.GetKeyDown(KeyCode.LeftControl))
            DownShift();
        AplicarDrag();
        Move();
        AnimationWheels();
        Steer();
        Brake();

        FuerzaMotor();

        UpdateRPM();
    }

    private void LateUpdate()
    {
        RevolutionsText.text = currentRPM.ToString();
    }

    private void DownShift()
    {
        currentGear--;
        if (currentGear < 0)
            currentGear = 0;
        m_ActualGear = m_Gears[currentGear];
        updateGearGui();
    }

    private void UpShift()
    {
        currentGear++;
        if (currentGear > m_Gears.Count -1)
            currentGear = m_Gears.Count -1;
        m_ActualGear = m_Gears[currentGear];
        currentRPM -= 2000;
        updateGearGui();
    }

    private void FuerzaMotor()
    {
        float torque = torqueCurve.Evaluate(currentRPM);
        print((torque / m_ActualGear.radioBrazoDePalanca) / 900);
    }

    void Accelerate()
    {
        float torque = torqueCurve.Evaluate(currentRPM);
        float aaaac = torque / m_ActualGear.radioBrazoDePalanca / 900;
        currentRPM += aaaac * Time.deltaTime * 1000; // Aumenta las RPM con el tiempo

    }
    void UpdateRPM()
    {
        currentRPM = Mathf.Clamp(currentRPM, 0, maxRPM); // Limita las RPM al rango máximo
        if (currentRPM < 1000)
            currentRPM = 1000;
        //print(currentRPM + " rpm");

        // Evalúa la curva de torque y RPM según las RPM actuales
        float currentTorque = torqueCurve.Evaluate(currentRPM / maxRPM) * maxTorque;

        float torque = torqueCurve.Evaluate(currentRPM);
        float HorsePower = HPCurve.Evaluate(currentRPM);

        //print(HorsePower / torque);

        //print(torqueCurve.Evaluate(currentRPM) + " torque");

        //print(currentTorque + "torque");

        // Lógica para simular la pérdida de RPM cuando no se acelera
        if (!Input.GetKey(KeyCode.W) && currentRPM > 0)
        {
            currentRPM -= Time.deltaTime * 2000; // Simula la disminución de RPM sin acelerar
        }
    }

    private void AplicarDrag()
    {
        //m_RigidBody.drag = C * m_RigidBody.velocity.magnitude * m_RigidBody.velocity.magnitude;
    }

    private void Brake()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.axel == Axel.REAR)
            {
                wheel.wheelCollider.brakeTorque = 600 * currentBrakeForce * Time.deltaTime;
            }

        }
    }

    private void Steer()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.axel == Axel.FRONT)
            {
                wheel.wheelCollider.steerAngle = currentTurnAngle;
            }
        }
    }

    private void AnimationWheels()
    {
        foreach (Wheel wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    private void Move()
    {
        if (m_Traction == Traction.TOTAL)
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = 600 * currentAcceleration * Time.deltaTime;
            }
        }
        if (m_Traction == Traction.FRONT_WHEEL)
        {
            foreach (Wheel wheel in wheels)
            {
                if (wheel.axel == Axel.FRONT)
                    wheel.wheelCollider.motorTorque = 600 * currentAcceleration * Time.deltaTime;
            }
        }
        if (m_Traction == Traction.REAR_WHEEL)
        {
            foreach (Wheel wheel in wheels)
            {
                //print(wheel.wheelCollider.rpm + "rpm");
                if (wheel.axel == Axel.REAR)
                    wheel.wheelCollider.motorTorque = 600 * currentAcceleration * Time.deltaTime;
            }
        }
    }
}
