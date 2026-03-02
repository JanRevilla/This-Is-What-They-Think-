using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public int _DialogueIndex;
    private bool _TriggeredAudio = false;

    [Header("Segundo Diálogo (Opcional)")]
    public bool _HasSecondDialogue = false;
    public int _SecondDialogueIndex;
    public float _DelayBetweenDialogs = 7.0f;

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
        }
    }

    IEnumerator SecondDialog()
    {
        float l_Duration = DialogsController.Instance.m_AudioDialogs[_DialogueIndex].length;
        yield return new WaitForSeconds(l_Duration + _DelayBetweenDialogs);
        DialogsController.Instance.PlayDialog(_SecondDialogueIndex);
    }
}
