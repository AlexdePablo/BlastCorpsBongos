using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaAnimator : MonoBehaviour
{
    private Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            m_Animator.SetBool("Running", true);
            m_Animator.SetBool("Idling", false);
            m_Animator.SetBool("Jumping", false);
            m_Animator.SetBool("Hitting", false);
            m_Animator.SetBool("Stomping", false);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            m_Animator.SetBool("Running", false);
            m_Animator.SetBool("Idling", true);
            m_Animator.SetBool("Jumping", false);
            m_Animator.SetBool("Hitting", false);
            m_Animator.SetBool("Stomping", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Animator.SetBool("Running", false);
            m_Animator.SetBool("Idling", false);
            m_Animator.SetBool("Jumping", true);
            m_Animator.SetBool("Hitting", false);
            m_Animator.SetBool("Stomping", false);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_Animator.SetBool("Running", false);
            m_Animator.SetBool("Idling", true);
            m_Animator.SetBool("Jumping", false);
            m_Animator.SetBool("Hitting", false);
            m_Animator.SetBool("Stomping", false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            m_Animator.SetBool("Running", false);
            m_Animator.SetBool("Idling", false);
            m_Animator.SetBool("Jumping", false);
            m_Animator.SetBool("Hitting", true);
            m_Animator.SetBool("Stomping", false);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            m_Animator.SetBool("Running", false);
            m_Animator.SetBool("Idling", true);
            m_Animator.SetBool("Jumping", false);
            m_Animator.SetBool("Hitting", false);
            m_Animator.SetBool("Stomping", false);
        }
    }
}
