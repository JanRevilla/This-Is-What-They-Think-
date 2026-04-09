using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PickableObject
{
    OnGround = 0,
    OnHands,
    OnSite
}

public enum NameOfPickableObject
{
    Humano = 0,
    Coche,
    Paraguas,
    Pato,
    Pinguino1 = 0,
    Pinguino2 = 1,
    Pinguino3 = 2,
    Pinguino4 = 3,
    Pinguino5 = 4,
    Pinguino6 = 5
}

public class PickUp : MonoBehaviour
{
    bool _canMove = false;
    bool _itsFalling = false;
    GameObject _target;
    PickableObject _pickableObject = 0;

    [SerializeField]
    private float _speed = 1.0f;

    [SerializeField]
    private float _distanceToStop = 0.25f;

    [SerializeField]
    private NameOfPickableObject _numOfPiece;

    void Start()
    {
        _target = GameObject.FindWithTag("Pick Pivot");
    }

    private void LateUpdate()
    {
        if (_canMove)
            MoveToTarget();
        else
        {
            if (CollectorOfPickables.CANFALLING)
            {
                if (_pickableObject == PickableObject.OnHands && Input.GetKeyDown(KeyCode.Mouse0))
                    ObjectPenetration();

                if (_itsFalling)
                    MoveToGround();
            }
        }
    }

    void PickUpObject()
    {
        if (GetComponent<MeshCollider>().enabled)
        {
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
        }

        _canMove = true;
    }

    void MoveToTarget()
    {
        transform.position += (_target.transform.position - transform.position).normalized * _speed * Time.deltaTime;

        if(_distanceToStop > (_target.transform.position - transform.position).magnitude)
        {
            _canMove = false;
            transform.SetParent(_target.transform);
            _pickableObject = PickableObject.OnHands;
        }
    }

    void MoveToGround()
    {
        if(transform.parent != null)
            transform.SetParent(null);

        if (gameObject.scene.name == "DontDestroyOnLoad")
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _distanceToStop))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                _pickableObject = PickableObject.OnGround;
                _itsFalling = false;
                GetComponent<MeshCollider>().enabled = true;
                GetComponent<SphereCollider>().enabled = true;
                return;
            }
        }
        
        transform.position += Vector3.down * _speed * Time.deltaTime;
    }

    private void ObjectPenetration()
    {
        Vector3 rayOrigin = Vector3.zero;

        for(int i = 0; i < _target.transform.parent.childCount; i++)
        {
            if(_target.transform.parent.GetChild(i) != _target.transform)
            {
                rayOrigin = _target.transform.parent.GetChild(i).GetChild(0).position;
                break;
            }
        }

        Vector3 dir = transform.position - rayOrigin;
        float distance = dir.magnitude;
        dir.Normalize();

        if(Physics.Raycast(rayOrigin, dir, out RaycastHit hit, distance))
        {
            transform.position = hit.point + (hit.normal * 0.25f);
        }

        _itsFalling = true;
    }

    public PickableObject GetPickableObject() {  return _pickableObject; }

    public void SetPickableObject(PickableObject value) {  _pickableObject = value; }

    public int GetNameOfPickableObject() { return (int)_numOfPiece; }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.Mouse0) && _pickableObject == PickableObject.OnGround)
        {
            if(_target.transform.childCount == 0)
            {
                PickUpObject();
            }
        }
    }
}
