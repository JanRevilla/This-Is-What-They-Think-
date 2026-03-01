using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPeople : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _Positions = new();

    [SerializeField]
    private float _Speed = 5.0f;

    [SerializeField]
    private float _DistanceToChangeDirection = 0.25f;

    [SerializeField]
    private int _MaxPeopleInScene = 10;
    private int _PeopleInScene = 0;

    [SerializeField]
    private float _TimeToSpawn = 1.25f;
    private float _Counter = 0;

    [SerializeField]
    private PoolElements _Pool;

    [SerializeField]
    private GameObject _SpawnObject;

    [SerializeField]
    private int maxPuntuation = 10;
    private int actualPuntuation = 0;

    [SerializeField]
    private GameObject wall;

    void Start()
    {
        _Pool = new PoolElements(15, _SpawnObject, _Pool.transform);
        _Pool.SetSpawnPositionComponentInPool(this);
        _Pool.PutComponentInPool(_Positions, _Speed, _DistanceToChangeDirection);
    }


    void Update()
    {
        if (_PeopleInScene < _MaxPeopleInScene)
        {
            TimeToSpawn();
        }
    }

    private void Spawn()
    {
        _Pool.GetNextElement().SetActive(true);
        _PeopleInScene++;
    }

    private void TimeToSpawn()
    {
        if(_Counter >= _TimeToSpawn)
        {
            _Counter = 0;
            Spawn();
        }
        _Counter += Time.deltaTime;
    }

    public Vector3 RandomFirstPos()
    {
        float rnd = Random.Range(-(GetComponent<BoxCollider>().size.x / 2), GetComponent<BoxCollider>().size.x / 2);
        return new Vector3(_Positions[0].position.x + rnd, _Positions[0].position.y, _Positions[0].position.z);
    }

    private void CheckPuntuation()
    {
        if (actualPuntuation == maxPuntuation)
            GameOver();
    }

    public void IncreaseActualPuntuation()
    {
        ++actualPuntuation;
        Debug.Log(actualPuntuation);

        CheckPuntuation();
    }

    public void DecreasePeopleInScene()
    {
        _PeopleInScene--;
    }

    private void GameOver()
    {
        Debug.Log("Entra");
        _MaxPeopleInScene = 0;
        wall.SetActive(true);
    }
}
