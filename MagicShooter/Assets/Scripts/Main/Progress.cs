using System;

[Serializable]
public class Progress
{
    public int Money;
    public int Level;
    public int CharacterLevel;
    public int DefaultSpellIndexLeft;
    public int DefaultSpellIndexRight;
    public int SelectedMap;

    public Progress()
    {
        Money = 0;
        Level = 0;
        DefaultSpellIndexLeft = 0;
        DefaultSpellIndexRight = 0;
        SelectedMap = 0;
    }
}
