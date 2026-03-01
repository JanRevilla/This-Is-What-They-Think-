using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EyesAnim : MonoBehaviour
{
    [SerializeField] private GameObject[] frames;
    [SerializeField] private float delayBetweenBlinks;
    [SerializeField] private float samples;

    private float elapsedTimeFrames = 0f;
    private float elapsedTimeAnimation = 0f;
    private int frameIndex = 0;
    bool playingAnimation = false;

    void Start()
    {
        foreach (var frame in frames)
        {
            frame.SetActive(false);
        }

        frames[0].SetActive(true);

    }

    void Update()
    {
        if (elapsedTimeAnimation >= delayBetweenBlinks && !playingAnimation)
        {
            playingAnimation = true;
        }

        PlayAnimation();


        elapsedTimeAnimation += Time.deltaTime;
        elapsedTimeFrames += Time.deltaTime;
    }

    private void PlayAnimation()
    {
        if (!playingAnimation) return;

        if (elapsedTimeFrames >= samples)
        {
            frameIndex++;

            if (frameIndex > frames.Length - 1)
            {
                frameIndex = 0;
                playingAnimation = false;
                elapsedTimeAnimation = 0;

                frames[frameIndex].SetActive(true);
                frames[frames.Length - 1].SetActive(false);
            }
            else
            {
                frames[frameIndex].SetActive(true);
                frames[frameIndex - 1].SetActive(false);
            }
            elapsedTimeFrames = 0f;
        }
    }
}
