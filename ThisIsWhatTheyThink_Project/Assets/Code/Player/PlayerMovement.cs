using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Linear Parameters")]
    /*[SerializeField]
    private float m_minLinearSpeed = 1.0f;
    [SerializeField]
    private float m_maxLinearSpeed = 5.0f;
    [SerializeField]
    private float m_linearAcceleration = 2.5f;*/
    public float m_minLinearSpeed = 1.0f;
    public float m_maxLinearSpeed = 5.0f;
    public float m_linearAcceleration = 2.5f;

    [Header("Angular Parameters")]
    /*[SerializeField]
    private float m_minAngularSpeed = 15.0f;
    [SerializeField]
    private float m_maxAngularSpeed = 90.0f;
    [SerializeField]
    private float m_angularAcceleration = 45.0f;*/
    public float m_minAngularSpeed = 15.0f;
    public float m_maxAngularSpeed = 360.0f;
    public float m_angularAcceleration = 180.0f;
    [SerializeField] private float m_closeEnoughAngle = 1.0f;

    public float m_currentRotation { get; private set; }
    private Vector3 m_currentLinearVelocity;
    private float m_currentAngularSpeed;

    private bool m_isRotating;


    private CharacterController m_characterController;

    private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        m_currentRotation = transform.rotation.eulerAngles.y;
    }
    private void Update()
    {
        Vector3 l_inputDirection = GetKeyboardInput();
        //float l_targetAngle = VectorToAngle(l_inputDirection);;

        ApplyLinearVelocity(l_inputDirection);

        if (!m_isRotating)
        {
            ApplyAngularSpeed(l_inputDirection);
        }
    }

    private Vector3 GetKeyboardInput()
    {
        return (Vector3.forward * Input.GetAxisRaw("Vertical") + Vector3.right * Input.GetAxisRaw("Horizontal")).normalized;
    }
    private void ApplyLinearVelocity(Vector3 targetDirection)
    {
        if (targetDirection != Vector3.zero)
        {
            m_currentLinearVelocity += targetDirection * m_linearAcceleration * Time.deltaTime;
        }
        else
        {
            m_currentLinearVelocity -= m_currentLinearVelocity.normalized * m_linearAcceleration * Time.deltaTime;
        }

        if (m_currentLinearVelocity.magnitude > m_maxLinearSpeed) m_currentLinearVelocity = m_currentLinearVelocity.normalized * m_maxLinearSpeed;
        if (Mathf.Abs(m_currentAngularSpeed) > m_maxAngularSpeed) m_currentAngularSpeed = m_maxAngularSpeed * Mathf.Sign(m_currentAngularSpeed);

        m_characterController.Move(m_currentLinearVelocity * Time.deltaTime);
    }
    private void ApplyAngularSpeed(Vector3 vector)
    {

        ApplyAngularSpeed(VectorToAngle(vector));
    }

    private void ApplyAngularSpeed(float targetAngle)
    {
        float l_requiredRotation = targetAngle - m_currentRotation;

        if (l_requiredRotation < 0.0f) l_requiredRotation += 360.0f;

        if (l_requiredRotation > 180.0f) l_requiredRotation = -(360.0f - l_requiredRotation);

        if (Mathf.Abs(l_requiredRotation) < m_closeEnoughAngle)
            m_currentAngularSpeed = 0.0f; //Should it decelerate instead of stopping?
        else
        {
            m_currentAngularSpeed += Mathf.Sign(l_requiredRotation) * m_angularAcceleration * Time.deltaTime;
            m_currentRotation += m_currentAngularSpeed * Time.deltaTime;
        }
        transform.rotation = Quaternion.Euler(0.0f, m_currentRotation, 0.0f);
    }
    public void ApplyAngularSpeed(int signValue)
    {
        m_currentAngularSpeed += Mathf.Sign(signValue) * m_angularAcceleration / 8 * Time.deltaTime;
        m_currentRotation += m_currentAngularSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0.0f, m_currentRotation, 0.0f);
    }
    public void BlockMovement()
    {
        m_isRotating = true;
        m_currentLinearVelocity = Vector3.zero;
    }
    public void AllowMovementRotation()
    {
        m_isRotating = false;
    }
    private float VectorToAngle(Vector3 vector)
    {
        if (vector.Equals(Vector3.zero))
        {
            return transform.eulerAngles.y;
        }

        Vector3 l_direction = vector.normalized;

        float sin = l_direction.x;
        float cos = l_direction.z;

        float tan = sin / cos;

        float orientation = Mathf.Atan(tan) * Mathf.Rad2Deg;

        if (cos < 0)
        {
            orientation += 180.0f;
        }
        return orientation;
    }
}
