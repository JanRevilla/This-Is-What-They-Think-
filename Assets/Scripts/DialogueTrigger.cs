using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public int _DialogueIndex;
    private bool _TriggeredAudio = false;

    public bool _IsTimeUpAudio = false; 

    [Header("Segundo Diálogo (Opcional)")]
    public bool _HasSecondDialogue = false;
    public int _SecondDialogueIndex;
    public float _DelayBetweenDialogs = 7.0f;

    public DoorBehaviour _Door;

    private void OnTriggerEnter(Collider other)
    {
        if (!_TriggeredAudio)
        {
            DialogsController.Instance.PlayDialog(_DialogueIndex);
            _TriggeredAudio = true;

            if (_HasSecondDialogue)
            {
                StartCoroutine(SecondDialog());
            }
            if (_IsTimeUpAudio)
            {
                StartCoroutine(OpenDoor());
            }
        }
    }

    IEnumerator SecondDialog()
    {
        float l_Duration = DialogsController.Instance.m_AudioDialogs[_DialogueIndex].length;
        yield return new WaitForSeconds(l_Duration + _DelayBetweenDialogs);
        DialogsController.Instance.PlayDialog(_SecondDialogueIndex);
    }
    IEnumerator OpenDoor()
    {
        float l_Duration = DialogsController.Instance.m_AudioDialogs[_DialogueIndex].length;
        yield return new WaitForSeconds(l_Duration);
        _Door.ClosedDoor = false;

    }
}
