using Sounds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public EnemySpawner EnemySpawner;
    [SerializeField] public Transform FollowTarget;
    [SerializeField] public float DropChance = 0.05f;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected float _speedCoef;
    [SerializeField] protected float _speedModificator;
    [SerializeField] protected GameObject _particles;
    [SerializeField] protected Transform _explosionPlace;
    [SerializeField] protected Transform _attackCollider;
    [SerializeField] protected float _attackRadius;
    [SerializeField] protected LayerMask _attackLayerMask;
    [SerializeField] protected LayerMask _ground;
    [SerializeField] protected float _damage;
    [SerializeField] protected bool _inAttackRange;

    [SerializeField] public float HealthCurrent;
    [SerializeField] public float HealthMax;
    [SerializeField] protected bool _isDead;
    [SerializeField] protected List<GameObject> _toDestroy;

    [SerializeField] protected float _healthBuff;
    [SerializeField] protected float _damageBuff;
    [SerializeField] protected float _speedBuff;

    [SerializeField] protected GameObject[] _drops;

    [SerializeField] protected AudioClip _deathSound;
    [SerializeField] protected AudioClip[] _meatExplosion;
    [SerializeField] protected AudioMixerGroup _audioMixerGroup;

    [SerializeField] protected static float _easyDifficultyCoef = 0.75f;
    [SerializeField] protected static float _mediumDifficultyCoef = 1f;
    [SerializeField] protected static float _hardDifficultyCoef = 1.5f;

    [SerializeField] protected bool _isBoss;
    [SerializeField] public string Name;
    [SerializeField] public HealthBar BossHPBar;

    private void Init()
    {
        HealthCurrent = HealthMax;
        _speedModificator = _agent.speed / _speedCoef;
        _animator.SetFloat("SpeedModificator", _speedModificator);

    }
    protected virtual void Update()
    {
        if (!_isDead)
        {
            _inAttackRange = Physics.CheckSphere(_attackCollider.position, _attackRadius, _attackLayerMask);
            _animator.SetBool("InAttackRange", _inAttackRange);
            _agent.destination = FollowTarget.position;
        }
    }

    public void SetTier(int tier, Color color)
    {
        SetColor(color);
        SetBuff(tier);
        SetDifficulty();
        Init();
    }

    public void SetBoss(Color color)
    {
        SetColor(color);
        SetDifficulty();
        Init();
        BossHPBar.UpdateHealthBar(HealthCurrent, HealthMax);
    }

    private void SetDifficulty()
    {
        int dif  = SaveManager.Instance.CurrentProgress.Difficulty;
        switch (dif)
        {
            case 0:
                HealthMax *= _easyDifficultyCoef;
                _damage *= _easyDifficultyCoef;
                break;
            case 1:
                HealthMax *= _mediumDifficultyCoef;
                _damage *= _mediumDifficultyCoef;
                break;
            case 2:
                HealthMax *= _hardDifficultyCoef;
                _damage *= _hardDifficultyCoef;
                break;
            default: break;
        }
    }

    private void SetBuff(int tier)
    {
        HealthMax *= Mathf.Pow(_healthBuff, tier);
        _damage *= Mathf.Pow(_damageBuff, tier);
        _agent.speed *= Mathf.Pow(_speedBuff, tier);
    }

    public void SetColor(Color color)
    {
        SkinnedMeshRenderer smr = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        foreach (Material m in smr.materials)
        {
            if (m.name == "Green (Instance)") m.color = color;
        }
    }

    public virtual void MakeDamage() { }

    public void Die()
    {
        if (!_isBoss)
        {
            EnemySpawner.IncrementDead();
            DropItem();
        }
        else
        {
            EnemySpawner.BossDefeated();
        }
        
    }

    public void DropItem()
    {
        if (Random.Range(0f, 1f) <= DropChance * SaveManager.Instance.CurrentProgress.CurrentStats.Luck)
        {
            int itemIndex = Random.Range(0, _drops.Length);
            Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 100, transform.position.z), Vector3.down * 200, out RaycastHit hitInfo, 300, _ground);
            Vector3 spawnPosition = hitInfo.point;
            Instantiate(_drops[itemIndex], spawnPosition + Vector3.up, Quaternion.identity);
        }
    }

    public void DeathSound(AudioClip clip)
    {
        AudioSource audio = SoundManager.Instance.PlaySound(clip, _audioMixerGroup);
        audio.spatialBlend = 1;
        audio.minDistance = 10;
        audio.gameObject.transform.position = transform.position;
    }

    public void DeathSoundRandom(AudioClip[] clip)
    {
        AudioSource audio = SoundManager.Instance.PlaySoundRandom(clip, _audioMixerGroup);
        audio.spatialBlend = 1;
        audio.minDistance = 10;
        audio.gameObject.transform.position = transform.position;
    }

    public float GetDamage(float damage)
    {
        if (!_isDead)
        {
            float lastHealth = HealthCurrent;
            HealthCurrent -= damage;

            if (_isBoss) BossHPBar.UpdateHealthBar(HealthCurrent, HealthMax);

            if (HealthCurrent <= 0)
            {
                SetDead();
                if (damage >= HealthMax)
                {
                    Destroy(Instantiate(_particles, _explosionPlace.position, transform.rotation), 5);
                    DeathSoundRandom(_meatExplosion);
                    Die();
                    Destroy(gameObject);
                }
                else
                {
                    _animator.SetTrigger("Death");
                    DeathSound(_deathSound);
                    Die();
                    Destroy(gameObject, 2);
                }
                return lastHealth;
            }
            else
            {
                return damage;
            }
        }
        return 0;
    }

    private void SetDead()
    {
        _inAttackRange = false;
        _animator.SetBool("InAttackRange", _inAttackRange);
        _isDead = true;
        _agent.isStopped = true;
        foreach(GameObject c in _toDestroy)
        {
            Destroy(c);
        }
    }

    public void StopMovement()
    {
        _agent.isStopped = true;
    }

    public void ContinueMovement()
    {
        _agent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackCollider.position, _attackRadius);
    }
}
