using Sounds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Projectile : MonoBehaviour
{
    [Header("Public values")]
    public float Speed;
    public float Damage;
    public float LifeTime;
    public LayerMask Targets;
    public GameObject Impact;
    public AudioMixerGroup AudioMixerGroup;
    public float ImpactVolume = 1;

    [Header("Private values")]
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private AudioClip _destroySFX;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private ProjectileType _type;

    private enum ProjectileType
    {
        HitTarget,
        ExplodeOnHit
    }
    
    private void Update()
    {
        Vector3 translation = Vector3.forward * Speed * Time.deltaTime;
        transform.Translate(translation);

        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0) DestroyProjectile();

        Collider[] collisions = Physics.OverlapSphere(transform.position, _collisionRadius, Targets);
        if (collisions.Length > 0)
        {
            MakeImpact(collisions);
        }
    }

    private void MakeImpact(Collider[] collisions)
    {
        switch (_type)
        {
            case ProjectileType.HitTarget:

                collisions[0].TryGetComponent(out IDamageable damageable);
                if (damageable != null) MakeDamage(damageable);
                PlayAudio();
                DestroyProjectile();

                break;

            case ProjectileType.ExplodeOnHit:

                ExplosionDamage();
                PlayAudio();
                DestroyProjectile();

                break;

            default:
                break;
        }
    }

    private void DestroyProjectile()
    {
        foreach (ParticleSystem ps in _particleSystems)
        {
            DestroyParticles(ps);
        }
        SpawnImpact();
        Destroy(gameObject);
    }

    private void DestroyParticles(ParticleSystem ps)
    {
        ps.transform.parent = null;
        Destroy(ps.gameObject, 5f);
        ParticleSystem.EmissionModule em = ps.emission;
        em.enabled = false;
    }

    private void PlayAudio()
    {
        if (_destroySFX != null)
        {
            AudioSource audio = SoundManager.Instance.PlaySound(_destroySFX, AudioMixerGroup, ImpactVolume);
            audio.spatialBlend = 1;
            audio.minDistance = 10;
            audio.gameObject.transform.position = transform.position;
        }
    }

    private void MakeDamage(IDamageable damageable)
    {
        damageable.GetDamage(Damage);
    }

    private void ExplosionDamage()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, _explosionRadius, Targets);
        HashSet<IDamageable> damageables = new HashSet<IDamageable>();
        foreach (Collider col in collisions)
        {
            damageables.Add(col.GetComponentInParent<IDamageable>());
        }
        foreach (IDamageable damageable in damageables)
        {
            if (damageable != null)
            {
                damageable.GetDamage(Damage);
            }
        }
    }

    private void SpawnImpact()
    {
        GameObject particles = Instantiate(Impact, transform.position, Quaternion.identity);
        Destroy(particles, 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _collisionRadius);
        Gizmos.DrawRay(transform.position, Vector3.forward * Speed * Time.deltaTime);
    }
}
