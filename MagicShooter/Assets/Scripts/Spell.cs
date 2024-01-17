using System.Collections;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private float _damage;
    [SerializeField] private float _level;
    [SerializeField] private float _exp;
    [SerializeField] private float _reloadTime;
    [SerializeField] private bool _canShoot;

    [SerializeField] private GameObject _projectile;

    [SerializeField] private ShotType _shotType;

    [SerializeField] private bool _isAttacking;
    private static float s_maxShootingDistance;
    private static float s_destroyEffectTime;
    [SerializeField] private LayerMask _targets;

    [SerializeField] private ParticleSystem _muzzle;
    [SerializeField] private GameObject _tracer;
    [SerializeField] private GameObject _impact;

    private enum ShotType
    {
        HitScan,
        Projectile,
        Beam
    }

    private void Update()
    {
        if (_isAttacking && _canShoot && !UIContorller.InMenu)
        {
            switch (_shotType)
            {
                case ShotType.HitScan:

                    RaycastHit hitInfo;
                    if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, s_maxShootingDistance, _targets))
                    {
                        StartCoroutine(Reload());
                        _muzzle.Play();
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
