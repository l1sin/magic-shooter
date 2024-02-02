using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _selfSprite;
    [SerializeField] private Sprite _lockedSprite;
    public void UpdateIcon(bool state)
    {
        if (state) _image.sprite = _selfSprite;
        else _image.sprite = _lockedSprite;
    }
}
