using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _filledImage;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _commonColor;
    [SerializeField] private Color _maxFillColor;
    [SerializeField] private bool _showFinalNumber;

    public void UpdateProgressBar(float current, float max)
    {
        _text.text = $"{current}/{max}";
        _filledImage.fillAmount = current / max;
        if (_filledImage.fillAmount < 1)
        {
            
            _filledImage.color = _commonColor;
        }
        else
        {
            if (!_showFinalNumber) _text.text = "MAX";
            _filledImage.color = _maxFillColor;
        } 
    }
}
