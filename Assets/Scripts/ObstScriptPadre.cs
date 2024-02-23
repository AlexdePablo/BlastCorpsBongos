using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstScriptPadre : MonoBehaviour
{
    [SerializeField]
    protected float NewtonsNecesariosParaDestuirse;
    protected bool muerto = false;
    [SerializeField]
    protected int puntosPorEdificio;
    [SerializeField]
    protected GameEventInt onEdificioDestruidoInt;
    [SerializeField]
    protected GameObject explosion;
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PJSMB>())
                return;
            float v = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
            float x = collision.gameObject.GetComponent<Rigidbody>().mass;
            RestarVida(v * x);
        }
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            int num = other.GetComponent<Putiaso>().Damage;

            RestarVida(num);
        }
        if (other.CompareTag("Golpe"))
        {
            RestarVida(50000);
        }
    }

    public void RestaVida(float vida)
    {
        RestarVida(vida);
    }

    protected void RestarVida(float vida)
    {
        NewtonsNecesariosParaDestuirse -= vida;
        if (NewtonsNecesariosParaDestuirse < 0)
        {
            muerto = true;
            Destroy(gameObject);
        }
    }
}