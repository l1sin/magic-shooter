using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        _currentSpellIndexLeft = 0;
        _currentSpellIndexRight = 0;
        SetSpell(_currentSpellIndexLeft, 0);
        SetSpell(_currentSpellIndexRight, 1);
    }

    public void SetSpell(int spellIndex, int hand)
    {
        if (hand == 0)
        {
            _currentSpellIndexLeft = spellIndex;
            _currentSpellLeft = SpellsLeft[spellIndex];
            foreach (GameObject g in SpellObjectsLeft)
            {
                g.SetActive(false);
            }
            SpellObjectsLeft[_currentSpellIndexLeft].SetActive(true);

        }
        else if (hand == 1)
        {
            _currentSpellIndexRight = spellIndex;
            _currentSpellRight = SpellsRight[spellIndex];
            foreach (GameObject g in SpellObjectsRight)
            {
                g.SetActive(false);
            }
            SpellObjectsRight[_currentSpellIndexRight].SetActive(true);
        }
    }
}
