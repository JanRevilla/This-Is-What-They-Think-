using UnityEngine;
using UnityEngine.Rendering;

public class CarSoundController : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip loopClip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = loopClip;
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
    }

    void OnEnable()
    {
        if (loopClip != null)
        {
            _audioSource.Play();
        }
    }

    public void StopLoopEngine()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    //OPCIONAL
    public void StartLoop()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
