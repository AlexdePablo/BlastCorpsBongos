using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveGame;

[Serializable]
public class SaveGame
{
    [Serializable]
    public struct DataPlayer
    {
        public string namePlayer;
        public int goldPlayer;
        public GameManager.Personajes[] diccionarioMapas;
        public GameManager.Personajes[] diccionarioPersonajes;

        public DataPlayer(string _namePlayer, int _goldPlayer, GameManager.Personajes[] _diccionarioMapas, GameManager.Personajes[] _diccionarioPersonajes)
        {
            namePlayer = _namePlayer;
            goldPlayer = _goldPlayer;
            diccionarioMapas = _diccionarioMapas;
            diccionarioPersonajes = _diccionarioPersonajes;
        }

    }

    public DataPlayer m_PlayerData;

    public void PopulateDataPlayer(ISaveableDataPlayer _playerData)
    {
        m_PlayerData = _playerData.Save();
    }
}

public interface ISaveableDataPlayer
{
    public DataPlayer Save();
    public void Load(DataPlayer _dataPlayer);
}
