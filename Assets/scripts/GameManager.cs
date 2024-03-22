using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject multiMenu, multiLobby;

    [SerializeField] private GameObject PlayerFieldBox, PlayerCardPrefab;
    [SerializeField] private GameObject readyButton, notReadyButton, startButton;

    public Dictionary<ulong, GameObject> playerInfo = new Dictionary<ulong, GameObject>();

    public bool connected;
    public bool inGame;
    public bool isHost;
    public ulong myClientId;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }



    public void HostCreated()
    {
        multiMenu.SetActive(false);
        multiLobby.SetActive(true);
        isHost = true;
        connected = true;
    }

    public void ConnectedAsClient()
    {
        multiMenu.SetActive(false);
        multiLobby.SetActive(true);
        isHost = false;
        connected = true;
    }

    public void Disconnected()
    {
        playerInfo.Clear();
        GameObject[] PlayerCards = GameObject.FindGameObjectsWithTag("PlayerCard");
        foreach (GameObject card in PlayerCards)
        {
            Destroy(card);
        }

        multiMenu.SetActive(true);
        multiLobby.SetActive(false);
        isHost = false;
        connected = false;
    }

    public void AddPlayerToDictionary(ulong _clientId, string _steamName, ulong _steamId)
    {
        if (!playerInfo.ContainsKey(_clientId))
        {
            PlayerInfo _pi = Instantiate(PlayerCardPrefab, PlayerFieldBox.transform).GetComponent<PlayerInfo>();
            _pi.steamId = _steamId;
            _pi.name = _steamName;
            playerInfo.Add(_clientId, _pi.gameObject);
        }
    }

    public void UpdateClients()
    {
        foreach (KeyValuePair<ulong, GameObject> _player in playerInfo)
        {
            ulong _steamId = _player.Value.GetComponent<PlayerInfo>().steamId;
            string _steamName = _player.Value.GetComponent<PlayerInfo>().steamName;
            ulong _clientId = _player.Key;

            NetworkTransmission.instance.UpdateClientsPlayerInfoClientRPC(_steamId, _steamName, _clientId);
        }
    }
    public void RemovePlayerFromDictionary(ulong _steamId)
    {
        GameObject _value = null;
        ulong _key = 100;
        foreach (KeyValuePair<ulong, GameObject> _player in playerInfo)
        {
            if (_player.Value.GetComponent<PlayerInfo>().steamId == _steamId)
            {
                _value = _player.Value;
                _key = _player.Key;
            }
        }
        if (_key != 100)
        {
            playerInfo.Remove(_key);
        }
        if (_value != null)
        {
            Destroy(_value);
        }

    }
    public void ReadyButton(bool _ready)
    {
        NetworkTransmission.instance.IsTheClientReadyServerRpc(_ready, myClientId);
    }

    public bool CheckIfPlayersAreReady()
    {
        bool _ready = false;

        foreach (KeyValuePair<ulong, GameObject> _player in playerInfo)
        {
            if (!_player.Value.GetComponent<PlayerInfo>().isReady)
            {
                startButton.SetActive(false);
                return false;
            }
            else
            {
                startButton.SetActive(true);
                _ready = true;

            }
        }
        return true;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
