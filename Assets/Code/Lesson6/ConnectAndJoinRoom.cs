using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using Photon.Realtime;

public class ConnectAndJoinRoom : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    [SerializeField]
    private ServerSettings _serverSettings;

    [SerializeField]
    private TMP_Text _stateUiText;
    
    [SerializeField]
    private TMP_Text _roomList;

    [SerializeField]
    private TMP_Text _currentRoom;

    [SerializeField]
    private  TMP_InputField _inputFieldNameRoom;

    [SerializeField]
    private Toggle _toggleVisibleRoom;

    [SerializeField]
    private Toggle _toggleOpenRoom;

    [SerializeField]
    private TMP_InputField _expectedUsers;

    [SerializeField]
    private TMP_Text _countRooms;

    [SerializeField]
    private Button _exitButton;

    [SerializeField]
    private Button _connectButton;

    [SerializeField]
    private Button _logOutRoom;

    private TypedLobby customLobby = new TypedLobby("customLobby",  LobbyType.Default);

    private const string GAME_MOD_KEY = "gm";
    private const string AI_MOD_KEY = "ai";
    private const string _roomName = "MyFirstRoom";

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string,  RoomInfo>();

    List<RoomInfo> _roomInfo;

    LoadBalancingClient _lbc;
    private string[] expectedUsers = {"user1", "user2" };

    private void Start()
    {
        _exitButton.onClick.AddListener(OnExitRoom);
        _connectButton.onClick.AddListener(OnConnectRoom);
        _logOutRoom.onClick.AddListener(OnLogOutRoom);

        _inputFieldNameRoom.text = _roomName;

        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);

        if (!_lbc.ConnectUsingSettings(_serverSettings.AppSettings))
            Debug.Log("Error Conncted");
    }

    private void OnLogOutRoom()
    {

        _lbc.OpJoinLobby(customLobby);  
    }

    private void OnConnectRoom()
    {
        if (!_lbc.ConnectUsingSettings(_serverSettings.AppSettings))
            Debug.Log("Error Conncted");

        Debug.Log($"Run OnConnectRoom {_roomName}");
        RoomOptions paramsOptions = new RoomOptions
        {
            IsOpen = _toggleOpenRoom,
            IsVisible = _toggleVisibleRoom,
             PublishUserId=true,
             MaxPlayers=15
          
        };
        var paramsRoom = new EnterRoomParams
        {
            ExpectedUsers= expectedUsers,
            RoomName= _inputFieldNameRoom.text, 
            RoomOptions=paramsOptions
        };
        _lbc.OpJoinOrCreateRoom(paramsRoom);
    }

    private void OnExitRoom()
    {
        _lbc.Disconnect(new DisconnectCause());
    }

    private void Update()
    {
        if (_lbc == null) return;

        _lbc.Service();

        var state = _lbc.State.ToString();
        _stateUiText.text = string.Format("State {0} userID {1}", state, _lbc.UserId);

        if (_expectedUsers.text!="")
        {
            expectedUsers = _expectedUsers.text.Split(',');
        }

            _currentRoom.text = _lbc.CurrentRoom.Name;
            _countRooms.text = _lbc.RoomsCount.ToString();
    }

    private void OnDestroy()
    {
        _lbc.RemoveCallbackTarget(this);
    }

    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster active");
        //_lbc.OpJoinRandomRoom();
        var roomOptions = new RoomOptions {
            MaxPlayers = 12,
            CustomRoomProperties = new Hashtable { { GAME_MOD_KEY, 1 } },
            IsVisible = true,
            PublishUserId = true
        };
        var enterRoomParams = new EnterRoomParams { RoomName= _inputFieldNameRoom.text, RoomOptions = roomOptions };
        _lbc.OpCreateRoom(enterRoomParams);
    }

    public void OnCreatedRoom()
    {
        Debug.Log($"OnCreatedRoom {_inputFieldNameRoom.text}");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed");
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("OnCustomAuthenticationFailed");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnJoinedLobby()
    {
        Debug.Log($"OnJoinedLobby");

        cachedRoomList.Clear();

        
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        _lbc.OpCreateRoom(new EnterRoomParams());
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed");
    }

    public void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");

        cachedRoomList.Clear();
    }

    public void OnLeftRoom()
    {
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    void ILobbyCallbacks.OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        UpdateCachedRoomList(roomList);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        Debug.Log("UpdateCachedRoomList");
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);

            }
            else
            {
                cachedRoomList[info.Name] = info;
            }

            _roomList.text = info.Name;
        }
    }
    //public void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    Debug.Log("OnRoomListUpdate active");

    //    foreach (var itemRoom in roomList)
    //    {
    //        var nameRom = itemRoom.Name;
    //        // TMPro.TMP_Dropdown.OptionData item = new TMP_Dropdown.OptionData ();
    //        // item.text = nameRom;

    //        // _roomList.options.Add(item);
    //        _roomList.text = nameRom;
    //        Debug.Log($"room: {nameRom}");
    //    }     
    //}



}
