using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class GameManager : MonoBehaviour, ISaveableDataPlayer
{
    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get { return m_Instance; }
    }

    public enum PersonajeSeleccionado { MECHA_SALTO, MECHA_PUNYO, TRACTOR, CAMION_PUNYO }
    private PersonajeSeleccionado m_PersonajeSeleccionado;
    public PersonajeSeleccionado PersonajeSelecionado => m_PersonajeSeleccionado;
    public enum MapaSeleccionado { MAPA_1, MAPA_2 }
    private MapaSeleccionado m_MapaSeleccionado;

    [SerializeField]
    private int m_Dinero;
    private string m_Name;
    public string Name => m_Name;
    public int Dinero => m_Dinero;

    public Action<int> onCanviDiners;

    [Serializable]
    public struct Personajes
    {
        public string NombreJugador;
        public string NombreEnum;
        public int DineroJugador;
        public bool tengui;

        public Personajes(string _NombreJugador, string _NombreEnum, int _DineroJugador, bool _tengui)
        {
            NombreJugador = _NombreJugador;
            NombreEnum = _NombreEnum;
            DineroJugador = _DineroJugador;
            tengui = _tengui;
        }
    }
    private Personajes[] m_ListaPersonajes =
{
        new Personajes("Tractor", PersonajeSeleccionado.TRACTOR.ToString(), 3000, true),
        new Personajes("Mecha Salto", PersonajeSeleccionado.MECHA_SALTO.ToString(), 5000, false),
        new Personajes("Tractor Putiaso", PersonajeSeleccionado.CAMION_PUNYO.ToString(), 7000, false),
        new Personajes("Mecha Putiaso", PersonajeSeleccionado.MECHA_PUNYO.ToString(), 9000, false)
    };

    private Personajes[] m_ListaMapas =
    {
        new Personajes("Mapa 1", MapaSeleccionado.MAPA_1.ToString(), 3000, true),
        new Personajes("Mapa 2", MapaSeleccionado.MAPA_2.ToString(), 5000, false),
    };

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadData();
    }
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SeleccionMapaPersonaje")
        {
            GetComponent<SaveDataManager>().SaveData();
        }
        else if (scene.name == "Mundo1")
        {

        }
        else if (scene.name == "Mundo2")
        {

        }
    }

    public void CargarPartida()
    {
        GetComponent<SaveDataManager>().LoadData();
        SceneManager.LoadScene("SeleccionMapaPersonaje");
    }
    public void SetName(string Name)
    {
        m_Name = Name;
        SceneManager.LoadScene("SeleccionMapaPersonaje");
    }
    private void loadData()
    {
        m_Dinero = 0;
    }
    public void Comprar(int dinero)
    {
        m_Dinero -= dinero;
        onCanviDiners?.Invoke(m_Dinero);
    }
    public void getDinero(int dinero)
    {
        m_Dinero += dinero;
    }
    public void CargarMapa(string mapa, string personaje)
    {
        m_MapaSeleccionado = getEnumMapa(mapa);
        m_PersonajeSeleccionado = getEnumPersonaje(personaje);
        if (mapa == MapaSeleccionado.MAPA_1.ToString())
            SceneManager.LoadScene("Mundo1");
        else
            SceneManager.LoadScene("Mundo2");
    }

    private PersonajeSeleccionado getEnumPersonaje(string personaje)
    {
        PersonajeSeleccionado perso = PersonajeSeleccionado.MECHA_SALTO;
        switch (personaje)
        {
            case nameof(PersonajeSeleccionado.CAMION_PUNYO):
                perso = PersonajeSeleccionado.CAMION_PUNYO;
                break;
            case nameof(PersonajeSeleccionado.TRACTOR):
                perso = PersonajeSeleccionado.TRACTOR;
                break;
            case nameof(PersonajeSeleccionado.MECHA_SALTO):
                perso = PersonajeSeleccionado.MECHA_SALTO;
                break;
            case nameof(PersonajeSeleccionado.MECHA_PUNYO):
                perso = PersonajeSeleccionado.MECHA_PUNYO;
                break;
            default:
                break;
        }
        return perso;
    }

    private MapaSeleccionado getEnumMapa(string mapa)
    {
        MapaSeleccionado map = MapaSeleccionado.MAPA_1;
        switch (mapa)
        {
            case nameof(MapaSeleccionado.MAPA_1):
                map = MapaSeleccionado.MAPA_1;
                break;
            case nameof(MapaSeleccionado.MAPA_2):
                map = MapaSeleccionado.MAPA_2;
                break;
            default:
                break;
        }
        return map;
    }

    public Personajes[] GetListaMapas
    {
        get { return m_ListaMapas; }
    }
    public Personajes[] GetListaPersonajes
    {
        get { return m_ListaPersonajes; }
    }

    public SaveGame.DataPlayer Save()
    {
        SaveGame.DataPlayer SaveData = new SaveGame.DataPlayer();
        SaveData.namePlayer = m_Name;
        SaveData.goldPlayer = m_Dinero;
        SaveData.diccionarioMapas = m_ListaMapas;
        SaveData.diccionarioPersonajes = m_ListaPersonajes;

        return SaveData;
    }

    public void Load(SaveGame.DataPlayer _dataPlayer)
    {
        m_Dinero = _dataPlayer.goldPlayer;
        m_Name = _dataPlayer.namePlayer;
        m_ListaMapas = _dataPlayer.diccionarioMapas;
        m_ListaPersonajes = _dataPlayer.diccionarioPersonajes;
    }
}

