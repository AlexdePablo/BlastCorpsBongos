using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CamionScript : MonoBehaviour
{
    private NavMeshAgent m_NavMeshAgent;
    private Transform posicionFinal;
    [SerializeField]
    private GameEventBool finDelJuego;

    // Start is called before the first frame update
    void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        posicionFinal = GameObject.FindGameObjectWithTag("Final").transform;
        m_NavMeshAgent.destination = posicionFinal.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Final"))
        {
            finDelJuego.Raise(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("edifisi"))
        {
            print("final");
            finDelJuego.Raise(false);
        }
    }
}
