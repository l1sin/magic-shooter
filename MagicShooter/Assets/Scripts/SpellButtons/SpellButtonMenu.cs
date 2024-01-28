public class SpellButtonMenu : SpellButton
{
    protected override void OnLeftClick()
    {
        MenuController.Instance.SetDefaultSpells(_spellIndex, 0);
        SaveManager.Instance.CurrentProgress.DefaultSpellIndexLeft = _spellIndex;
    }
    protected override void OnRightClick()
    {
        MenuController.Instance.SetDefaultSpells(_spellIndex, 1);
        SaveManager.Instance.CurrentProgress.DefaultSpellIndexRight = _spellIndex;
    }
}
