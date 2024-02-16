using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _character;
    [SerializeField] private List<GameObject> _enemiesToSpawn;
    [SerializeField] private float _enemiesCount;
    [SerializeField] private float _minSpawnRadius;
    [SerializeField] private float _maxSpawnRadius;
    [SerializeField] private float _spawnPerSecond;
    [SerializeField] private int _enemiesPerLevel;
    [SerializeField] private int _startEnemies;

    [SerializeField] private Color[] _tierColors;

    [SerializeField] private int _level;
    [SerializeField] private float _dropChance;

    [SerializeField] public static int AllEnemiesCount;
    [SerializeField] public int DeadEnemies;
    [SerializeField] private int _spawnedEnemies;
    [SerializeField] private TextMeshProUGUI _deathCountText;

    [SerializeField] public float Timer;

    [SerializeField] private float _winTimer;

    [SerializeField] private LayerMask _ground;

    private float _spawnPeriod;
    private float _spawnTimer;

    private void Start()
    {
        _level = SaveManager.Instance.CurrentProgress.Level;

        AllEnemiesCount = _startEnemies + _level * _enemiesPerLevel;

        _spawnPeriod = 1 / _spawnPerSecond;
        _spawnTimer = _spawnPeriod;
        for (int i = 0; i < _enemiesCount; i++)
        {
            SpawnEnemy();
        }
        UpdateDeathCount();
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            _spawnTimer = _spawnPeriod;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (_spawnedEnemies < AllEnemiesCount)
        {
            int i;
            do
            {
                i = Random.Range(0, _enemiesToSpawn.Count);
            }
            while (i > _level);

            int maxTier = Mathf.Min((_level - i) / 5, _tierColors.Length - 1);
            int tier = Random.Range(0, maxTier + 1);
            GameObject enemyToSpawn = _enemiesToSpawn[i];


            Vector3 randomPosition;
            randomPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            randomPosition *= Random.Range(_minSpawnRadius, _maxSpawnRadius);

            Physics.Raycast(new Vector3(randomPosition.x, 200, randomPosition.z), Vector3.down * 200, out RaycastHit hitInfo, 300, _ground);
            Vector3 spawnPosition = hitInfo.point;

            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            Enemy enemy = newEnemy.GetComponent<Enemy>();

            enemy.SetTier(tier, _tierColors[tier]);
            enemy.FollowTarget = _character;
            enemy.EnemySpawner = this;
            enemy.DropChance = _dropChance;
            _spawnedEnemies++;
        }
    }

    public void IncrementDead()
    {
        DeadEnemies++;
        UpdateDeathCount();
        if (DeadEnemies >= AllEnemiesCount)
        {
            StartCoroutine(FinishLevel());
        }
    }

    public IEnumerator FinishLevel()
    {
        yield return new WaitForSeconds(_winTimer);
        LevelController.Instance.Win();
    }

    public void UpdateDeathCount()
    {
        _deathCountText.text = $"{DeadEnemies}/{AllEnemiesCount}";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _minSpawnRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _maxSpawnRadius);
    }
}
