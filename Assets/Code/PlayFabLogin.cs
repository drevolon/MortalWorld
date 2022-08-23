using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayFabLogin : MonoBehaviour
{
    private string _username;
    private string _mail;
    private string _pass;

    private string _playerKey = "HP";

    private const string AuthGuidKey = "auth_guid";

    private string _id_user;

    private int _playHP;

    public string PlayHP { get; set; }

    public void UpdateUsername(string username)
    {
        _username = username;
    }
    public void UpdateEmail(string mail)
    {
        _mail = mail;
    }
    public void UpdatePassword(string pass)
    {
        _pass = pass;
    }

    
    public void Start()
    {

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "90647";
        }

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        _id_user = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

        var request = new LoginWithCustomIDRequest
        {
            CustomId = _id_user,
            CreateAccount = !needCreation
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log($"Congratulations, you made successful API call! User {_username} success login");
        PlayerPrefs.SetString(AuthGuidKey, _id_user);
        SetUserData(result.PlayFabId, _playerKey);
        MakePurchase();
        GetInventory();
    }

    private void SetUserData(string playFabId, string playerKey)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> { { playerKey ,  "800"} }
        },
        result =>
        {
            Debug.Log($"Success UPDATE. {playerKey}");

            GetUserData(playFabId, _playerKey);

            

        },
        result => { Debug.Log($"Fail GET {playerKey} for user {playFabId}"); }
        ); 
    }
    private void GetUserData(string playFabId, string playerKey)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId
        },
        result => {
            PlayHP = result.Data[playerKey].Value;
            Debug.Log($"Success GET. {playerKey}:{PlayHP}");
            
        },
        result => { Debug.Log($"Fail GET {playerKey} for user {playFabId}"); }
        ); 
    }

    private void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "",
            ItemId = "FirstAid",
            Price = 10,
            VirtualCurrency = "CR"
        }, result =>
        {
            Debug.Log("Success purchase Antidote");
        }, error =>
        {
            Debug.Log("Fail purchase Antidote");
        });
    }

    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result => LogSuccess(result.Inventory),
            error =>
            {
                Debug.Log("Fail Get data Inventory");
            });
    }

    private void LogSuccess(List<ItemInstance> inventory)
    {
        var firstItem = inventory.First();
        Debug.Log($"firstItem {firstItem.ItemId}");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private string  GetUserName()
    {
        return _username = "FirstUserTestFotGB";// Guid.NewGuid().ToString();
    }
    public void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username,
            Email = _mail,
            Password = _pass,
            RequireBothUsernameAndEmail = true
        }, result =>
        {
            Debug.Log($"Success: {_username}");
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });
    }

}