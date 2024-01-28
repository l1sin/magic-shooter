using UnityEngine;

public class SpellButtonGame : SpellButton
{
    [SerializeField] private SpellController _spellController;

    protected override void OnLeftClick()
    {
        _spellController.SetSpell(_spellIndex, 0);
    }
    protected override void OnRightClick()
    {
        _spellController.SetSpell(_spellIndex, 1);
    }
}
