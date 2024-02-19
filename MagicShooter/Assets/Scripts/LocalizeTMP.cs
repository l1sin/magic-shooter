using TMPro;
using UnityEngine;

public class LocalizeTMP : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public int LineID;

    public void Start()
    {
        Text.text = DataController.Instance.Dictionary[LineID];
    }
}
