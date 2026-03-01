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
    int _numOfPieces = 0;

    public Farola[] nextFarolas;
    public Farola[] lastFarolas;
    bool IsFinished = false;
    public AudioClip wood;
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

        if (_numOfPieces == _positionPoints.Count && IsFinished == false)
        {
            SetFarolas();
        }
    }

    void SetFarolas()
    {
        IsFinished = true;
        Debug.Log("Farolaaaaaas");
        foreach (var f in nextFarolas)
        {
            Debug.Log("Encender");
            f.SetLights(true);
        }
        foreach (var b in lastFarolas)
        {
            Debug.Log("Apagar");
            b.SetLights(false);
        }
    }

    private void AttachPickableObject()
    {
        if (_pickableObject.transform.parent == _target.transform)
        {
            _pickableObject.transform.parent = transform;
        }

        if(_pickableObject.tag == "WoodPiece")
        {
            if (_pickableObject.GetComponent<PickUp>().GetNameOfPickableObject() > 1)
            {
                _pickableObject.transform.localRotation = Quaternion.Euler(Vector3.right.x, Vector3.up.y, -1 * transform.localRotation.eulerAngles.z);
                Debug.Log(_pickableObject.GetComponent<PickUp>().GetNameOfPickableObject());
            }
            else
                _pickableObject.transform.localRotation = Quaternion.Euler(Vector3.left.x * 180, 0, -1 * transform.localRotation.eulerAngles.z);
        }
        else
        {
            if(_pickableObject.GetComponent<PickUp>().GetNameOfPickableObject() == 4)
                _pickableObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            else
                _pickableObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }

        _pickableObject.transform.position += (_positionPoints[_numOfPositionInList].position - _pickableObject.transform.position).normalized * _speed * Time.deltaTime;

        if (_distanceToStop > (_positionPoints[_numOfPositionInList].position - _pickableObject.transform.position).magnitude)
        {
            _pickableObject.transform.position = _positionPoints[_numOfPositionInList].position;
            _canAttach = false;
            _pickableObject.GetComponent<PickUp>().SetPickableObject(PickableObject.OnSite);
            ++_numOfPieces;

            if(_positionPoints[_numOfPositionInList].GetComponent<MeshRenderer>())
                _positionPoints[_numOfPositionInList].GetComponent<MeshRenderer>().enabled = false;

            if (_pickableObject.tag == "WoodPiece")
            {
                GetComponent<AudioSource>().PlayOneShot(wood);
            }
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
