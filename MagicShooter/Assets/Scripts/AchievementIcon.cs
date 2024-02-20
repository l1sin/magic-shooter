using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AchievementIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _selfSprite;
    [SerializeField] private Sprite _lockedSprite;
    [SerializeField] private int _index;
    [SerializeField] private Button _button;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioMixerGroup _group;
    [SerializeField][Range(0f, 1f)] private float _volume;

    public void UpdateIcon(bool state)
    {
        if (state) _image.sprite = _selfSprite;
        else _image.sprite = _lockedSprite;
    }

    public void Start()
    {
        _button.onClick.AddListener(Focus);
    }

    public void Focus()
    {
        AchievementController.Instance.FocusAchivement(_index, _image.sprite);
        PlaySoundOnClick();
    }

    private void PlaySoundOnClick()
    {
        SoundManager.Instance.PlaySound(_clip, _group, _volume);
    }
}
