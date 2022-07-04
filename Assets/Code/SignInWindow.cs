using PlayFab;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignInWindow: AccountDataWindowsBase
{
    [SerializeField]
    private Button _signInButton;

    [SerializeField]
    private Button _signOutButton;

    [SerializeField]
    private Text _textStatus;

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
        PlayFabClientAPI.LoginWithPlayFab(new PlayFab.ClientModels.LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password
        }, result =>
        {
            Debug.Log($"Success: {_username}");
            _textStatus.color = Color.green;
            _textStatus.text = $"Success login: {_username}";

        }, error =>
        {
            Debug.Log($"Error: {error.ErrorMessage}");
            _textStatus.color = Color.red;
            _textStatus.text = $"Error login: {_username}";
        });
    }
}
