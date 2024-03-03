using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image _frame;
    [SerializeField] private Image _line;
    [SerializeField] private Color[] _colors;


    public void UpdateStaminaBar(float current, float max, bool drained)
    {
        if (current < 0) current = 0;
        float percent = current / max;
        _line.fillAmount = percent;

        int colorIndex;
        if (drained) colorIndex = 0;
        else colorIndex = 1;

        _line.color = _colors[colorIndex];
        _frame.color = new Color(_colors[colorIndex].r * 0.5f, _colors[colorIndex].g * 0.5f, _colors[colorIndex].b * 0.5f, _frame.color.a);
    }
}
