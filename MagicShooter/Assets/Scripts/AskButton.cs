using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AskButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _tintColor;
    [SerializeField] private GameObject _tip;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _tip.SetActive(true);
        _image.color = _tintColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tip.SetActive(false);
        _image.color = Color.white;
    }
}
