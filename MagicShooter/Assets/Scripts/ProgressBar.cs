using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _filledImage;
    [SerializeField] private TextMeshProUGUI _text;

    public void UpdateProgressBar(int current, int max)
    {
        _text.text = $"{current}/{max}";
        _filledImage.fillAmount = (float)current / (float)max;
    }
}
