using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRecollection : MonoBehaviour
{
    public PlayerMovement m_PlayerMovement;
    public PlayerData m_PlayerData;
    private float m_startMinLinearSpeed;
    private float m_startMaxLinearSpeed;
    private float m_startLinearAcceleration;
    private List<GameObject> m_absorbObjects = new List<GameObject>();
    

    [Header("Recollect")]
    public KeyCode m_RecollectKey = KeyCode.Space;
    [Tooltip("0 is no movement | 1 is no movement penalty")]
    [Range(0f, 1f)]public float m_speedPenalization;

    [Header("Configuración del Cono")]
    [SerializeField] private Transform m_recollectDestination;
    [SerializeField] private float m_alcanceMaximo = 10f;
    [SerializeField] private float m_anguloCono = 45f;
    [SerializeField] private float m_fuerzaAbsorcion = 2f;
    [SerializeField] private float m_distanciaDestruccion = 0.5f;
    [SerializeField] private float m_velocidadRotacion = 30f;
    [SerializeField] private LayerMask m_capasAbsorbibles;
    private Tplayerstates m_PlayerState;
    private float m_recollectedCubes = 0;

    public enum Tplayerstates
    {
        Movement,
        Recollection
    }

    void Start()
    {
        m_PlayerState = Tplayerstates.Movement;
        m_startMinLinearSpeed = m_PlayerMovement.m_minLinearSpeed;
        m_startMaxLinearSpeed = m_PlayerMovement.m_maxLinearSpeed;
        m_startLinearAcceleration = m_PlayerMovement.m_linearAcceleration;
    }


    void Update()
    {

        if (Input.GetKeyDown(m_RecollectKey))
        {
            m_PlayerState = Tplayerstates.Recollection;
            StartRecollection();
        }
        if (Input.GetKeyUp(m_RecollectKey))
        {
            StopRecollection();
            m_PlayerState = Tplayerstates.Movement;
            StartMovement();
        }
        if(m_PlayerState == Tplayerstates.Recollection)
        {
            CheckForNewCubes();
            RecollectionUpdate();
        }
    }

    void StartRecollection()
    {
        m_PlayerMovement.m_minLinearSpeed = m_startMinLinearSpeed * m_speedPenalization;
        m_PlayerMovement.m_maxLinearSpeed = m_startMaxLinearSpeed * m_speedPenalization;
        m_PlayerMovement.m_linearAcceleration = m_startLinearAcceleration * m_speedPenalization;
        Collider[] collidersEnRango = Physics.OverlapSphere(m_recollectDestination.position, m_alcanceMaximo, m_capasAbsorbibles);
        foreach (Collider col in collidersEnRango)
        {
            if (CheckCone(col.transform))
            {
                col.transform.parent = m_recollectDestination;
                col.GetComponent<Rigidbody>().isKinematic = false;
                m_absorbObjects.Add(col.gameObject);
            }
        }
    }
    void StartMovement()
    {
        m_PlayerMovement.m_minLinearSpeed = m_startMinLinearSpeed;
        m_PlayerMovement.m_maxLinearSpeed = m_startMaxLinearSpeed;
        m_PlayerMovement.m_linearAcceleration = m_startLinearAcceleration;
    }
    void CheckForNewCubes()
    {
        Collider[] collidersEnRango = Physics.OverlapSphere(m_recollectDestination.position, m_alcanceMaximo, m_capasAbsorbibles);
        foreach (Collider col in collidersEnRango)
        {
            if (CheckCone(col.transform)&& !m_absorbObjects.Contains(col.gameObject))
            {
                col.transform.parent = m_recollectDestination;
                col.GetComponent<Rigidbody>().isKinematic = false;
                m_absorbObjects.Add(col.gameObject);
            }
        }
    }
    void RecollectionUpdate()
    {
        List<GameObject> l_removeList = new List<GameObject>();
        foreach(GameObject obj in m_absorbObjects)
        {
            if (obj == null)
            {
                continue;
            }
            Vector3 l_direction = (m_recollectDestination.position - obj.transform.position).normalized;
            m_recollectDestination.transform.Rotate(Vector3.forward, m_velocidadRotacion);
            obj.transform.position += l_direction * m_fuerzaAbsorcion * Time.deltaTime;

            float l_distance = Vector3.Distance(m_recollectDestination.position, obj.transform.position);
            if(l_distance <= m_distanciaDestruccion)
            {
                //Debug.Log("Destroyed");
                obj.transform.parent = null;
                m_recollectedCubes++;
                obj.SetActive(false);
                l_removeList.Add(obj);
                m_PlayerData.Collect(obj);
            }
        }
        foreach(GameObject obj in l_removeList)
        {
            m_absorbObjects.Remove(obj);
        }
    }

    void StopRecollection()
    {
        foreach (GameObject obj in m_absorbObjects)
        {
            obj.transform.parent = null;
            obj.GetComponent<Rigidbody>().isKinematic = true;
        }
        m_absorbObjects.Clear();
    }

    bool CheckCone(Transform m_objectTransform)
    {
        Vector3 l_direccionObjetivo = m_objectTransform.position-m_recollectDestination.position;
        float l_distance = l_direccionObjetivo.magnitude;
        if(l_distance > m_alcanceMaximo)
        {
            return false;  
        }
        float l_angle = Vector3.Angle(m_recollectDestination.forward, l_direccionObjetivo);
        return l_angle <= m_anguloCono;
    }
    void OnDrawGizmos()
    {
        if (m_recollectDestination == null) return;

        Gizmos.color = Color.yellow;
        Vector3 forward = m_recollectDestination.forward * m_alcanceMaximo;
        Vector3 right = Quaternion.Euler(0, m_anguloCono, 0) * forward;
        Vector3 left = Quaternion.Euler(0, -m_anguloCono, 0) * forward;
        Vector3 up = Quaternion.Euler(m_anguloCono, 0, 0) * forward;
        Vector3 down = Quaternion.Euler(-m_anguloCono, 0, 0) * forward;
        
        Gizmos.DrawRay(m_recollectDestination.position, right);
        Gizmos.DrawRay(m_recollectDestination.position, left);
        Gizmos.DrawRay(m_recollectDestination.position, up);
        Gizmos.DrawRay(m_recollectDestination.position, down);
        Gizmos.DrawWireSphere(m_recollectDestination.position, 0.2f);
    }
}
