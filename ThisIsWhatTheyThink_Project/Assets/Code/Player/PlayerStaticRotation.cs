using UnityEngine;

public class PlayerStaticRotation : MonoBehaviour
{
    private PlayerMovement m_player;
    [Range (0.0f, 2.0f)]public float m_MinRotationSpeed = 0.1f;
    [Range(0.0f, 2.0f)] public float m_MaxRotationSpeed = 2.0f;
    public float m_PersistanceTime = 0.2f;

    float m_oldX;
    float m_oldY;

    private float m_stopRotatingTimer;
    private float m_persistanceTimer;

    [SerializeField]private float m_rotationCount; //Serialized for debug purposes
    [SerializeField]private float m_rotationCountToSpin = 10.0f; 

    private void Awake()
    {
        m_player = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        float l_inputX = Input.GetAxisRaw("Horizontal");
        float l_inputY = Input.GetAxisRaw("Vertical");
        
        float deltaValueY = l_inputY - m_oldY;
        float deltaValueX = l_inputX - m_oldX;

        m_oldX = l_inputX;
        m_oldY = l_inputY;

        bool l_checkMinSpeed = Mathf.Abs(deltaValueX) >= m_MinRotationSpeed || Mathf.Abs(deltaValueY) >= m_MinRotationSpeed;
        bool l_checkMaxSpeed = Mathf.Abs(deltaValueX) < m_MaxRotationSpeed && Mathf.Abs(deltaValueY) < m_MaxRotationSpeed;
        if (l_checkMinSpeed && l_checkMaxSpeed)
        {
            m_rotationCount++;
            m_stopRotatingTimer = 0f;

            if (m_rotationCount >= m_rotationCountToSpin)
            {
                m_persistanceTimer = 0f;
            }
        }
        else
        {
            m_stopRotatingTimer += Time.deltaTime;
            if (m_stopRotatingTimer > m_PersistanceTime)
            {
                m_rotationCount = 0;
                m_player.AllowMovementRotation();
            }
        }
        if (m_persistanceTimer < m_PersistanceTime) Spin();
        m_persistanceTimer += Time.deltaTime;
    }

    private void Spin()
    {
        m_player.BlockMovement();
        m_player.ApplyAngularSpeed(1);
    }
}
