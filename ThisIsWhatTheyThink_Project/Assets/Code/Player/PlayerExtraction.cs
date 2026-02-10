using UnityEngine;

public class PlayerExtraction : MonoBehaviour
{
    public GameObject m_playerExtractionZone;
    public KeyCode m_extractionKey;
    public PlayerData m_playerData;
    public float m_extractionTime;
    private float m_extractionProgress = 0f;

    private bool m_onExtractionZone = false;
    

    private void Update()
    {
        if (m_onExtractionZone && Input.GetKey(m_extractionKey) && m_playerData.CarryingCubes())
        {
            m_extractionProgress += Time.deltaTime;
        }
        else
        {
            m_extractionProgress = 0f;
        }
        if(m_extractionProgress > m_extractionTime && m_playerData.CarryingCubes())
        {
            m_playerData.Extract();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExtractionZone") && m_playerExtractionZone == other.gameObject)
        {
            m_onExtractionZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ExtractionZone") && m_playerExtractionZone == other.gameObject)
        {
            m_onExtractionZone = false;
        }
    }
}
