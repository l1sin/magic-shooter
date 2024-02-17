using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _frame;
    [SerializeField] private Image _line;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Volume _vignetteVolume;
    
    [SerializeField] private float _vignetteThreshold;
    private float _colorStep;

    [SerializeField] private Color[] _colors;

    public void Start()
    {
        _colorStep = 1f / _colors.Length;
    }

    public void UpdateHealthBar(float current, float max)
    {
        if (current < 0) current = 0;
        float percent = current / max;
        _line.fillAmount = percent;
        _text.text = $"{Mathf.FloorToInt(current)}/{Mathf.FloorToInt(max)}";
        int colorIndex;
        if (percent == 1) colorIndex = _colors.Length - 1;
        else colorIndex = Mathf.FloorToInt(percent / _colorStep);
        _line.color = _colors[colorIndex];
        _frame.color = new Color(_colors[colorIndex].r * 0.5f, _colors[colorIndex].g * 0.5f, _colors[colorIndex].b * 0.5f, _frame.color.a);

        _vignetteVolume.weight = 1 - percent;
    }
}
