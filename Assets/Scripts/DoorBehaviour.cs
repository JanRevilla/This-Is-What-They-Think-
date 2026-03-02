using UnityEditor;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    public new Animation animation;
    BoxCollider boxCollider;
    public AudioClip OpenAudio;
    public AudioClip CloseAudio;
    AudioSource audioSource;
    public bool ClosedDoor ;
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
        if (!ClosedDoor)
        {
            animation.PlayQueued("OpenDoor", QueueMode.CompleteOthers);
            IsOpen = true;
            audioSource.PlayOneShot(OpenAudio);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!ClosedDoor)
        {
            animation.PlayQueued("CloseDoor", QueueMode.CompleteOthers);
            IsOpen = false;
            audioSource.PlayOneShot(CloseAudio);
        }
    }
}
