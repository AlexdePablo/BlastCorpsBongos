using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuSeleccionScrip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextoMapa;
    [SerializeField]
    private TextMeshProUGUI TextoPersonaje;

    private GameManager.Personajes[] nombresMapa;
    private GameManager.Personajes[] nombrePersonaje;

    [Header("Dinero y nombre Jugador")]
    [SerializeField]
    private GameObject BotonMapa;
    [SerializeField]
    private GameObject BotonPersonaje;
    [SerializeField]
    private TextMeshProUGUI m_textoBotonMapa;
    [SerializeField]
    private TextMeshProUGUI m_textoBotonJugador;

    [Header("Dinero y nombre Jugador")]
    [SerializeField]
    private TextMeshProUGUI nombreJugador;
    [SerializeField]
    private TextMeshProUGUI dineroJugador;

    private int numMapa;
    private int numPersonaje;



    private void OnEnable()
    {
        nombresMapa = GameManager.Instance.GetListaMapas;
        nombrePersonaje = GameManager.Instance.GetListaPersonajes;
        numMapa = 0;
        numPersonaje = 0;
        TextoMapa.text = nombresMapa[numMapa].NombreJugador;
        TextoPersonaje.text = nombrePersonaje[numPersonaje].NombreJugador;
        nombreJugador.text = GameManager.Instance.Name;
        dineroJugador.text = GameManager.Instance.Dinero + "";
        BotonPersonaje.SetActive(false);
        BotonMapa.SetActive(false);
        GameManager.Instance.onCanviDiners += CanviDiners;
    }

    private void CanviDiners(int obj)
    {
        dineroJugador.text = obj + "";
    }

    public void AvanzarMapa()
    {
        numMapa++;
        if (numMapa > nombresMapa.Length - 1)
            numMapa = 0;
        TextoMapa.text = nombresMapa[numMapa].NombreJugador;

        if (nombresMapa[numMapa].tengui)
            BotonMapa.SetActive(false);
        else
        {
            BotonMapa.SetActive(true);
            m_textoBotonMapa.text = nombresMapa[numMapa].DineroJugador + "";
        }
    }
    public void RetrocederMapa()
    {
        numMapa--;
        if (numMapa < 0)
            numMapa = nombresMapa.Length - 1;
        TextoMapa.text = nombresMapa[numMapa].NombreJugador;

        if (nombresMapa[numMapa].tengui)
            BotonMapa.SetActive(false);
        else
        {
            BotonMapa.SetActive(true);
            m_textoBotonMapa.text = nombresMapa[numMapa].DineroJugador + "";
        }
    }
    public void AvanzarPersonaje()
    {
        numPersonaje++;
        if (numPersonaje > nombrePersonaje.Length - 1)
            numPersonaje = 0;
        TextoPersonaje.text = nombrePersonaje[numPersonaje].NombreJugador;

        if (nombrePersonaje[numPersonaje].tengui)
            BotonPersonaje.SetActive(false);
        else
        {
            BotonPersonaje.SetActive(true);
            m_textoBotonJugador.text = nombrePersonaje[numPersonaje].DineroJugador + "";
        }
    }
    public void RetrocederPersonaje()
    {
        numPersonaje--;
        if (numPersonaje < 0)
            numPersonaje = nombrePersonaje.Length - 1;
        TextoPersonaje.text = nombrePersonaje[numPersonaje].NombreJugador;

        if (nombrePersonaje[numPersonaje].tengui)
            BotonPersonaje.SetActive(false);
        else
        {
            BotonPersonaje.SetActive(true);
            m_textoBotonJugador.text = nombrePersonaje[numPersonaje].DineroJugador + "";
        }
    }
    public void AceptarConfiguracion()
    {
        if (!nombrePersonaje[numPersonaje].tengui && !nombresMapa[numMapa].tengui)
        {
            return;
        }

        else
        {
            GameManager.Instance.CargarMapa(nombresMapa[numMapa].NombreEnum, nombrePersonaje[numPersonaje].NombreEnum);
        }
    }
    public void ComprarMapa()
    {
        if(GameManager.Instance.Dinero > nombresMapa[numMapa].DineroJugador)
        {
            nombresMapa[numMapa].tengui = true;
            GameManager.Instance.Comprar(nombresMapa[numMapa].DineroJugador);
        }
        if (nombresMapa[numMapa].tengui)
            BotonMapa.SetActive(false);
        else
        {
            BotonMapa.SetActive(true);
            m_textoBotonMapa.text = nombresMapa[numMapa].DineroJugador + "";
        }
    }
    public void ComprarPersonaje()
    {
        if (GameManager.Instance.Dinero > nombrePersonaje[numPersonaje].DineroJugador)
        {
            nombrePersonaje[numPersonaje].tengui = true;
            GameManager.Instance.Comprar(nombrePersonaje[numPersonaje].DineroJugador);
        }

        if (nombrePersonaje[numPersonaje].tengui)
            BotonPersonaje.SetActive(false);
        else
        {
            BotonPersonaje.SetActive(true);
            m_textoBotonJugador.text = nombrePersonaje[numPersonaje].DineroJugador + "";
        }
    }
}