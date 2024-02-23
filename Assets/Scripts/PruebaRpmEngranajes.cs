using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PruebaRpmEngranajes : MonoBehaviour
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

    private float MaxRPM = 9000;

    private int dientesMotor = 10;

    private float ruedasTorque;

    [SerializeField]
    private AnimationCurve curvaDeTorque;


    [Serializable]
    public struct TransmisionMarcha
    {
        public float Multiplicador;
        public int piñones;
    }

    public TransmisionMarcha[] marchitasDeVerdad = new TransmisionMarcha[5];



    [SerializeField]
    private TextMeshProUGUI textoRPM;
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
        textoRPM.text = engineRPM.ToString();
        AnimateWheels();
        MoveVehicle();
        SteerVehicle();
        AddDownForce();
        GetFriction();
        rpmRuedas();
        CalculateEnginePower();
        Shifter();


        /*if (Input.GetKey(KeyCode.W))
        {
            // Aumentar las RPM cuando se presiona la tecla W
            engineRPM += Time.deltaTime * 2000f * marchitasDeVerdad[gearNum].Multiplicador; // Puedes ajustar este valor según tu necesidad
            engineRPM = Mathf.Clamp(engineRPM, 0f, MaxRPM); // Asegúrate de que no exceda el máximo
        }
        else
        {
            // Disminuir las RPM cuando no se presiona la tecla W
            engineRPM -= Time.deltaTime * 3000f; // Simulando una disminución constante
            engineRPM = Mathf.Max(1000f, engineRPM); // Asegúrate de que no sea negativo
        }*/
    }

    private void rpmRuedas()
    {
        if (engineRPM > 0)
        {
            var relacion = marchitasDeVerdad[gearNum].piñones / dientesMotor;
            ruedasTorque = engineRPM / relacion;
            
        }
        if(engineRPM == MaxRPM)
        {
            ruedasTorque = 0;
        }
        /*
        var relacion = marchitasDeVerdad[gearNum].piñones / dientesMotor;
        var torqueEntrada = curvaDeTorque.Evaluate(engineRPM);

        ruedasTorque = relacion * torqueEntrada;
        totalPower = ruedasTorque;
        */
    }

    private void FixedUpdate()
    {
        
       
    }

    private void Shifter()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            gearNum++;
            if (gearNum > marchitasDeVerdad.Length - 1)
                gearNum = marchitasDeVerdad.Length - 1;
            else
            {
                engineRPM -= 3000;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gearNum--;
            if (gearNum < 0)
                gearNum = 0;
        }
    }
    private void CalculateEnginePower()
    {
        WheelRPM();
        totalPower = enginePower.Evaluate(engineRPM) * (marchitasDeVerdad[gearNum].Multiplicador) * IM.vertical;
        float velocity = 0;
        float targetRPM = 1000 + (wheelsRPM * 3.6f * marchitasDeVerdad[gearNum].Multiplicador);
        targetRPM = Mathf.Clamp(targetRPM, 0f, MaxRPM); // Limitar el valor a un máximo permitido

        engineRPM = Mathf.SmoothDamp(engineRPM, targetRPM, ref velocity, smoothTime);


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
        wheelsRPM = (R != 0) ? sum / 2 : 0;
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
                wheels[i].motorTorque = Mathf.Abs(IM.vertical) * (ruedasTorque / 2);
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
