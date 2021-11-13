using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] private int _repeatCount;
    [SerializeField] private int _distanceBetweenFullLine;
    [SerializeField] private int _distanceBetweenRandomLine;

    [Header("Block")]
    [SerializeField] private Transform _blockContainer;
    [SerializeField] private Block _blockTemplate;
    [SerializeField] private int _blockSpawnChance;

    [Header("Bonus")]
    [SerializeField] private Bonus _bonusTemplate;
    [SerializeField] private Transform _bonusContainer;
    [SerializeField] private int _bonusSpawnChance;

    [Header("Wall")]
    [SerializeField] private Wall _wallTemplate;
    [SerializeField] private int _wallSpawnChance;
    [SerializeField] private Transform _wallContainer;
    [SerializeField] private int _distanceBetweenWalls;
    [SerializeField] private int _wallAdditionalScaleY;

    [Header("Obstacle")]
    [SerializeField] private Obstacle _obstacleTemplate;
    [SerializeField] private int _obstacleSpawnChance;
    [SerializeField] private Transform _obstacleContainer;
    [SerializeField] private float _obstacleOffsetY;

    private BlockSpawnPoint[] _blockSpawnPoints;
    private WallSpawnPoint[] _wallSpawnPoints;
    private BonusSpawnPoint[] _bonusSpawnPoints;
    private ObstacleSpawnPoint[] _obstacleSpawnPoints;
    private SideWallSpawnPoint[] _sideWallSpawnPoints;

    private void Start()
    {
        _blockSpawnPoints = GetComponentsInChildren<BlockSpawnPoint>();
        _bonusSpawnPoints = GetComponentsInChildren<BonusSpawnPoint>();
        _wallSpawnPoints = GetComponentsInChildren<WallSpawnPoint>();
        _obstacleSpawnPoints = GetComponentsInChildren<ObstacleSpawnPoint>();
        _sideWallSpawnPoints = GetComponentsInChildren<SideWallSpawnPoint>();

        for (int i = 0; i < _repeatCount; i++)
        {
            MoveSpawner(_distanceBetweenWalls);
            GenerateFullLine(_sideWallSpawnPoints, _wallTemplate.gameObject, _wallContainer,_wallTemplate.transform.localScale.y + _wallAdditionalScaleY);

            MoveSpawner(_distanceBetweenFullLine);
            GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance, _wallContainer, _wallTemplate.transform.localScale.y, _distanceBetweenFullLine / 2f);
            GenerateRandomElements(_obstacleSpawnPoints, _obstacleTemplate.gameObject, _obstacleSpawnChance, _obstacleContainer, _obstacleTemplate.transform.localScale.y, _distanceBetweenFullLine - _obstacleOffsetY);
            GenerateFullLine(_blockSpawnPoints, _blockTemplate.gameObject, _blockContainer, _blockTemplate.transform.localScale.y);

            MoveSpawner(_distanceBetweenRandomLine);
            GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance, _wallContainer, _wallTemplate.transform.localScale.y, _distanceBetweenRandomLine / 2f);
            GenerateRandomElements(_blockSpawnPoints, _blockTemplate.gameObject, _blockSpawnChance, _blockContainer, _blockTemplate.transform.localScale.y);
            GenerateRandomElements(_bonusSpawnPoints, _bonusTemplate.gameObject, _bonusSpawnChance, _bonusContainer, _bonusTemplate.transform.localScale.y, _distanceBetweenRandomLine);
        }
    }

    private void GenerateFullLine(SpawnPoint[] spawnPoints, GameObject generatedElement, Transform container = null, float scaleY = 1f, float offsetY = 0)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject element = GenerateElement(spawnPoints[i].transform.position, generatedElement, container, offsetY);
            element.transform.localScale = new Vector3(element.transform.localScale.x, scaleY,
                          element.transform.localScale.z);
        }
    }

    private void GenerateRandomElements(SpawnPoint[] spawnPoints, GameObject generatedElement, int spawnChance,
       Transform container = null, float scaleY = 1f, float offsetY = 0)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Random.Range(0, 100) < spawnChance)
            {
                GameObject element = GenerateElement(spawnPoints[i].transform.position, generatedElement, container, offsetY);
                element.transform.localScale = new Vector3(element.transform.localScale.x, scaleY,
                    element.transform.localScale.z);
            }  
        }
    }

    private GameObject GenerateElement(Vector3 spawnPoint, GameObject generatedElement, Transform container = null, float offsetY = 0)
    {
        spawnPoint.y -= offsetY;
        return Instantiate(generatedElement, spawnPoint, Quaternion.identity, container);
    }

    private void MoveSpawner(int distanceY)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + distanceY, transform.position.z);
    }
}
