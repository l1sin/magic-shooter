using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected int _spellIndex;
    [SerializeField] protected Image _image;
    [SerializeField] protected Color _tintColor;
    [SerializeField] protected GameObject _lockedIcon;
    [SerializeField] protected bool _interactable;
    protected bool _leftHold;
    protected bool _rightHold;
    UnityEvent _leftClick = new UnityEvent();
    UnityEvent _rightClick = new UnityEvent();

    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioMixerGroup _group;
    [SerializeField][Range(0f, 1f)] private float _volume;

    protected void Start()
    {
        _leftClick.AddListener(OnLeftClick);
        _rightClick.AddListener(OnRightClick);
    }

    protected virtual void OnLeftClick() { }
    protected virtual void OnRightClick() { }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_interactable) return;
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
        if (!_interactable) return;
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
        PlaySoundOnClick();
    }

    private void PlaySoundOnClick()
    {
        SoundManager.Instance.PlaySound(_clip, _group, _volume);
    }
}
