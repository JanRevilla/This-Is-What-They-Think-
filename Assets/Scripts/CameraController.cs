using UnityEngine;
using UnityEngine.Audio;

public class CameraController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform m_PlayerBody; 
    public Transform m_PitchController;
    private CharacterController m_CharacterController;
    public PlayerController m_PlayerController;

    [Header("Rotación (Mouse Look)")]
    public float m_YawSpeed = 150f;
    public float m_PitchSpeed = 150f;
    public float m_MinPitch = -80f;
    public float m_MaxPitch = 80f;
    public bool m_UseInvertedYaw = false;
    public bool m_UseInvertedPitch = false;
    public KeyCode m_AngleLockedKeycode = KeyCode.I;

    [Header("Head Bobbing (Rebote)")]
    public bool m_UseBobbing = true;
    [Range(0, 0.1f)] public float m_BobAmplitude = 0.015f;
    [Range(0, 30)] public float m_BobFrequency = 12.0f;

    private float m_Yaw;
    private float m_Pitch;
    private bool m_AngleLocked = false;
    private Vector3 m_CameraStartPos;
    private float m_BobTimer;

    [Header("Audio")]
    public AudioClip[] m_AudioSteps;
    public AudioSource m_audioSource;
    private bool m_StepsPlayed;

    private void Start()
    {
        m_CharacterController = m_PlayerController.GetComponent<CharacterController>();

        m_CameraStartPos = transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_Yaw = m_PlayerBody.eulerAngles.y;
        float initialPitch = m_PitchController.localEulerAngles.x;
        if (initialPitch > 180) initialPitch -= 360;
        m_Pitch = initialPitch;

        m_audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleRotation();
        if (m_UseBobbing) HandleHeadBob();
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(m_AngleLockedKeycode))
            m_AngleLocked = !m_AngleLocked;

        if (m_AngleLocked) return;

        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");

        if (!m_PlayerController.m_CarMinigame)
            m_Yaw += l_MouseX * m_YawSpeed * Time.deltaTime * (m_UseInvertedYaw ? -1.0f : 1.0f);

        
        m_Pitch += l_MouseY * m_PitchSpeed * Time.deltaTime * (m_UseInvertedPitch ? -1.0f : 1.0f);
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        if (!m_PlayerController.m_CarMinigame)
            m_PlayerBody.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);

        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);
    }

    private void HandleHeadBob()
    {
        float l_Speed = new Vector3(m_CharacterController.velocity.x, 0, m_CharacterController.velocity.z).magnitude;

        if (l_Speed > 0.5f && m_CharacterController.isGrounded)
        {
            m_BobTimer += Time.deltaTime * (m_BobFrequency * (l_Speed/m_PlayerController.m_MaxSpeed));

            float l_waveValue = Mathf.Sin(m_BobTimer);

            float posX = m_CameraStartPos.x + Mathf.Sin(m_BobTimer * 0.5f) * m_BobAmplitude;
            float posY = m_CameraStartPos.y + Mathf.Sin(m_BobTimer) * m_BobAmplitude;

            transform.localPosition = new Vector3(posX, posY, m_CameraStartPos.z);

            //Debug.Log(m_BobTimer); DEBUG

            if (l_waveValue < -0.95f && !m_StepsPlayed)
            {
                //Debug.Log("AUDIO PASOS"); DEBUG
                PlayFootstep();
                m_StepsPlayed = true; // Bloqueamos para que no suene en cada frame del fondo
            }
            // Cuando la cabeza vuelve a subir, reseteamos el permiso para el próximo paso
            else if (l_waveValue > 0.0f)
            {
                m_StepsPlayed = false;
            }
        }
        else
        {
            //m_BobTimer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_CameraStartPos, Time.deltaTime * 10f);
        }
    }

    void PlayFootstep()
    {
        // Elegir un clip aleatorio de la lista
        int indice = Random.Range(0, m_AudioSteps.Length);
        m_audioSource.clip = m_AudioSteps[indice];

        // Variar el pitch para que cada paso sea único
        m_audioSource.pitch = Random.Range(0.75f, 1.10f);

        m_audioSource.PlayOneShot(m_audioSource.clip);
    }
}