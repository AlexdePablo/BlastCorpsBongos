using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlastCorps : MonoBehaviour
{
    private GameObject camion;

    [SerializeField]
    private Vector3 offset;

    public void Init()
    {
        camion = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(camion != null)
            transform.position = new Vector3(camion.transform.position.x + offset.x, camion.transform.position.y + offset.y, camion.transform.position.z + offset.z);
    }
}
