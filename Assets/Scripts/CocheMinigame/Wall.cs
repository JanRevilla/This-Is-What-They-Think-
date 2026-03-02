using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private float speed = 15;

    public AudioClip _audioClip;
    AudioSource _audioSource;

    bool activeFade = false;
    void Update()
    {
        MovementWall();
        _audioSource = GetComponent<AudioSource>();
    }

    void MovementWall()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(PlayAudio());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (activeFade)
            {
                other.GetComponent<PlayerController>()._fakeFade.parent.GetComponent<CameraController>().enabled = false;
                if (other.GetComponent<PlayerController>()._fakeFade.localScale.magnitude < 5)
                    other.GetComponent<PlayerController>()._fakeFade.localScale += new Vector3(0.25f, 0.25f, 0);
                else
                    transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    IEnumerator PlayAudio()
    {
        _audioSource.clip = _audioClip;
        _audioSource.Play();

        yield return new WaitUntil(() => _audioSource.time >= (_audioClip.length / 5));

        speed = 0;
        activeFade = true;
    }
}
