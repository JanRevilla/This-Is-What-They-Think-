using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    static private GameManager m_GameManager;

    PlayerController m_Player;

    public Transform m_DestroyObjects;

    [SerializeField]
    bool _IsPaused;
    public GameObject _MenuPause;
    GameObject _MainMenu;

    // public Fade m_Fade;

    private bool m_KeyPicked;

    /*public string m_FirstSceneName = "FirstLevel";
    public string m_SecondSceneName = "SecondLevel";*/

    [Header("Configuración de Transición")]
    [SerializeField] private Animator m_CanvaFadeAnimator;
    [SerializeField] private float m_WaitTime = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        m_GameManager = this;
        m_KeyPicked = false;

        _MainMenu = GameObject.Find("MenuCanvas");
        DontDestroyOnLoad(gameObject);
    }

    public static GameManager GetGameManager()
    {
        return m_GameManager;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_MainMenu == null)
            {
                _MainMenu = GameObject.Find("MenuCanvas");
                Debug.Log(_MainMenu);
            }

            if (_MainMenu != null && _MainMenu.activeInHierarchy)
            {
                return;
            }
            else
            {
                if (_IsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }        
        }   
    }
    public void GameOverScreen()
    {
        //m_Canvas.SetActive(true);
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

    public void Resume()
    {
        _MenuPause.SetActive(false);
        Time.timeScale = 1f;
        _IsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource source in allAudioSources)
        {
            source.UnPause();
        }
    }

    void Pause()
    {
        _MenuPause.SetActive(true);
        Time.timeScale = 0f;
        _IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource source in allAudioSources)
        {
            source.Pause();
        }
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    //-------------------------------------SCENE CHANGER---------------------------------------------//
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(TransitionProcess(sceneName));
    }

    IEnumerator TransitionProcess(string sceneName)
    {
        if (m_CanvaFadeAnimator != null)
        {
            m_CanvaFadeAnimator.SetTrigger("FadeOut");
        }

        yield return new WaitForSeconds(m_WaitTime);

        SceneManager.LoadScene(sceneName);
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        _IsPaused = false;

        if (DialogsController.Instance != null)
        {
            DialogsController.Instance.m_audioSource.Stop();
        }

        SceneManager.LoadScene("Main");

        AudioListener.pause = false;
        _MenuPause.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}