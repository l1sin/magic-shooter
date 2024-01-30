using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private int _upgradeIndex;
    [SerializeField] private UpgradeType _upgradeType;

    private enum UpgradeType
    {
        Spell,
        Skill
    }

    private void Start()
    {
        _button.onClick.AddListener(ShowInfo);
    }

    private void ShowInfo()
    {
        if (_upgradeType == UpgradeType.Spell)
        {
            UpgradeController.Instance.SetMagic(_upgradeIndex, _image.sprite);
        }
    }
}
