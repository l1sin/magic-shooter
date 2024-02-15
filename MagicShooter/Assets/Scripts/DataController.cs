using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController Instance;
    public int[,] Experience;
    public int[,] Price;
    public string[,] Dictionary;
    public string[] Localization;
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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

    }

    public void LoadLanguage(string language)
    {
        Dictionary = Utility.Utility.ReadCSVString("Localization");

        int id = GetLanguageId(language);
        Localization = new string[Dictionary.GetLength(0) - 1];

        for (int i = 1; i < Dictionary.GetLength(0); i++)
        {
            Localization[i - 1] = Dictionary[i, id];
        }
    }

    public int GetLanguageId(string language)
    {
        for (int j = 0; j < Dictionary.GetLength(1); j++)
        {
            if (Dictionary[0, j] == language)
            {
                return j;
            }
        }
        Debug.Log("Unknown language - switch to en");
        return GetLanguageId("en");
    }
}
