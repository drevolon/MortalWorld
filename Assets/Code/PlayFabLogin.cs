using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
public class PlayFabLogin : MonoBehaviour
{
    private string _username;
    private string _mail;
    private string _pass;
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

        //if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        //{
        //    ;
        //    PlayFabSettings.staticSettings.TitleId = "90647";
        //}
        //var request = new LoginWithCustomIDRequest
        //{
        //    CustomId = GetUserName(),
        //    CreateAccount = true
        //};
        //PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log($"Congratulations, you made successful API call! User {_username} success login");
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