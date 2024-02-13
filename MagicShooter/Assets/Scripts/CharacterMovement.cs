using Sounds;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float DefaultSpeed;
    [SerializeField] public float CurrentSpeed;
    [SerializeField] private float _speedBuff;
    [SerializeField] private Vector3 MoveInput;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private float _stickDistance;
    [SerializeField] private float _stickForce;
    [SerializeField] private float _jumpTime;
    [SerializeField] private bool _jumpState = false;
    [SerializeField] private float _sprintCoef;

    [Header("Jump")]
    [SerializeField] public float JumpHeight;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] public bool IsGrounded = true;
    [SerializeField] private AudioClip[] _stepSounds;
    [SerializeField] private AudioClip[] _jumpSounds;
    [SerializeField] private AudioClip[] _landSounds;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    

    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        DefaultSpeed = SaveManager.Instance.CurrentProgress.CurrentStats.Speed;
        IsGrounded = true;
    }

    private void Update()
    {
        Move();
        CheckIfGrounded();
        StickToGround();
        JumpInput();
        Fall();
        ApplyVerticalVelocity();
        ToggleAnimationState();
    }

    // Called from Animator event
    public void MakeStepSound()
    {
        SoundManager.Instance.PlaySoundRandom(_stepSounds, _audioMixerGroup);
    }

    private void StickToGround()
    {
        if (_characterController.isGrounded)
        {
            if (Physics.Raycast(transform.position, Vector3.down, (_characterController.height / 2) + _stickDistance, _whatIsGround))
            {
                if (!_jumpState) _velocity.y = -_stickForce;
            }
            else
            {
                _velocity.y = 0;
            }
        }
    }

    private void Move()
    {
        MoveInput = new Vector2(CharacterInput.MoveInputX, CharacterInput.MoveInputY);
        if (MoveInput == default) return;
        Vector3 movement = transform.right * MoveInput.x + transform.forward * MoveInput.y;
        CurrentSpeed = DefaultSpeed;
        if (CharacterInput.Sprint && IsGrounded) CurrentSpeed *= _sprintCoef;
        _characterController.Move(movement * CurrentSpeed * Time.deltaTime);
    }

    private void CheckIfGrounded()
    {
        if (IsGrounded)
        {
            IsGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _whatIsGround);
            if (!IsGrounded)
            {
                SoundManager.Instance.PlaySoundRandom(_jumpSounds, _audioMixerGroup);
            }
        }
        else
        {
            IsGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _whatIsGround);
            if (IsGrounded)
            {
                SoundManager.Instance.PlaySoundRandom(_landSounds, _audioMixerGroup);
            }
        }
    }

    private void Fall()
    {
        if (!_characterController.isGrounded)
        {
            _velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void ApplyVerticalVelocity()
    {
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void ToggleAnimationState()
    {
        if (IsGrounded && MoveInput != default) _animator.SetBool("IsRunning", true);
        else _animator.SetBool("IsRunning", false);
    }

    private void JumpInput()
    {
        if (CharacterInput.Jump)
        {
            if (IsGrounded)
            {
                _velocity.y = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
                StartCoroutine(JumpCountdown());
            } 
        }
    }
    IEnumerator JumpCountdown()
    {
        _jumpState = true;
        yield return new WaitForSeconds(_jumpTime);
        _jumpState = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        Gizmos.DrawRay(transform.position, Vector3.down * (_stickDistance + _characterController.height/2));
    }
}

