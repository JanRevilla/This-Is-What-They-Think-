using UnityEditor;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    new Animation animation;
    BoxCollider boxCollider;
    public AudioClip OpenAudio;
    public AudioClip CloseAudio;
    AudioSource audioSource;
    bool IsOpen;
    void Start()
    {
        animation = GetComponent<Animation>();
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    //void Update()
    //{
    //    if (IsOpen)
    //    {
    //        animation.PlayQueued("Opened", QueueMode.CompleteOthers);
    //    }
    //    else
    //    {
    //        animation.PlayQueued("Closed", QueueMode.CompleteOthers);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        animation.PlayQueued("OpenDoor", QueueMode.CompleteOthers);
        IsOpen = true;
        audioSource.PlayOneShot(OpenAudio);
    }
    private void OnTriggerExit(Collider other)
    {
        animation.PlayQueued("CloseDoor", QueueMode.CompleteOthers);
        IsOpen=false;
        audioSource.PlayOneShot(CloseAudio);
    }
}
