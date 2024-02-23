using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class SMBWalkState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private float m_JumpForce;
    private LayerMask layerRayCastSalto;
    private float m_RCDetection;

    private Vector2 m_Movement;

    [SerializeField]
    private float m_Speed = 3;

    private void Awake()
    {
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_JumpForce = m_PJ.JumpForce;
        layerRayCastSalto = m_PJ.LayerRayCastSalto;
        m_RCDetection = m_PJ.FloorDetection;
    }

    public override void Init()
    {
        base.Init();
        m_PJ.Input.FindActionMap("PersonajesMov").FindAction("Golpe").started += Golpe;
        m_Animator.SetBool("Running", true);
        m_Animator.SetBool("Idling", false);
        m_Animator.SetBool("Jumping", false);
        m_Animator.SetBool("Hitting", false);
        m_Animator.SetBool("Stomping", false);

    }

    public override void Exit()
    {
        base.Exit();
        if (m_PJ.Input != null)
        {
            m_PJ.Input.FindActionMap("PersonajesMov").FindAction("Golpe").started -= Golpe;
        }
    }

    private void Golpe(InputAction.CallbackContext context)
    {
        if (GetComponent<SMBHitState>())
        {
            m_StateMachine.ChangeState<SMBHitState>();
        }
    }

    private void Update()
    {
        m_Movement = m_PJ.MovementAction.ReadValue<Vector2>();

        if (m_Movement == Vector2.zero)
            m_StateMachine.ChangeState<SMBIdleState>();

        
    }

    private void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;

        if (m_Movement.y > 0)
            movement += Vector3.forward;

        else if (m_Movement.y < 0)
            movement += Vector3.back;

        if (m_Movement.x > 0)
            movement += Vector3.right;

        else if (m_Movement.x < 0)
            movement += Vector3.left;

        if (movement.magnitude >= 0.1f)
            transform.rotation = Quaternion.LookRotation(movement);

        m_Rigidbody.velocity = movement.normalized * m_Speed;
    }
}

