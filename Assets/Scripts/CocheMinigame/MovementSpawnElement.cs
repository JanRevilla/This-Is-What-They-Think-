using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpawnElement : MonoBehaviour
{
    List<Transform> _Positions = new();
    float _Speed = 0;
    Vector3 _SpawnPosition = Vector3.zero;
    Vector3 _Direction = Vector3.zero;
    int _SpawnIndex = 0;
    float _DistanceToChangeDirection = 0;
    int randomPose = 0;

    private void Start()
    {
        SetMannequinPose();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += _Direction * _Speed * Time.deltaTime;

        if((transform.position - new Vector3(transform.position.x, _Positions[_SpawnIndex].position.y, _Positions[_SpawnIndex].position.z)).magnitude <= _DistanceToChangeDirection)
        {
            if (_SpawnIndex <= _Positions.Count - 2)
                _Direction = Direction();
            
            _SpawnIndex++;

            if (_SpawnIndex >= _Positions.Count)
                FinishPath();
        }
    }

    private Vector3 Direction()
    {
        Vector3 _NextPosition = Vector3.zero;
        _NextPosition = new Vector3(_SpawnPosition.x, _SpawnPosition.y, _Positions[_SpawnIndex + 1].position.z);

        if (_SpawnIndex == 0)
            return (_NextPosition - _SpawnPosition).normalized;
        else
            return (_NextPosition - transform.position).normalized;
    }

    public void SetSpawnPosition(Vector3 _pos)
    {
        _SpawnPosition = _pos;
    }

    private void SetMannequinPose()
    {
        randomPose = Random.Range(0, transform.childCount);

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.GetChild(randomPose).gameObject.SetActive(true);
    }

    private void SetVariables()
    {
        SetMannequinPose();
        transform.position = _SpawnPosition;
        _Direction = Direction();
        _SpawnIndex++;
    }

    public void Init(List<Transform> l_Pos, float l_Speed, float l_DistanceToChangeDirection)
    {
        _Positions = l_Pos;
        _Speed = l_Speed;
        _DistanceToChangeDirection = l_DistanceToChangeDirection;
        SetVariables();
    }

    private void FinishPath()
    {
        transform.parent.parent.GetComponent<SpawnPeople>().DecreasePeopleInScene();
        _SpawnPosition = transform.parent.parent.GetComponent<SpawnPeople>().RandomFirstPos();
        _SpawnIndex = 0;
        SetVariables();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent.parent.GetComponent<SpawnPeople>().IncreaseActualPuntuation();
            FinishPath();
        }
    }
}
