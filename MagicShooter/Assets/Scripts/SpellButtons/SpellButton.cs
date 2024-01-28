using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected int _spellIndex;
    [SerializeField] protected Image _image;
    [SerializeField] protected Color _tintColor;
    protected bool _leftHold;
    protected bool _rightHold;
    UnityEvent _leftClick = new UnityEvent();
    UnityEvent _rightClick = new UnityEvent();
    protected void Start()
    {
        _leftClick.AddListener(OnLeftClick);
        _rightClick.AddListener(OnRightClick);
    }

    protected virtual void OnLeftClick() { }
    protected virtual void OnRightClick() { }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _leftHold = false;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            _rightHold = false;
        }
        if (!_leftHold && !_rightHold) _image.color = Color.white;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _leftClick.Invoke();
            _leftHold = true;
            _image.color = _tintColor;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            _rightClick.Invoke();
            _rightHold = true;
            _image.color = _tintColor;
        }
    }
}
