using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    void Update()
    {
        MovementWall();
    }

    void MovementWall()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            speed = 0;
        }
    }
}
