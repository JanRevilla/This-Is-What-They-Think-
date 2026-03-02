using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        animator.Play("FadeIn", -1, 0f);
    }

    public void PlayFadeOut()
    {
        animator.SetTrigger("FadeOut");
    }
}
