using Sounds;
using UnityEngine;
using UnityEngine.Audio;

public class Drop : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private DropType _dropType;
    [SerializeField] private LayerMask _character;
    [SerializeField] private float _curePercent;
    [SerializeField] private int _coinValue;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioMixerGroup _group;
    [SerializeField][Range(0f, 1f)] private float _volume;
    private enum DropType
    {
        Coin,
        Aidkit
    }
    private void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_character == (_character | (1 << other.gameObject.layer)))
        {
            switch (_dropType)
            {
                case DropType.Coin:
                    LevelController.Instance.CollectCoin(_coinValue * (SaveManager.Instance.CurrentProgress.Level + 1));
                    break;
                case DropType.Aidkit:
                    other.GetComponent<CharacterHealth>().Cure(_curePercent);
                    break;
                default: break;
            }
            PlaySoundOnClick();
            Destroy(gameObject);
        }
    }

    private void PlaySoundOnClick()
    {
        SoundManager.Instance.PlaySound(_clip, _group, _volume);
    }
}
