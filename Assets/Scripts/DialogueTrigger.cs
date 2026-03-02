using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public int _DialogueIndex;
    private bool _TriggeredAudio = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_TriggeredAudio)
        {
            DialogsController.Instance.PlayDialog(_DialogueIndex);
            _TriggeredAudio = true;
        }
    }
}
