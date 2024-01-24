using Sounds;
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
    

    private void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0) DestroyProjectile();
        if (Physics.OverlapSphere(transform.position, _collisionRadius, Targets).Length > 0)
        {
            DestroyProjectile();
            MakeDamage();
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

    private void MakeDamage()
    {
        PlayAudio();
        DestroyProjectile();
    }

    private void SpawnImpact()
    {
        GameObject particles = Instantiate(Impact, transform.position, Quaternion.identity);
        Destroy(particles, 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _collisionRadius);
    }
}
