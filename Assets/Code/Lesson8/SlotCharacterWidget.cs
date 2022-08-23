using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotCharacterWidget : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private GameObject _emptySlot;

    [SerializeField]
    private GameObject _infoCharacterSlot;

    [SerializeField]
    private TMP_Text _nameLabel;

    [SerializeField]
    private TMP_Text _levelLabel;

    [SerializeField]
    private TMP_Text _goldLabel;

    [SerializeField]
    private TMP_Text _HPLabel;

    [SerializeField]
    private TMP_Text _damageLabel;

    [SerializeField]
    private TMP_Text _expLabel;

    public Button SlotButton => _button;

    public void ShowInfoCharacterSlot(string name, string level, string gold, string hp, string damage, string exp)
    {
        _nameLabel.text = name;
        _levelLabel.text = level;
        _goldLabel.text = gold;

        _HPLabel.text = hp;
        _damageLabel.text = damage;
        _expLabel.text = exp;

        _infoCharacterSlot.SetActive(true);
        _emptySlot.SetActive(false);
    }

    public void ShowEmptySlot()
    {
        _infoCharacterSlot.SetActive(false);
        _emptySlot.SetActive(true);
    }
}