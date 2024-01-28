using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButtonMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int _spellIndex;
    [SerializeField] private Image _image;
    [SerializeField] private Color _tintColor;
    private bool _leftHold;
    private bool _rightHold;
    private float _transitionTime;
    UnityEvent _leftClick = new UnityEvent();
    UnityEvent _rightClick = new UnityEvent();


    private void Start()
    {
        _leftClick.AddListener(SetSpellLeft);
        _rightClick.AddListener(SetSpellRight);
    }

    private void SetSpellLeft()
    {
        MenuController.Instance.SetDefaultSpells(_spellIndex, 0);
    }
    private void SetSpellRight()
    {
        MenuController.Instance.SetDefaultSpells(_spellIndex, 1);
    }

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
