using Cinemachine;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _noize;
    [SerializeField] private float _shakeResetSpeed;
    [SerializeField] private float _minShake = 0f;
    [SerializeField] private float _maxShake = 5f;

    [SerializeField] private Transform _player;
    [SerializeField] public float MouseSensitivityX = 1f;
    [SerializeField] public float MouseSensitivityY = 1f;
    private float _verticalRotation;
    private float _horizontalRotation;
    private Vector2 _rotation;

    private void Start()
    {
        _noize = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        Rotate();
        GetInput();
        ResetShake();
    }

    private void GetInput()
    {
        _rotation = new Vector2(CharacterInput.MouseInputX, CharacterInput.MouseInputY);
    }

    private void Rotate()
    {
        _horizontalRotation = _rotation.x * MouseSensitivityX;
        _player.Rotate(Vector3.up * _horizontalRotation);

        _verticalRotation -= _rotation.y * MouseSensitivityY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

    private void ResetShake()
    {
        if (_noize.m_AmplitudeGain > _minShake) _noize.m_AmplitudeGain -= Time.deltaTime * _shakeResetSpeed;
        else if (_noize.m_AmplitudeGain < _minShake) _noize.m_AmplitudeGain = _minShake;
        _minShake = 0;
    }

    public void Shake(float shake)
    {
        _noize.m_AmplitudeGain += shake;
        if (_noize.m_AmplitudeGain > _maxShake) _noize.m_AmplitudeGain = _maxShake;
    }

    public void SetShake(float shake)
    {
        _minShake = shake;
    }
}