using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _filledImage;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _commonColor;
    [SerializeField] private Color _maxFillColor;

    public void UpdateProgressBar(int current, int max)
    {
        _text.text = $"{current}/{max}";
        _filledImage.fillAmount = (float)current / (float)max;
        if (_filledImage.fillAmount < 1) _filledImage.color = _commonColor;
        else _filledImage.color = _maxFillColor;
    }
}
