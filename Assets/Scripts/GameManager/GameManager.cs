using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour, IGameManager
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<GameObject> defenders;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject defenderPrefab;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private Transform[] defenderSpawnPoints;
    public Waypoint enemyWaypoint;
    public Waypoint defenderWaypoint;
    [SerializeField] private float enemyWaveDelay = 15f;
    [SerializeField] private float enemySpawnDelay = 1f;
    [SerializeField] private int enemiesPerWave = 4;
    [SerializeField] private int totalWaves = 3;
    [SerializeField] private float defenderSpawnDelay = 5f;
    [SerializeField] private TextMeshProUGUI defenderSpawnTimerText;
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private GameObject defeatCanvas;
    private bool canSpawnDefender = true;
    private float defenderSpawnTimer = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(SpawnEnemyWaves());
        victoryCanvas.gameObject.SetActive(false);
        defeatCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSpawnDefender)
        {
            StartCoroutine(SpawnDefender());
        }

        if (!canSpawnDefender)
        {
            defenderSpawnTimer -= Time.deltaTime;
            defenderSpawnTimerText.text = $"Next defender in: {Mathf.Ceil(defenderSpawnTimer)}s";
        }
        else
        {
            defenderSpawnTimerText.text = "Click to spawn defender";
        }
    }

    private IEnumerator SpawnEnemyWaves()
    {
        for (int wave = 0; wave < totalWaves; wave++)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnUnit(enemyPrefab, enemySpawnPoints, enemies, enemyWaypoint);
                yield return new WaitForSeconds(enemySpawnDelay);
            }
            yield return new WaitForSeconds(enemyWaveDelay);
        }
    }

    private IEnumerator SpawnDefender()
    {
        canSpawnDefender = false;
        defenderSpawnTimer = defenderSpawnDelay;
        SpawnUnit(defenderPrefab, defenderSpawnPoints, defenders, defenderWaypoint);
        yield return new WaitForSeconds(defenderSpawnDelay);
        canSpawnDefender = true;
    }

    private void SpawnUnit(GameObject prefab, Transform[] spawnPoints, List<GameObject> unitList, Waypoint waypoint, Vector3? position = null)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 spawnPosition = position ?? spawnPoint.position;
        GameObject unit = Instantiate(prefab, spawnPosition, spawnPoint.rotation);
        Unit unitComponent = unit.GetComponent<Unit>();
        if (unitComponent != null)
        {
            unitComponent.waypoint = waypoint;
        }
        unitList.Add(unit);
    }

    public void EndGame(bool isVictory)
    {
        Time.timeScale = 0;
        canSpawnDefender = false;
        if (isVictory)
        {
            victoryCanvas.gameObject.SetActive(true);
        }
        else
        {
            defeatCanvas.gameObject.SetActive(true);
        }
    }
}
