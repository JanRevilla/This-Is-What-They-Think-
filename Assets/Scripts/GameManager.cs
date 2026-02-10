using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static private GameManager m_GameManager;

    PlayerController m_Player;

    public Transform m_DestroyObjects;

    public GameObject m_Canvas;

    // public Fade m_Fade;

    private bool m_KeyPicked;

    public string m_FirstSceneName = "FirstLevel";
    public string m_SecondSceneName = "SecondLevel";

    private void Awake()
    {
        if (m_GameManager != null)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        m_GameManager = this;
        m_KeyPicked = false;
        DontDestroyOnLoad(gameObject);
    }

    public static GameManager GetGameManager()
    {
        return m_GameManager;
    }
    public void GameOverScreen()
    {
        m_Canvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    public void RestartLevel()
    {

        for (int i = 0; i < m_DestroyObjects.childCount; ++i)
        {
            Destroy(m_DestroyObjects.GetChild(i).gameObject);
        }

        /*m_Player.Restart();
        m_Fade.FadeOut(() =>
        {
            m_Fade.gameObject.SetActive(false);
        });
        */
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadSceneAsync(m_FirstSceneName);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadSceneAsync(m_SecondSceneName);
        }
    }

    public PlayerController GetPlayer()
    {
        return m_Player;
    }

    public void SetPlayer(PlayerController Player)
    {
        m_Player = Player;
    }

    public void SetKey(bool key)
    {
        m_KeyPicked = key;
    }

    public bool GetKey()
    {
        return m_KeyPicked;
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
        /*m_Fade.FadeOut(() =>
        {
            m_Fade.gameObject.SetActive(false);
        });
        */
    }
}