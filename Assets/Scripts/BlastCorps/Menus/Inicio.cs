using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inicio : MonoBehaviour
{
    [SerializeField]
    private GameObject panelNombre;
    [SerializeField]
    private GameObject BotonCargar;
    [SerializeField]
    private GameObject BotonNuevo;
    [SerializeField]
    private TextMeshProUGUI TextoNombre;

    // Start is called before the first frame update
    void Start()
    {
        CerrarNombre();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AbrirNombre()
    {
        panelNombre.SetActive(true);
        BotonCargar.SetActive(false);
        BotonNuevo.SetActive(false);
    }

    public void CerrarNombre()
    {
        panelNombre.SetActive(false);
        BotonCargar.SetActive(true);
        BotonNuevo.SetActive(true);
    }
    public void SetName()
    {
        GameManager.Instance.SetName(TextoNombre.text);
    }

    public void CargarPartida()
    {
        GameManager.Instance.CargarPartida();
    }

}
