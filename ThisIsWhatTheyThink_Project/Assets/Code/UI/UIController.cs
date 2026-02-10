using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    //public PlayerData playerData;

    [Header("Player Info")]
    public int playerID; //cambiar cuando tenga el playerData

    [Header("Scores")]
    public int bagScore = 0; //cambiar cuando tenga el playerData
    public int baseScore = 0; //cambiar cuando tenga el playerData

    [Header("State & Stun")]
    public string playerState = "Normal";
    public float stunTimer = 0f;
    public float maxStunTime = 3f;

    [Header("Stun Effects")]
    float pulseSpeed = 4f;
    float pulseAmount = 0.5f;

    [Header("Test Keys (Solo Prototipo)")]
    public KeyCode addKey = KeyCode.KeypadPlus;  
    public KeyCode removeKey = KeyCode.KeypadMinus;
    public KeyCode storeKey = KeyCode.Space; 
    public KeyCode stunKey = KeyCode.S; 

    [Header("UI Elements")]
    public TMP_Text bagText;
    public TMP_Text basetext;
    public GameObject stunIcon;
    public Image stunImage;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(addKey))
        {
           bagScore++;
        }

        if(Input.GetKeyDown(removeKey))
        {
            if(bagScore > 0)
                bagScore--;
        }

        if(Input.GetKeyDown(stunKey))
        {
            playerState = "Stunned";
            stunTimer = maxStunTime;
        }

        if (Input.GetKeyDown("0"))
        {
            bagScore = 0;
            baseScore = 0;
        }

        if (Input.GetKeyDown(storeKey))
        {
            baseScore += bagScore;
            bagScore = 0;
        }

        if (playerState == "Stunned")
        {
            stunIcon.SetActive(true);
            stunTimer -= Time.deltaTime;

            if (stunImage != null)
            {
                stunImage.fillAmount = stunTimer / maxStunTime;
            }

            float baseScale = 1f;
            float currentScale = baseScale + Mathf.PingPong(Time.time * pulseSpeed, pulseAmount); 
            stunIcon.transform.localScale = new Vector3(currentScale, currentScale, 1f);

            if (stunTimer <= 0)
            {
                EndStun();
            }
        }
        else
        {
            if (stunIcon.activeSelf) stunIcon.SetActive(false);
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        bagText.text = "Bag Score: " + bagScore.ToString();
        basetext.text = "Base Score: " + baseScore.ToString();
    }

    public void EndStun()
    {
        playerState = "Normal";
        stunIcon.SetActive(false);

        stunIcon.transform.localScale = Vector3.one;
        if (stunImage != null)
        {
            stunImage.fillAmount = 1f;
        }
    }

    //Necesito o que las variables de PlayerData sean publicas o crear un metodo que me devuelva el valor de las variables privadas, para poder mostrarlo en la UI
}
