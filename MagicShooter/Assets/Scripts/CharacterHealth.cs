using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] public float HealthCurrent;
    [SerializeField] public float HealthMax;
    [SerializeField] private float _healthBuff;
    [SerializeField] private bool _isDead;
    [SerializeField] private Image _hpBar;
    [SerializeField] private AudioClip _hurt;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    public int HitsTaken;

    public void Start()
    {
        HealthMax = SaveManager.Instance.CurrentProgress.CurrentStats.Health;
        HealthCurrent = HealthMax;
        UpdateHealthBar();
    }
    
    public void Die()
    {
        //LevelController.Instance.ShowDeathScreen();
    }

    public void GetDamage(float damage)
    {
        if (!_isDead)
        {
            SoundManager.Instance.PlaySound(_hurt, _audioMixerGroup);
            HitsTaken++;
            HealthCurrent -= damage;
            UpdateHealthBar();
            if (HealthCurrent <= 0)
            {
                Die();
            }
        }
    }

    public void Cure(float curePercent)
    {
        HealthCurrent += HealthMax * curePercent;
        if (HealthCurrent > HealthMax) HealthCurrent = HealthMax;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        _hpBar.fillAmount = HealthCurrent / HealthMax;
    }
}
