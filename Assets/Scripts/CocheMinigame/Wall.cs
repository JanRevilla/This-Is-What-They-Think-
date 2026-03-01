using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private float speed = 15;
    private Transform _fakeFade;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>()._fakeFade.parent.GetComponent<CameraController>().enabled = false;
            if (other.GetComponent<PlayerController>()._fakeFade.localScale.magnitude < 5)
                other.GetComponent<PlayerController>()._fakeFade.localScale += new Vector3(0.25f, 0.25f, 0);
        }
    }
}
