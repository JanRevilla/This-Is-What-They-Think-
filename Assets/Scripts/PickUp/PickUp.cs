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
    Piece_1 = 0,
    Piece_2,
    Piece_3,
    Piece_4,
    Piece_5
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
                if (_pickableObject == PickableObject.OnHands && Input.GetKey(KeyCode.Mouse0))
                    _itsFalling = true;

                if (_itsFalling)
                    MoveToGround();
            }
        }
    }

    void PickUpObject()
    {
        if (GetComponent<Collider>().enabled)
        {
            GetComponent<Collider>().enabled = false;
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

        transform.position += Vector3.down * _speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _distanceToStop))
        {
            if (hit.collider.tag == "Ground")
            {
                _pickableObject = PickableObject.OnGround;
                _itsFalling = false;
                GetComponent<Collider>().enabled = true;
            }
        }
    }

    public PickableObject GetPickableObject() {  return _pickableObject; }

    public void SetPickableObject(PickableObject value) {  _pickableObject = value; }

    public int GetNameOfPickableObject() { return (int)_numOfPiece; }

    private void OnTriggerStay(Collider other)
    {
        Debug.DrawRay(transform.position, _target.transform.position - transform.position, Color.red);
        if (other.tag == "Player" && Input.GetKey(KeyCode.Mouse0) && _pickableObject == PickableObject.OnGround)
        {
            if(_target.transform.childCount == 0)
            {
                if (!Physics.Raycast(transform.position, _target.transform.position - transform.position, out RaycastHit hit))
                {
                    if (hit.collider == null)
                        PickUpObject();
                }
            }
        }
    }
}
