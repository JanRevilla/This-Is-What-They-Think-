using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube[] m_cubeTypes;
    //[SerializeField] private GameObject m_standardCube;
    //[SerializeField] private GameObject m_bonusCube;

    //[SerializeField] private int m_maxStandardCubes = 8;
    //[SerializeField] private int m_maxBonusCubes = 3;

    //[SerializeField] private int m_startingStandardCubes = 5;
    //[SerializeField] private int m_startingBonusCubes = 3;

    //[SerializeField] private int m_standardPoints = 2;
    //[SerializeField] private int m_bonusPoints = 6;

    //[SerializeField] private float m_standardCooldown = 5.0f;
    //[SerializeField] private float m_bonusCooldown = 10.0f;

    //[SerializeField] private float m_standardRadius = 10.0f;
    //[SerializeField] private float m_bonusRadius = 5.0f;

    //[SerializeField] private float m_spawnHeight = 10.0f;

    //private int m_standardCubeCounter;
    //private int m_bonusCubeCounter;

    //private float m_standardTimer;
    //private float m_bonusTimer;

    private List<GameObject> m_cubeList;

    private void Start()
    {
        m_cubeList = new List<GameObject>();

        foreach (Cube cubeType in m_cubeTypes)
        {
            cubeType.cubeType.spawnTimer = 0.0f;
            cubeType.cubeType.cubeCounter = 0;

            for (int i = 0; i < cubeType.cubeType.initialCubes; i++)
            {
                SpawnCube(cubeType);
                cubeType.cubeType.spawnTimer = cubeType.cubeType.spawnCooldown;
            }
        }
        //for (int i = 0; i < m_startingStandardCubes; i++)
        //{
        //    SpawnStandard();
        //}
        //for (int i = 0; i < m_startingBonusCubes; i++)
        //{
        //    SpawnBonus();
        //}


        //m_standardTimer = m_standardCooldown;
        //m_bonusTimer = m_bonusCooldown;
    }
    private void Update()
    {
        //if (m_standardCubeCounter < m_maxStandardCubes)
        //{
        //    m_standardTimer -= Time.deltaTime; m_standardTimer = m_standardTimer < 0.0f ? 0.0f : m_standardTimer;
        //}
        //if (m_bonusCubeCounter < m_maxBonusCubes)
        //{
        //    m_bonusTimer -= Time.deltaTime; m_bonusTimer = m_bonusTimer < 0.0f ? 0.0f : m_bonusTimer;
        //}

        foreach (Cube cubeType in m_cubeTypes)
        {
            if (cubeType.cubeType.cubeCounter < cubeType.cubeType.maxCubes)
            {
                cubeType.cubeType.spawnTimer -= Time.deltaTime; cubeType.cubeType.spawnTimer = cubeType.cubeType.spawnTimer < 0.0f ? 0.0f : cubeType.cubeType.spawnTimer;
            }
            if (cubeType.cubeType.spawnTimer == 0.0f)
            {
                SpawnCube(cubeType);
            }
        }

        //if (m_standardTimer == 0.0f)
        //{
        //    SpawnStandard();
        //}
        //if (m_bonusTimer == 0.0f)
        //{
        //    SpawnBonus();
        //}
    }

    private void SpawnCube(Cube cubeType)
    {
        GameObject l_cube = GameObject.Instantiate(cubeType.gameObject);
        l_cube.transform.position = Vector3.up * cubeType.cubeType.spawnHeight + (Vector3.right * Random.Range(-1.0f, 1.0f) + Vector3.forward * Random.Range(-1.0f, 1.0f)).normalized * cubeType.cubeType.spawnRadius * Random.value;
        m_cubeList.Add(l_cube);

        cubeType.cubeType.cubeCounter++;
        cubeType.cubeType.spawnTimer = cubeType.cubeType.spawnCooldown;
    }
    //private void SpawnStandard()
    //{
    //    GameObject l_cube = GameObject.Instantiate(m_standardCube);
    //    l_cube.transform.position = Vector3.up * m_spawnHeight + (Vector3.right * Random.Range(-1.0f, 1.0f) + Vector3.forward * Random.Range(-1.0f, 1.0f)).normalized * m_standardRadius * Random.value;
    //    m_cubeList.Add(l_cube);
    //    l_cube.SetActive(true);

    //    m_standardCubeCounter++;
    //    m_standardTimer = m_standardCooldown;
    //    //Debug.Log("Standard Cubes " + m_standardCubeCounter);
    //    //Debug.Log(l_cube.transform.position);
    //}
    //private void SpawnBonus()
    //{
    //    GameObject l_cube = GameObject.Instantiate(m_bonusCube);
    //    l_cube.transform.position = Vector3.up * m_spawnHeight + (Vector3.right * Random.Range(-1.0f, 1.0f) + Vector3.forward * Random.Range(-1.0f, 1.0f)).normalized * m_bonusRadius * Random.value;
    //    m_cubeList.Add(l_cube);
    //    l_cube.SetActive(true);

    //    m_bonusCubeCounter++;
    //    m_bonusTimer = m_bonusCooldown;
    //    //Debug.Log("Bonus Cubes " + m_bonusCubeCounter);
    //    //Debug.Log(l_cube.transform.position);
    //}
    public void TakeCube(CubeBase cubeType, GameObject cube)
    {
        //if (cube.CompareTag("BonusCube"))
        //{
        //    cube.Type--;
        //}
        //if (cube.CompareTag("StandardCube"))
        //{
        //    m_standardCubeCounter--;
        //}
        cubeType.cubeCounter--;

        cube.SetActive(false);
        m_cubeList.Remove(cube);
    }
    private void OnDrawGizmos()
    {
        foreach (Cube cubeType in m_cubeTypes)
        {
            if (cubeType != null)
            Gizmos.DrawWireSphere(Vector3.zero, cubeType.cubeType.spawnRadius);
        }
        //Gizmos.DrawWireSphere(Vector3.zero, m_standardRadius);
        //Gizmos.DrawWireSphere(Vector3.zero, m_bonusRadius);
    }
}
