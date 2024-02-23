using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoScript : ObstScriptPadre
{
    private Rigidbody m_RigidBody;
    [SerializeField]
    private int m_Force;
    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody= GetComponent<Rigidbody>();
        m_RigidBody.AddForceAtPosition(new Vector3(getNum(), getNum(), getNum()).normalized * m_Force, new Vector3(getNum(), transform.position.y - transform.lossyScale.y / 2, getNum()), ForceMode.Force);
    }
    private float getNum()
    {
        return Random.Range(-transform.lossyScale.x, transform.lossyScale.x);
    }
    private void OnDestroy()
    {
        if (muerto)
        {
            onEdificioDestruidoInt.Raise(puntosPorEdificio);
            GameObject exp = Instantiate(explosion);
            exp.transform.position = transform.position;
        }
    }
}
