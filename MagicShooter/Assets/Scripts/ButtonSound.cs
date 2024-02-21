using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioMixerGroup _group;
    [SerializeField][Range(0f, 1f)] private float _volume;

    private void OnEnable()
    {
        _button.onClick.AddListener(PlaySoundOnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void PlaySoundOnClick()
    {
        SoundManager.Instance.PlaySound(_clip, _group, _volume);
    }
}
