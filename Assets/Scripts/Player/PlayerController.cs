using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public CharacterController m_CharacterController;
    public float m_MaxSpeed = 5.0f;
    public float m_RunMultiplier = 2.0f;
    public float m_Acceleration = 10.0f;
    public float m_Deceleration = 12.0f;
    public float m_JumpSpeed = 8.0f;
    public float m_GravityMultiplier = 2.0f;

    [Header("Input")]
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;

    [Header("Car Minigame")]
    public bool m_CarMinigame = false;
    public Transform _fakeFade;

    private Vector3 m_CurrentHorizontalVelocity;
    private float m_VerticalSpeed;


    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float inputX = 0;
        float inputZ = 0;

        if (Input.GetKey(KeyCode.D)) inputX = 1;
        else if (Input.GetKey(KeyCode.A)) inputX = -1;

        if (!m_CarMinigame)
        {
            if (Input.GetKey(KeyCode.W)) inputZ = 1;
            else if (Input.GetKey(KeyCode.S)) inputZ = -1;
        }

        Vector3 targetDirection = (transform.forward * inputZ + transform.right * inputX).normalized;

        float currentMaxSpeed = m_MaxSpeed * (Input.GetKey(m_RunKeyCode) ? m_RunMultiplier : 1.0f);
        Vector3 targetVelocity = targetDirection * currentMaxSpeed;

        float lerpSpeed = (targetDirection.magnitude > 0) ? m_Acceleration : m_Deceleration;

        m_CurrentHorizontalVelocity = Vector3.Lerp(
            m_CurrentHorizontalVelocity,
            targetVelocity,
            lerpSpeed * Time.deltaTime
        );

        m_VerticalSpeed += Physics.gravity.y * m_GravityMultiplier * Time.deltaTime;

        Vector3 finalMovement = m_CurrentHorizontalVelocity;
        finalMovement.y = m_VerticalSpeed;

        CollisionFlags l_Flags = m_CharacterController.Move(finalMovement * Time.deltaTime);

        if ((l_Flags & CollisionFlags.Below) != 0)
        {
            m_VerticalSpeed = 0;
            //Salto
            //if (Input.GetKeyDown(KeyCode.Space)) m_VerticalSpeed = m_JumpSpeed;
        }
        else if ((l_Flags & CollisionFlags.Above) != 0)
        {
            m_VerticalSpeed = 0;
        }
    }


    //-----------------------------SCENE CONTROLLER------------------------------//
    /*void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");

        if (spawnPoint != null)
        {
            m_CharacterController.enabled = false;
            transform.position = spawnPoint.transform.position;
            transform.rotation = spawnPoint.transform.rotation;
            m_CharacterController.enabled = true;
        }
        else
        {
            Debug.LogWarning("No scene found!!!");
        }
    }*/
}