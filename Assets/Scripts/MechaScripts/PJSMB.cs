using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBWalkState))]
[RequireComponent(typeof(FiniteStateMachine))]

public class PJSMB : MonoBehaviour
{
    private FiniteStateMachine m_StateMachine;
    public FiniteStateMachine StateMachine => m_StateMachine;
    [SerializeField]
    private InputActionAsset m_InputAsset;
    private InputActionAsset m_Input;
    public InputActionAsset Input => m_Input;
    private InputAction m_MovementAction;
    public InputAction MovementAction => m_MovementAction;
    private TextMeshProUGUI MarchaGUI;
    [SerializeField]
    private float m_RCDetection = 1.1f;
    public float FloorDetection => m_RCDetection;
    [SerializeField]
    private float m_JumpForce = 5;
    public float JumpForce => m_JumpForce;
    [SerializeField]
    private LayerMask m_LayerRayCastSalto;
    public LayerMask LayerRayCastSalto => m_LayerRayCastSalto;

    // Start is called before the first frame update


    private void Awake()
    {
        Assert.IsNotNull(m_InputAsset);
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Input = Instantiate(m_InputAsset);
        m_MovementAction = m_Input.FindActionMap("PersonajesMov").FindAction("Movimiento");
        m_Input.FindActionMap("PersonajesMov").Enable();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBlastCorps>().Init();
        MarchaGUI = GameObject.FindGameObjectWithTag("MarchasGui").GetComponent<TextMeshProUGUI>();
        MarchaGUI.text = "";
    }

    void Start()
    {
        m_StateMachine.ChangeState<SMBIdleState>();
    }
    private void OnDestroy()
    {
        m_Input.FindActionMap("PersonajesMov").Disable();
    }
}
