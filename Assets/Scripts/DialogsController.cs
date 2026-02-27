using UnityEngine;

public class DialogsController : MonoBehaviour
{
    public static DialogsController Instance { get; private set; }

    [Header("Audio")]
    public AudioClip[] m_AudioDialogs;
    public AudioSource m_audioSource;
    public AudioClip[] m_RandomAudioDialogs;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
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

    public void PlayRandomDialog()
    {
        int indice = Random.Range(0, m_RandomAudioDialogs.Length);
        m_audioSource.clip = m_RandomAudioDialogs[indice];

        m_audioSource.PlayOneShot(m_audioSource.clip);
    }


}
