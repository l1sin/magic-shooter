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
    public bool[] Maps;
    public int[] Upgrades;
    public float[] Experience;
    public bool[] Achievements;
    public int AllMoney;
    public int Kills;
    public float SFXVolume;
    public float MusicVolume;
    public float MouseSensetivity;
    public StatsController.Stats CurrentStats;
    public bool NoAds;
    public bool CoinPremium;
    public bool ExpPremium;
    public int Difficulty;

    public Progress()
    {
        Money = 0;
        Level = 0;
        CharacterLevel = 0;
        DefaultSpellIndexLeft = 0;
        DefaultSpellIndexRight = 0;
        SelectedMap = 0;
        Maps = new bool[4];
        Upgrades = new int[24];
        Experience = new float[12];
        Achievements = new bool[27];
        AllMoney = 0;
        Kills = 0;
        SFXVolume = 0;
        MusicVolume = 0;
        MouseSensetivity = 1;
        CurrentStats = new StatsController.Stats();
        NoAds = false;
        CoinPremium = false;
        ExpPremium = false;
        Difficulty = 1;
    }
}
