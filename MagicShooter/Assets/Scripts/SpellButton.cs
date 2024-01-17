using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpellController _spellController;
    [SerializeField] private Button _button;
    [SerializeField] private int _spellIndex;
    UnityEvent _leftClick = new UnityEvent();
    UnityEvent _rightClick = new UnityEvent();

    private void Start()
    {
        //_button.onClick.AddListener(SetSpell);
        _leftClick.AddListener(SetSpellLeft);
        _rightClick.AddListener(SetSpellRight);
    }

    private void SetSpellLeft()
    {
        _spellController.SetSpell(_spellIndex, 0);
    }
    private void SetSpellRight()
    {
        _spellController.SetSpell(_spellIndex, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            _leftClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            _rightClick.Invoke();
    }
}
