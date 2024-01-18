using Sounds;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Spell : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private FPSCamera _fpsCamera;
    [SerializeField] private bool _cameraShake;
    [SerializeField] private float _shakeAmount;

    [SerializeField] private float _damage;
    [SerializeField] private float _level;
    [SerializeField] private float _exp;
    [SerializeField] private float _reloadTime;
    [SerializeField] private bool _canShoot = true;

    [SerializeField] private GameObject _projectile;

    [SerializeField] private ShotType _shotType;

    [SerializeField] public bool IsAttacking = false;
    private static float s_maxShootingDistance = 100f;
    private static float s_destroyEffectTime = 5f;
    [SerializeField] private LayerMask _targets;

    [SerializeField] private ParticleSystem _muzzle;
    [SerializeField] private GameObject _tracer;
    [SerializeField] private GameObject _impact;

    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioClip _shotSound;

    private enum ShotType
    {
        HitScan,
        Projectile,
        Beam
    }

    private void Start()
    {
        _canShoot = true;
        IsAttacking = false;
    }

    private void Update()
    {
        if (IsAttacking && _canShoot && !UIContorller.InMenu)
        {
            switch (_shotType)
            {
                case ShotType.HitScan:

                    Shoot();

                    RaycastHit hitInfo;
                    if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
                    {     
                        BeamHit(_tracer, hitInfo);
                        SpawnImpact(hitInfo);
                        MakeDamage(hitInfo);
                    }
                    else
                    {
                        BeamMiss(_tracer);
                    }

                    break;
            }
        }
    }

    private void Shoot()
    {
        if (_cameraShake) _fpsCamera.Shake(_shakeAmount);
        SoundManager.Instance.PlaySound(_shotSound, _audioMixerGroup);
        _muzzle.Play();
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_reloadTime);
        _canShoot = true;
    }

    protected virtual void SpawnImpact(RaycastHit raycastHit)
    {
        GameObject particles = Instantiate(_impact, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
        Destroy(particles, s_destroyEffectTime);
    }

    protected virtual void MakeDamage(RaycastHit raycastHit)
    {
        IDamageable damageable = raycastHit.transform.GetComponentInParent<IDamageable>();
        if (damageable != null) damageable.GetDamage(_damage);
    }

    protected void BeamHit(GameObject beam, RaycastHit raycastHit)
    {
        GameObject beamObj = Instantiate(beam, _muzzle.transform.position, Quaternion.identity);
        beamObj.transform.LookAt(raycastHit.point);
        beamObj.transform.localScale = new Vector3(1, 1, raycastHit.distance);
        Destroy(beamObj, s_destroyEffectTime);
    }

    protected void BeamMiss(GameObject beam)
    {
        GameObject beamObj = Instantiate(beam, _muzzle.transform.position, Quaternion.identity);
        beamObj.transform.LookAt(_camera.transform.position + _camera.transform.forward * s_maxShootingDistance);
        beamObj.transform.localScale = new Vector3(1, 1, s_maxShootingDistance);
        Destroy(beamObj, s_destroyEffectTime);
    }


}
