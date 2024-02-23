using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CarController : MonoBehaviour
{
    [Header("Tipo de Tracción")]
    [SerializeField]
    private driveType drive;
    internal enum driveType
    {
        frontWheel,
        rearWheel,
        allWheel
    }

    [Header("Variables")]
    public float totalPower;
    public float KPH;
    public float wheelsRPM;
    public AnimationCurve enginePower;

    private InputManager IM;
    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject[] wheelMesh = new GameObject[4];
    public float[] gears;
    public int gearNum;
    public float engineRPM;
    public float smoothTime;
    public int motorTorque = 100;
    public int brakeTorque = 100;
    public float steeringMax = 25;
    private Rigidbody m_rigidBody;
    public GameObject centerOfMass;
    public float DownForceValue = 50;
    public float[] slip = new float[4];

    // Start is called before the first frame update
    void Start()
    {
       
        getObjects();
    }

    private void getObjects()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        IM = GetComponent<InputManager>();
        centerOfMass = transform.Find("centerOfMass").gameObject;
        m_rigidBody.centerOfMass = centerOfMass.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        AnimateWheels();
        MoveVehicle();
        SteerVehicle();
        AddDownForce();
        GetFriction();
        CalculateEnginePower();
        Shifter();
    }

    private void Shifter()
    {
        if (Input.GetKeyDown(KeyCode.E))
            gearNum++;
        if (Input.GetKeyDown(KeyCode.Q))
            gearNum--;
    }

    private void CalculateEnginePower()
    {
        WheelRPM();
        totalPower = enginePower.Evaluate(engineRPM) * (gears[gearNum]) * IM.vertical;
        float velocity = 0;
        engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (wheelsRPM * 3.6f * (gears[gearNum])), ref velocity, smoothTime);
    }

    private void WheelRPM()
    {
        float sum = 0;
        int R = 0;
        for(int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;
    }

    private void GetFriction()
    {
       for(int i =0; i < wheels.Length; i++)
        {
            WheelHit wheelHit;
            wheels[i].GetGroundHit(out wheelHit);

            slip[i] = wheelHit.sidewaysSlip;
        }
    }

    private void AddDownForce()
    {
        m_rigidBody.AddForce(transform.up * -1 * DownForceValue * m_rigidBody.velocity.magnitude);
    }

    private void MoveVehicle()
    {

        if(drive == driveType.allWheel)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = Mathf.Abs(IM.vertical) * (totalPower / 4);
            }
        }else if (drive == driveType.rearWheel)
        {
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = Mathf.Abs(IM.vertical) * (totalPower / 2);
            }
        }
        else
        {
            for (int i = 0; i < wheels.Length - 2; i++)
            {
                wheels[i].motorTorque = Mathf.Abs(IM.vertical) * (totalPower / 2);
            }
        }

        KPH = m_rigidBody.velocity.magnitude * 3.6f;


        if (IM.handBrake)
        {
            wheels[2].brakeTorque = wheels[3].brakeTorque = brakeTorque;
        }
        else
        {
            wheels[2].brakeTorque = wheels[3].brakeTorque = 0;
        }
    }

    private void SteerVehicle()
    {
        for (int i = 0; i < wheels.Length - 2; i++)
        {
            wheels[i].steerAngle = IM.horizontal * steeringMax;
        }
    }

    private void AnimateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;    
        }
    }
}
