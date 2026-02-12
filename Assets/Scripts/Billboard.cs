using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform m_TargetPosition;

    private void Start()
    {
        if (m_TargetPosition == null)
        {
            GameObject l_Player = GameObject.FindGameObjectWithTag("Player");
            if (l_Player != null)
            {
                m_TargetPosition = l_Player.transform;
            }
            else
            {
                Debug.LogError("Player Not Found");
            }
        }
    }

    void Update()
    {
        if (m_TargetPosition != null)
        {
            transform.LookAt(m_TargetPosition);
            transform.Rotate(0, 180, 0);

            //Debug
            Debug.DrawRay(transform.position, transform.forward * 2, Color.blue);
        }
    }
}
