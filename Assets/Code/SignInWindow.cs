using PlayFab;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using System.Collections;

public class SignInWindow: AccountDataWindowsBase
{
    [SerializeField]
    private Button _signInButton;

    [SerializeField]
    private Button _signOutButton;

    [SerializeField]
    private Text _textStatus;

    private bool _loginResult=false;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _signInButton.onClick.AddListener(SignIn);
        _signOutButton.onClick.AddListener(SignOut);
    }

    private void SignOut()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        Debug.Log($"Success logout: {_username}");
        _textStatus.color = Color.blue;
        _textStatus.text = $"Success logout: {_username}";
    }

    private void SignIn()   
    {
        //StartCoroutine(LoadDataClient());
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password
        }, ResultLoad, error =>
        {
            Debug.Log($"Error: {error.ErrorMessage}");
            _textStatus.color = Color.red;
            _textStatus.text = $"Error login: {_username}";
        });
        
    }

    private void ResultLoad(LoginResult loginResult)
    {
        _loginResult = true;

        Debug.Log($"Success: {_username}");
        _textStatus.color = Color.green;
        _textStatus.text = $"Success login: {_username}";
        EnterInGameScene();
    }

    IEnumerator LoadDataClient()
    {
        while (!_loginResult)
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
            {
                Username = _username,
                Password = _password
            }, ResultLoad, error =>
            {
                Debug.Log($"Error: {error.ErrorMessage}");
                _textStatus.color = Color.red;
                _textStatus.text = $"Error login: {_username}";
            });
            yield return null;
        }
    }
}
