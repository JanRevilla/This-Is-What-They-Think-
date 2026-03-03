using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DialogsController : MonoBehaviour
{
    public static DialogsController Instance { get; private set; }

    [Header("Audio")]
    public AudioClip[] m_AudioDialogs;
    public AudioSource m_audioSource;
    public AudioClip[] m_RandomAudioDialogs;
    public AudioClip m_FinalDialog;
    private int _indexArrayAudios;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        _indexArrayAudios = 0;

        DontDestroyOnLoad(gameObject);
    }
    public bool IsPlaying()
    {
        return m_audioSource.isPlaying;
    }


    public void PlayDialog(int dialogIndex)
    {
        if (dialogIndex < 0 || dialogIndex >= m_AudioDialogs.Length)
        {
            Debug.LogWarning($"El índice de diálogo {dialogIndex} no existe en el array.");
            return;
        }
        m_audioSource.Stop();

        m_audioSource.clip = m_AudioDialogs[dialogIndex];
        m_audioSource.Play();
    }

    public void PlayNotRandomDialog()
    {
        m_audioSource.clip = m_RandomAudioDialogs[_indexArrayAudios];
        m_audioSource.PlayOneShot(m_audioSource.clip);
        Debug.Log("SONAAAR");
        _indexArrayAudios++;
    }
    public void PlayFinalAudio()
    {
        m_audioSource.PlayOneShot(m_FinalDialog);
    }
}
