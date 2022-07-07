using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccountWindows : AccountDataWindowsBase
{
    [SerializeField]
    private InputField _emailField;

    [SerializeField]
    private Button _createAccountButton;

    private string _email;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();

        _emailField.onValueChange.AddListener(UpdateEmail);
        _createAccountButton.onClick.AddListener(CreateAccount);
    }

    private void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username=_username,
            Email=_email,
            Password=_password
        }, result=>
        {
            Debug.Log($"Success: {_username}");
            EnterInGameScene();
        }, error =>
        {
            Debug.Log($"Error: {_username}");
        });
    }

    private void UpdateEmail(string email)
    {
        _email= email;
    }
}
