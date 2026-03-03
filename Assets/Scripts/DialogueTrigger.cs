using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public int _DialogueIndex;
    private bool _TriggeredAudio = false;

    public bool _IsTimeUpAudio = false;
    public DoorBehaviour _Door;

    [Header("Diálogo Esperando (Opcional)")]
    public bool _TimedDialogue = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!_TriggeredAudio)
        {
            if (!_TimedDialogue)
            {
                DialogsController.Instance.PlayDialog(_DialogueIndex);
                _TriggeredAudio = true;
            }
            else
            {
                StartCoroutine(TimedDialog());
            }

            if (_IsTimeUpAudio)
            {
                StartCoroutine(OpenDoor());
            }
        }
    }

    IEnumerator TimedDialog()
    {
        while (DialogsController.Instance.IsPlaying())
        {
            yield return null;
        }
        Debug.Log("ELAudioSEHACEPLAY");

        yield return new WaitForSeconds(0.5f);

        DialogsController.Instance.PlayDialog(_DialogueIndex);

        _TriggeredAudio = true;
    }
    IEnumerator OpenDoor()
    {
        float l_Duration = DialogsController.Instance.m_AudioDialogs[_DialogueIndex].length;
        yield return new WaitForSeconds(l_Duration);
        _Door.ClosedDoor = false;

    }
}
