using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    [SerializeField] private MapShop _mapShop;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private int _index;

    public void SetPrice(int price)
    {
        _priceText.text = price.ToString();
    }

    public void CheckPrice(int price)
    {
        if (SaveManager.Instance.CurrentProgress.Money >= price) _button.interactable = true;
        else _button.interactable = false;
    }

    public void Buy()
    {
        _mapShop.BuyMap(_index);
    }
}
