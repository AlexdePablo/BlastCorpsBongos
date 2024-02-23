using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SMBHitState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
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
        m_Animator.SetBool("Running", false);
        m_Animator.SetBool("Idling", false);
        m_Animator.SetBool("Jumping", false);
        m_Animator.SetBool("Hitting", true);
        m_Animator.SetBool("Stomping", false);
        m_Rigidbody.velocity = Vector3.zero;
        StartCoroutine(Return());

    }
    private void Update()
    {
        
    }
    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
    }
    IEnumerator Return()
    {
        yield return new WaitForSeconds(0.3f);
        m_GolpeObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        m_GolpeObj.SetActive(false);
        m_StateMachine.ChangeState<SMBIdleState>();
    }
}
