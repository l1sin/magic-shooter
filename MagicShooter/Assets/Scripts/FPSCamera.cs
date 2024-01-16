using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _mouseSensitivityX;
    [SerializeField] private float _mouseSensitivityY;
    private float _verticalRotation;
    private float _horizontalRotation;
    private Vector2 _rotation;

    private void Update()
    {
        Rotate();
        GetInput();
    }

    private void GetInput()
    {
        _rotation = new Vector2(CharacterInput.MouseInputX, CharacterInput.MouseInputY);
    }

    private void Rotate()
    {
        _horizontalRotation = _rotation.x * _mouseSensitivityX;
        _player.Rotate(Vector3.up * _horizontalRotation);

        _verticalRotation -= _rotation.y * _mouseSensitivityY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }
}