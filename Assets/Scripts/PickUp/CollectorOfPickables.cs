using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectorOfPickables : MonoBehaviour
{
    GameObject _target, _pickableObject;
    bool _canAttach = false;
    private static bool _canFalling = true;
    public static bool CANFALLING => _canFalling;

    [SerializeField]
    private float _speed = 2.75f;

    [SerializeField]
    private float _distanceToStop = 0.05f;

    [SerializeField]
    private List<Transform> _positionPoints = new();

    int _numOfPositionInList = 0;

    void Start()
    {
        _target = GameObject.FindWithTag("Pick Pivot");
    }

    void Update()
    {
        if (_canAttach)
        {
            AttachPickableObject();
        }
    }

    private void AttachPickableObject()
    {
        if (_pickableObject.transform.parent == _target.transform)
        {
            _pickableObject.transform.parent =(transform);
            Debug.Log("Entra");
        }

        _pickableObject.transform.position += (_positionPoints[_numOfPositionInList].position - _pickableObject.transform.position).normalized * _speed * Time.deltaTime;

        if (_distanceToStop > (_positionPoints[_numOfPositionInList].position - _pickableObject.transform.position).magnitude)
        {
            _pickableObject.transform.position = _positionPoints[_numOfPositionInList].position;
            _canAttach = false;
            _pickableObject.GetComponent<PickUp>().SetPickableObject(PickableObject.OnSite);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            _canFalling = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && _target.transform.childCount > 0)
        {
            if (_target.transform.GetChild(0).GetComponent<PickUp>().GetPickableObject() == PickableObject.OnHands && Input.GetKey(KeyCode.Mouse0))
            {
                _canAttach = true;
                _pickableObject = _target.transform.GetChild(0).gameObject;
                _numOfPositionInList = _pickableObject.GetComponent<PickUp>().GetNameOfPickableObject();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            _canFalling = true;
    }
}
