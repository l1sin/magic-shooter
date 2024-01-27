using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private DropType _dropType;
    [SerializeField] private LayerMask _character;
    [SerializeField] private float _curePercent;
    [SerializeField] private float _coinValue;
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
                    break;
                case DropType.Aidkit:
                    other.GetComponent<CharacterHealth>().Cure(_curePercent);
                    break;
                default: break;
            }
            Destroy(gameObject);
        }
    }
}
