using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
            StartCoroutine(AudioCloseDoor());
            //audioSource.PlayOneShot(CloseAudio);
        }
    }

    IEnumerator AudioCloseDoor()
    {
        AnimationState state = animation["CloseDoor"];

        if (state != null)
        {
            yield return new WaitForSeconds(state.length);

            audioSource.PlayOneShot(CloseAudio);
            Debug.Log("Puerta cerrada: Sonando audio de impacto.");
        }
    }
}
