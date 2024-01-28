using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;
    [SerializeField] private Image DefaultSpellLeftImage;
    [SerializeField] private Image DefaultSpellRightImage;
    [SerializeField] private Sprite[] SpellSprites;

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        
    }

    public void SetDefaultSpells(int spellIndex, int hand)
    {
        if (hand == 0)
        {
            DefaultSpellLeftImage.sprite = SpellSprites[spellIndex];
        }
        else if (hand == 1)
        {
            DefaultSpellRightImage.sprite = SpellSprites[spellIndex];
        }
    }
}
