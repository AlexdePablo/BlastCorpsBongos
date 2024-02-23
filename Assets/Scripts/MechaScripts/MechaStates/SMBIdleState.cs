
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SMBIdleState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private float m_JumpForce;
    private LayerMask layerRayCastSalto;
    private float m_RCDetection;
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
        m_PJ.Input.FindActionMap("PersonajesMov").FindAction("Accion").started += Jump;
        m_PJ.Input.FindActionMap("PersonajesMov").FindAction("Golpe").started += Golpe;
        m_Rigidbody.velocity = Vector2.zero;
        m_Animator.SetBool("Running", false);
        m_Animator.SetBool("Idling", true);
        m_Animator.SetBool("Jumping", false);
        m_Animator.SetBool("Hitting", false);
        m_Animator.SetBool("Stomping", false);
    }

    public override void Exit()
    {
        base.Exit();
        if (m_PJ.Input != null)
        {
            m_PJ.Input.FindActionMap("PersonajesMov").FindAction("Accion").started -= Jump;
            m_PJ.Input.FindActionMap("PersonajesMov").FindAction("Golpe").started -= Golpe;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (GetComponent<SMBJumpState>())
        {
            m_Rigidbody.AddForce(Vector2.up * m_JumpForce, ForceMode.Impulse);
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
        if (m_PJ.MovementAction.ReadValue<Vector2>() != Vector2.zero)
            m_StateMachine.ChangeState<SMBWalkState>();

        if (GetComponent<SMBJumpState>())
        {
            RaycastHit hit;

            if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), -transform.up, out hit, m_RCDetection, layerRayCastSalto))
            {
                m_StateMachine.ChangeState<SMBJumpState>();
            }
        }
    }


}

