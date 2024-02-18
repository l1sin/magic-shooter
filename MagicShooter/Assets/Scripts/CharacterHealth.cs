using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] public float HealthCurrent;
    [SerializeField] public float HealthMax;
    [SerializeField] private bool _isDead;
    [SerializeField] private HealthBar _hpBar;
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
        LevelController.Instance.Lose();
    }

    public float GetDamage(float damage)
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
        return 0;
    }

    public void Cure(float curePercent)
    {
        HealthCurrent += HealthMax * curePercent;
        if (HealthCurrent > HealthMax) HealthCurrent = HealthMax;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        _hpBar.UpdateHealthBar(HealthCurrent, HealthMax);
    }
}
