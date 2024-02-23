using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Tractor;
    [SerializeField]
    private GameObject TractorPutiaso;
    [SerializeField]
    private GameObject MechaSalto;
    [SerializeField]
    private GameObject MechaPutiaso;
    [SerializeField]
    private Transform Spawn;
    [SerializeField]
    private TextMeshProUGUI textoPuntos;

    private int puntos = 0;

    // Start is called before the first frame update
    void Start()
    {
        Init(GameManager.Instance.PersonajeSelecionado);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(GameManager.PersonajeSeleccionado personaje)
    {
        GameObject PersonajeElegido;
        switch (personaje)
        {
            case GameManager.PersonajeSeleccionado.CAMION_PUNYO:
                PersonajeElegido = Instantiate(TractorPutiaso);
                break;
            case GameManager.PersonajeSeleccionado.TRACTOR:
                PersonajeElegido = Instantiate(Tractor);
                break;
            case GameManager.PersonajeSeleccionado.MECHA_PUNYO:
                PersonajeElegido = Instantiate(MechaPutiaso);
                break;
            case GameManager.PersonajeSeleccionado.MECHA_SALTO:
                PersonajeElegido = Instantiate(MechaSalto);
                break;
            default:
                PersonajeElegido = null;
                break;
        }
        PersonajeElegido.transform.position = Spawn.position;
        textoPuntos.text = puntos + "";
    }

    public void SumarPuntos(int _punts)
    {
        puntos += _punts;
        textoPuntos.text = puntos + "";
    }
    public void FinDelMundo(bool _pasado)
    {
        if (_pasado)
            GameManager.Instance.getDinero(puntos);

        SceneManager.LoadScene("SeleccionMapaPersonaje");
    }
}
