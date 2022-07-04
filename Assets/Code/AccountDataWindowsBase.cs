using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountDataWindowsBase : MonoBehaviour
{
    [SerializeField]
    private InputField _usernameField;

    [SerializeField]
    private InputField _passwordField;

    protected string _username;
    protected string _password;

    void Start()
    {
        SubscriptionsElementsUi();
    }

    protected virtual void SubscriptionsElementsUi()
    {
        _usernameField.onValueChange.AddListener(UpdateUsername);
        _passwordField.onValueChange.AddListener(UpdatePassword);
    }

    private void UpdatePassword(string password)
    {
        _password = password;
    }

    private void UpdateUsername(string username)
    {
        _username= username;
    }
}
