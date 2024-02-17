using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellController : MonoBehaviour
{
    [SerializeField] private Spell _currentSpellLeft;
    [SerializeField] private Spell _currentSpellRight;

    [SerializeField] private int _currentSpellIndexLeft;
    [SerializeField] private int _currentSpellIndexRight;

    [SerializeField] private List<Spell> SpellsLeft;
    [SerializeField] private List<Spell> SpellsRight;

    [SerializeField] private List<GameObject> SpellObjectsLeft;
    [SerializeField] private List<GameObject> SpellObjectsRight;

    [SerializeField] private Image _currentSpellLeftImage;
    [SerializeField] private Image _currentSpellRightImage;
    [SerializeField] private Sprite[] _spellImages;


    public bool LeftHandAttack;
    public bool RightHandAttack;

    private void Start()
    {
        _currentSpellIndexLeft = SaveManager.Instance.CurrentProgress.DefaultSpellIndexLeft;
        _currentSpellIndexRight = SaveManager.Instance.CurrentProgress.DefaultSpellIndexRight;
        SetSpell(_currentSpellIndexLeft, 0);
        SetSpell(_currentSpellIndexRight, 1);
        LoadSpellData();
    }

    public void LoadSpellData()
    {
        for (int i = 0; i < SpellsLeft.Count; i++)
        {
            SpellsLeft[i].SetDamage(i);
        }
        for (int i = 0; i < SpellsRight.Count; i++)
        {
            SpellsRight[i].SetDamage(i);
        }
    }

    public void Update()
    {
        _currentSpellLeft.IsAttacking = LeftHandAttack;
        _currentSpellRight.IsAttacking = RightHandAttack;
    }

    public void SetSpell(int spellIndex, int hand)
    {
        if (hand == 0)
        {
            _currentSpellIndexLeft = spellIndex;
            _currentSpellLeft = SpellsLeft[spellIndex];
            _currentSpellLeft.CanShoot = true;
            foreach (GameObject g in SpellObjectsLeft)
            {
                g.SetActive(false);
            }
            SpellObjectsLeft[_currentSpellIndexLeft].SetActive(true);
            _currentSpellLeftImage.sprite = _spellImages[spellIndex];

        }
        else if (hand == 1)
        {
            _currentSpellIndexRight = spellIndex;
            _currentSpellRight = SpellsRight[spellIndex];
            _currentSpellRight.CanShoot = true;
            foreach (GameObject g in SpellObjectsRight)
            {
                g.SetActive(false);
            }
            SpellObjectsRight[_currentSpellIndexRight].SetActive(true);
            _currentSpellRightImage.sprite = _spellImages[spellIndex];
        }
    }
}
