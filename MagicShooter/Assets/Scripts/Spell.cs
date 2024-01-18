using Sounds;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Spell : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private FPSCamera _fpsCamera;
    [SerializeField] private float _shakeAmount;

    [SerializeField] private float _damage;
    [SerializeField] private float _level;
    [SerializeField] private float _exp;
    [SerializeField] private float _reloadTime;
    [SerializeField] public bool CanShoot = true;

    [SerializeField] private GameObject _projectile;

    [SerializeField] private ShotType _shotType;
    [SerializeField] private Hand _hand;

    [SerializeField] public bool IsAttacking = false;
    private static float s_maxShootingDistance = 100f;
    private static float s_destroyEffectTime = 5f;
    [SerializeField] private LayerMask _targets;

    [SerializeField] private ParticleSystem _muzzle;
    [SerializeField] private GameObject _tracer;
    [SerializeField] private GameObject _impact;

    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioClip _shotSound;

    [SerializeField] private GameObject _beamObject;
    [SerializeField] private GameObject _beamImpact;
    [SerializeField] private GameObject _beamAudio;

    private enum Hand
    {
        Left,
        Right
    }

    private enum ShotType
    {
        HitScan,
        Projectile,
        Beam
    }

    private void Start()
    {
        CanShoot = true;
        IsAttacking = false;
    }

    private void Update()
    {
        if (IsAttacking && CanShoot && !UIContorller.InMenu)
        {
            switch (_shotType)
            {
                case ShotType.HitScan:

                    HitScanShot();

                    break;

                case ShotType.Projectile: break;

                case ShotType.Beam:

                    BeamShot();

                    break;
            }
        }
        else
        {
            switch (_shotType)
            {
                case ShotType.HitScan: break;

                case ShotType.Projectile: break;

                case ShotType.Beam:

                    BeamOff();

                    break;
            }
        }
    }

    private void HitScanShot()
    {
        Shoot();

        RaycastHit hitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
        {
            TracerHit(_tracer, hitInfo);
            SpawnImpact(hitInfo);
            MakeDamage(hitInfo);
        }
        else
        {
            TracerMiss(_tracer);
        }
    }

    private void BeamShot()
    {
        BeamOn();

        RaycastHit hitInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
        {
            BeamHit(hitInfo);
            MakeDamage(hitInfo);
        }
        else
        {
            BeamMiss();
        }
    }

    private void Shoot()
    {
        _fpsCamera.Shake(_shakeAmount);
        SoundManager.Instance.PlaySound(_shotSound, _audioMixerGroup);
        _muzzle.Play();
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        CanShoot = false;
        yield return new WaitForSeconds(_reloadTime);
        CanShoot = true;
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

    protected void TracerHit(GameObject beam, RaycastHit raycastHit)
    {
        GameObject beamObj = Instantiate(beam, _muzzle.transform.position, Quaternion.identity);
        beamObj.transform.LookAt(raycastHit.point);
        beamObj.transform.localScale = new Vector3(1, 1, raycastHit.distance);
        Destroy(beamObj, s_destroyEffectTime);
    }

    protected void BeamOn()
    {
        _fpsCamera.SetShake(_shakeAmount);
        _beamObject.SetActive(true);
        _beamImpact.SetActive(true);
        _beamAudio.SetActive(true);
    }


    protected void BeamOff()
    {
        _beamObject.SetActive(false);
        _beamImpact.SetActive(false);
        _beamAudio.SetActive(false);
    }

    protected void BeamHit(RaycastHit raycastHit)
    {
        _beamObject.transform.LookAt(raycastHit.point);
        if (_hand == Hand.Left) _beamObject.transform.localScale = new Vector3(-1, -1, -raycastHit.distance);
        else if (_hand == Hand.Right) _beamObject.transform.localScale = new Vector3(1, 1, raycastHit.distance);
        _beamImpact.transform.position = raycastHit.point;
    }

    protected void BeamMiss()
    {
        _beamObject.transform.LookAt(_camera.transform.position + _camera.transform.forward * s_maxShootingDistance);
        if (_hand == Hand.Left) _beamObject.transform.localScale = new Vector3(-1, -1, -s_maxShootingDistance);
        else if (_hand == Hand.Right) _beamObject.transform.localScale = new Vector3(1, 1, s_maxShootingDistance);
        _beamImpact.SetActive(false);
    }

    protected void TracerMiss(GameObject beam)
    {
        GameObject beamObj = Instantiate(beam, _muzzle.transform.position, Quaternion.identity);
        beamObj.transform.LookAt(_camera.transform.position + _camera.transform.forward * s_maxShootingDistance);
        beamObj.transform.localScale = new Vector3(1, 1, s_maxShootingDistance);
        Destroy(beamObj, s_destroyEffectTime);
    }


}
