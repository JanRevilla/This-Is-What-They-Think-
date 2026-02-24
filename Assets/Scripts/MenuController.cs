using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static System.TimeZoneInfo;

public class MenuController : MonoBehaviour
{
    [Header("References")]
    public Camera m_mainCamera;
    public Transform m_menuPoint;      // Punto A (Menú)
    public Transform m_cameraTarget;   // Punto B (Ojos del Player)
    public GameObject m_menuCanvas;

    [Header("Scripts a Controlar")]
    public MonoBehaviour PlayerController;
    public MonoBehaviour CameraController;

    [Header("Configuración")]
    public float m_transitionTime = 2.5f;
    public AnimationCurve m_transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        m_mainCamera.transform.position = m_menuPoint.position;
        m_mainCamera.transform.rotation = m_menuPoint.rotation;

        PlayerController.enabled = false;
        CameraController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnPlay()
    {
        m_menuCanvas.SetActive(false);
        StartCoroutine(MoveCameraToPlayer());
    }

    IEnumerator MoveCameraToPlayer()
    {
        float elapsed = 0;
        Vector3 startPos = m_mainCamera.transform.position;
        Quaternion startRot = m_mainCamera.transform.rotation;

        while (elapsed < m_transitionTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / m_transitionTime;
            float curveT = m_transitionCurve.Evaluate(t);

            m_mainCamera.transform.position = Vector3.Lerp(startPos, m_cameraTarget.position, curveT);
            m_mainCamera.transform.rotation = Quaternion.Slerp(startRot, m_cameraTarget.rotation, curveT);

            yield return null;
        }

        m_mainCamera.transform.position = m_cameraTarget.position;
        m_mainCamera.transform.rotation = m_cameraTarget.rotation;

        m_mainCamera.transform.SetParent(m_cameraTarget);

        ActivarGameplay();
    }
    void ActivarGameplay()
    {
        PlayerController.enabled = true;
        CameraController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
