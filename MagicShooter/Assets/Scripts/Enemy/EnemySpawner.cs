using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _character;
    [SerializeField] private List<GameObject> _enemiesToSpawn;
    [SerializeField] private List<GameObject> _bossesToSpawn;
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
    [SerializeField] private GameObject _bossHPBar;
    [SerializeField] private TextMeshProUGUI _bossNameText;
    [SerializeField] private HealthBar _bossHPBarScript;

    private float _spawnPeriod;
    private float _spawnTimer;
    private bool _bossDefeated;
    private bool _bossSpawned;
    private List<Enemy> _enemies = new List<Enemy>();

    private void Start()
    {
        _level = SaveManager.Instance.CurrentProgress.Level;
        if ((_level + 1) % 5 == 0)
        {
            _bossSpawned = false;
            _bossDefeated = false;
        }
        else
        {
            _bossSpawned = true;
            _bossDefeated = true;
        }

        AllEnemiesCount = _startEnemies + _level * _enemiesPerLevel;

        _spawnPeriod = 1 / _spawnPerSecond;
        _spawnTimer = _spawnPeriod;
        for (int i = 0; i < _enemiesCount; i++)
        {
            Spawn();
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
            Spawn();
        }
    }

    private void Spawn()
    {
        if (_spawnedEnemies < AllEnemiesCount)
        {
            SpawnEnemy();
        }
        else if (!_bossSpawned && DeadEnemies >= AllEnemiesCount)
        {
            SpawnBoss();
        }
    }

    public void SpawnEnemy()
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
        _enemies.Add(enemy);
    }

    public void SpawnBoss()
    {
        int bossIndex = ((_level + 1) / 5) - 1;
        if (bossIndex > 4) bossIndex = 4;

        GameObject enemyToSpawn = _bossesToSpawn[bossIndex];


        Vector3 randomPosition;
        randomPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        randomPosition *= Random.Range(_minSpawnRadius, _maxSpawnRadius);

        Physics.Raycast(new Vector3(randomPosition.x, 200, randomPosition.z), Vector3.down * 200, out RaycastHit hitInfo, 300, _ground);
        Vector3 spawnPosition = hitInfo.point;

        GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        Enemy enemy = newEnemy.GetComponent<Enemy>();

        
        enemy.FollowTarget = _character;
        enemy.EnemySpawner = this;
        enemy.DropChance = 0;
        enemy.BossHPBar = _bossHPBarScript;
        _bossSpawned = true;
        _bossHPBar.SetActive(true);
        _bossNameText.text = DataController.Instance.Dictionary[enemy.NameId];

        enemy.SetBoss(_tierColors[bossIndex]);
    }

    public void IncrementDead()
    {
        DeadEnemies++;
        UpdateDeathCount();
        if (DeadEnemies >= AllEnemiesCount && _bossDefeated)
        {
            StartCoroutine(FinishLevel());
        }
    }

    public void BossDefeated()
    {
        _bossDefeated = true;
        if (DeadEnemies >= AllEnemiesCount && _bossDefeated)
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
