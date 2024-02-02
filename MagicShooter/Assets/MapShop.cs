using UnityEngine;

public class MapShop : MonoBehaviour
{
    [SerializeField] private bool[] _maps;
    [SerializeField] private GameObject[] _buttonObjects;
    [SerializeField] private MapButton[] _buttons;
    [SerializeField] private int[] _prices;

    public void UpdateMapInfo()
    {
        _maps = SaveManager.Instance.CurrentProgress.Maps;
        UnlockMaps();
        SetPrices();
        CheckPrices();
    }

    private void UnlockMaps()
    {
        for (int i = 0; i < _buttonObjects.Length; i++)
        {
            if (_maps[i]) _buttonObjects[i].SetActive(false);
            else _buttonObjects[i].SetActive(true);
        }
    }

    private void SetPrices()
    {
        for (int i = 0; i < _buttonObjects.Length; i++)
        {
            _buttons[i].SetPrice(_prices[i]);
        }
    }

    private void CheckPrices()
    {
        for (int i = 0; i < _buttonObjects.Length; i++)
        {
            _buttons[i].CheckPrice(_prices[i]);
        }
    }

    public void BuyMap(int index)
    {
        MenuController.Instance.SpendMoney(_prices[index]);
        _maps[index] = true;
        SaveManager.Instance.CurrentProgress.Maps = _maps;
        UpdateMapInfo();
        MenuController.Instance.UpdateMapsBar();
        AchievementController.Instance.CheckType(AchievementController.Achievement.AchievementType.Maps);
    }
}
