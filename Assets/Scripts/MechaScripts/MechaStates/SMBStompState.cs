using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SMBStompState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private float m_Speed = 50;
    [SerializeField]
    private float m_Damage = 50000;
    private GameObject m_GolpeObj;
    private void Awake()
    {
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_GolpeObj = transform.GetChild(transform.childCount - 1).gameObject;
    }
    public override void Init()
    {
        base.Init();
        m_Rigidbody.velocity = new Vector3(0, -m_Speed, 0);
        m_Animator.SetBool("Running", false);
        m_Animator.SetBool("Idling", false);
        m_Animator.SetBool("Jumping", false);
        m_Animator.SetBool("Hitting", false);
        m_Animator.SetBool("Stomping", true);
        m_GolpeObj.SetActive(true);
    }
    public override void Exit()
    {
        base.Exit();
    }
    private void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!typeof(SMBStompState).IsInstanceOfType(m_StateMachine.CurrentState))
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            m_StateMachine.ChangeState<SMBIdleState>();
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("edifisi"))
        {
            collision.gameObject.GetComponent<ObstScriptPadre>().RestaVida(m_Damage);
            m_StateMachine.ChangeState<SMBIdleState>();
        }

        m_GolpeObj.SetActive(false);
    }
}
