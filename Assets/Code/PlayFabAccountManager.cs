using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;
using UnityEngine.UI;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _titleLabel;

    [SerializeField]
    private GameObject _newCharacterCreatePanel;

    [SerializeField]
    private Button _createCharacterButton;

    [SerializeField]
    private TMP_InputField _inputField;

    [SerializeField]
    private List<SlotCharacterWidget> _slots;

    private string _characterName;

    void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoResult, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnError);

        GetCharacter();

        foreach (var slot in _slots)
            slot.SlotButton.onClick.AddListener(OpenPanelCreateCharacter);

        _createCharacterButton.onClick.AddListener(CreateCharacterWithItem);
        _inputField.onValueChanged.AddListener(OnNameChange);
    }

    private void OnDestroy()
    {
        foreach (var slot in _slots)
            slot.SlotButton.onClick.RemoveAllListeners();

        _createCharacterButton.onClick.RemoveAllListeners();
        _inputField.onValueChanged.RemoveAllListeners();
    }

    private void OnNameChange(string characterName)
    {
        _characterName = characterName;
    }

    private void CreateCharacterWithItem()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = "character_item"
        }, result =>
        {
            UpdateCharacterStatistics(result.CharacterId);
        }, OnError);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"Level", 1 },
                {"Gold", 0 },
                {"HP", 100 },
                {"Damage", 5 },
                {"Exp", 1 }
        }
        }, result =>
        {
            Debug.Log("Complete create character!!!");
            ClosePanelCreateCharacter();
            GetCharacter();
        }, OnError);
    }

    private void OpenPanelCreateCharacter()
    {
        _newCharacterCreatePanel.SetActive(true);
    }

    private void ClosePanelCreateCharacter()
    {
        _newCharacterCreatePanel.SetActive(false);
    }

    private void GetCharacter()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                Debug.Log($"Character count: {result.Characters.Count}");
                ShowInfoCharacters(result.Characters);
            }, OnError);
    }

    private void ShowInfoCharacters(List<CharacterResult> characters)
    {
        if (characters.Count == 0)
        {
            foreach (var slot in _slots)
                slot.ShowEmptySlot();
        }
        else if (characters.Count > 0 && characters.Count <= _slots.Count)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
            {
                CharacterId = characters.First().CharacterId
            },
            result =>
            {
                var level = result.CharacterStatistics["Level"].ToString();
                var gold = result.CharacterStatistics["Gold"].ToString();
                var hp = result.CharacterStatistics["HP"].ToString();
                var damage = result.CharacterStatistics["Damage"].ToString();
                var exp = result.CharacterStatistics["Exp"].ToString();

                _slots.First().ShowInfoCharacterSlot(characters.First().CharacterName, level, gold, hp, damage, exp);
            }, OnError);
        }
        else
        {
            Debug.LogError("Added a slots for characters");
        }
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.Log(errorMessage);
    }
    private void OnGetAccountInfoResult(GetAccountInfoResult result)
    {
        var accountInfo = result.AccountInfo;
        _titleLabel.text = $"Username:{accountInfo.Username}, Id: {accountInfo.PlayFabId}";
    }
    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        ShowCatalog(result.Catalog);
        Debug.Log("Complete load catalog!");
    }

    private void ShowCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            if (item.Bundle == null)
                Debug.Log($"item_id: {item.ItemId}");
        }
    }

   
}
