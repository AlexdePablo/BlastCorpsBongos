using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdificioScript : ObstScriptPadre
{

    [SerializeField]
    private GameObject spawn;

    [SerializeField]
    protected GameEvent onEdificioDestruido;

    private void OnDestroy()
    {
        if (muerto)
        {
            DarPuntos();
            GameObject exp = Instantiate(explosion);
            exp.transform.position = transform.position;
            int num = Random.Range(1, 10);
            for (int i = 0; i < num; i++)
            {
                GameObject obj = Instantiate(spawn);
                obj.transform.position = transform.position;
            }
        }
    }
    protected void DarPuntos()
    {
        onEdificioDestruidoInt.Raise(puntosPorEdificio);
        onEdificioDestruido.Raise();
    }
}
