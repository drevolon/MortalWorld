using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomListCachingExample : ILobbyCallbacks, IConnectionCallbacks
{
    private TypedLobby customLobby = new TypedLobby("customLobby",
    LobbyType.Default);
    private LoadBalancingClient loadBalancingClient;
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string,
    RoomInfo>();
    public void JoinLobby()
    {
        loadBalancingClient.OpJoinLobby(customLobby);
    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
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
        }
    }
    // do not forget to register callbacks via loadBalancingClient.AddCallbackTarget
    // also deregister via loadBalancingClient.RemoveCallbackTarget
    void ILobbyCallbacks.OnJoinedLobby()
    {
        cachedRoomList.Clear();
    }
    void ILobbyCallbacks.OnLeftLobby()
    {
        cachedRoomList.Clear();
    }
    void ILobbyCallbacks.OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // here you get the response, empty list if no rooms found
        UpdateCachedRoomList(roomList);
    }
    void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        
    }

    public void OnConnected()
    {
       
    }

    public void OnConnectedToMaster()
    {
       
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }
}
