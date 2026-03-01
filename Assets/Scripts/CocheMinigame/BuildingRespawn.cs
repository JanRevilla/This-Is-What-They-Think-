using UnityEngine;

public class BuildingRespawn : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RespawnB")
        {
            transform.parent.GetComponent<MovementBuildings>().ResetBuildingPos(gameObject);
        }
    }
}
