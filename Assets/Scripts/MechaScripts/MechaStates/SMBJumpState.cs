using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SMBJumpState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private float m_Speed = 10;
    private Vector2 m_Movement;
    private LayerMask layerRayCastSalto;
    private float m_RCDetection;
    private void Awake()
    {
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        layerRayCastSalto = m_PJ.LayerRayCastSalto;
        m_RCDetection = m_PJ.FloorDetection;
    }
    public override void Init()
    {
        base.Init();
        m_PJ.Input.FindActionMap("PersonajesMov").FindAction("Stomp").started += Stomp;
        m_Animator.SetBool("Running", false);
        m_Animator.SetBool("Idling", false);
        m_Animator.SetBool("Jumping", true);
        m_Animator.SetBool("Hitting", false);
        m_Animator.SetBool("Stomping", false);
    }
    public override void Exit()
    {
        base.Exit();
        if (m_PJ != null)
        {
            m_PJ.Input.FindActionMap("PersonajesMov").FindAction("Accion").started -= Stomp;
        }
    }
    private void Stomp(InputAction.CallbackContext context)
    {
        m_StateMachine.ChangeState<SMBStompState>();
    }
    private void Update()
    {
        m_Movement = m_PJ.MovementAction.ReadValue<Vector2>();
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), -transform.up, out hit, m_RCDetection, layerRayCastSalto))
        {
            m_StateMachine.ChangeState<SMBIdleState>();
        }
    }
    private void FixedUpdate()
    {
        m_Rigidbody.velocity = new Vector3(m_Movement.x * m_Speed, m_Rigidbody.velocity.y, m_Movement.y * m_Speed);
        Vector3 movement = new Vector3(m_Movement.x, 0, m_Movement.y);
        if (movement.magnitude >= 0.1f)
            transform.rotation = Quaternion.LookRotation(movement);
    }
}
