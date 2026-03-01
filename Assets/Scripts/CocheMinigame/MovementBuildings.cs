using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MovementBuildings : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> buildings = new();

    [SerializeField]
    private float speed = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        buildings[0].transform.position += Vector3.back * speed * Time.deltaTime;
        buildings[1].transform.position += Vector3.back * speed * Time.deltaTime;
    }

    public void ResetBuildingPos(GameObject gO)
    {
        gO.SetActive(false);

        foreach(GameObject b in buildings)
        {
            if(b.activeSelf)
                gO.transform.position = b.transform.position + Vector3.forward * (b.transform.localScale.z * 1.5f);
        }

        gO.SetActive(true);
    }
}
