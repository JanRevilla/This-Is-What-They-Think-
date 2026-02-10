using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Rotation")]

    private float m_Yaw;
    private float m_Pitch;

    public float m_YawSpeed;
    public float m_PitchSpeed;

    public float m_MinPitch;
    public float m_MaxPitch;

    public Transform m_PitchController;

    public bool m_UseInvertedYaw;
    public bool m_UseInvertedPitch;

    [Header("Movement")]

    public CharacterController m_CharacterController;

    private float m_VerticalSpeed;

    public float m_Speed;
    public float m_SpeedMultiplier;
    public float m_JumpSpeed;

    public float m_GravityMultiplier;

    private bool m_AngleLocked = false;

    [Header ("Camera")]

    public Camera m_Camera;

    [Header("Input")]

    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;
    public KeyCode m_ReloadKeyCode = KeyCode.R;
    public int m_ShootMouseButton = 0;

    [Header("Debug Input")]
    public KeyCode m_AngleLockedKeycode = KeyCode.I;


    private void Start()
    {
        /* PlayerController l_Player = GameManager.GetGameManager().GetPlayer();     // evitar que es crein + players entre scenes
        if (l_Player != null)
        {
            l_Player.m_CharacterController.enabled = false;
            l_Player.transform.position = transform.position;
            l_Player.transform.rotation = transform.rotation;
            l_Player.m_CharacterController.enabled = true;

            l_Player.m_RespawnPosition = transform.position;
            l_Player.m_StartRotation = transform.rotation;

            GameObject.Destroy(gameObject);
            return;
        } */

        // m_RespawnPosition = transform.position;
        // m_StartRotation = transform.rotation;

        DontDestroyOnLoad(gameObject); 

        // GameManager.GetGameManager().SetPlayer(this);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(m_AngleLockedKeycode)) // bloqueig de camera
            m_AngleLocked = !m_AngleLocked;

        if (!m_AngleLocked)
        {
            m_Yaw = m_Yaw + l_MouseX * m_YawSpeed * Time.deltaTime * (m_UseInvertedYaw ? -1.0f : 1.0f);
            m_Pitch = m_Pitch + l_MouseY * m_PitchSpeed * Time.deltaTime * (m_UseInvertedPitch ? -1.0f : 1.0f);

            m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

            transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
            m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        }

        Vector3 l_Movement = Vector3.zero;

        //float l_YawPiRadians = m_Yaw * Mathf.Deg2Rad;
        //float l_Yaw90PiRadians = (m_Yaw + 90) * Mathf.Deg2Rad;

        //Vector3 l_ForwardDirection = new Vector3(Mathf.Sin(l_YawPiRadians), 0.0f, Mathf.Cos(l_YawPiRadians));
        //Vector3 l_RightDirection = new Vector3(Mathf.Sin(l_Yaw90PiRadians), 0.0f, Mathf.Cos(l_Yaw90PiRadians));

        Vector3 l_ForwardDirection = transform.forward; 
        Vector3 l_RightDirection = transform.right;

        if (Input.GetKey(m_RightKeyCode))
            l_Movement = l_RightDirection;
        else if (Input.GetKey(m_LeftKeyCode))
            l_Movement += -l_RightDirection;

        if (Input.GetKey(m_UpKeyCode))
            l_Movement += l_ForwardDirection;
        else if (Input.GetKey(m_DownKeyCode))
            l_Movement += -l_ForwardDirection;

        float l_SpeedMultiplier = 1.0f;
        if (Input.GetKey(m_RunKeyCode))
            l_SpeedMultiplier = m_SpeedMultiplier;

        l_Movement.Normalize();
        l_Movement *= m_Speed * l_SpeedMultiplier * Time.deltaTime;

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * m_GravityMultiplier * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;

        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Movement);

        if (((m_VerticalSpeed < 0.0f) && ((l_CollisionFlags & CollisionFlags.Below) != 0)))
        {
            m_VerticalSpeed = 0.0f;
        }
        else if (m_VerticalSpeed > 0.0f && (l_CollisionFlags & CollisionFlags.Above) != 0)
        {
            m_VerticalSpeed = 0.0f;
        }

        if (Input.GetKeyDown(m_JumpKeyCode))
        {
            m_VerticalSpeed = m_JumpSpeed;
        }
    }
}
