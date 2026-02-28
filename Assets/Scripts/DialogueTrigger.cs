using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public int _DialogueIndex;

    private void OnTriggerEnter(Collider other)
    {
        DialogsController.Instance.PlayDialog(_DialogueIndex);
    }
}
