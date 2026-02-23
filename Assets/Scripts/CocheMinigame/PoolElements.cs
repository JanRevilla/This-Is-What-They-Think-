using System.Collections.Generic;
using UnityEngine;

public class PoolElements : MonoBehaviour
{
    private List<GameObject> elementPool;
    private int currentElementId = 0;

    public PoolElements(int elementsCount, GameObject prefabElement, Transform parentSpawn)
    {
        elementPool = new List<GameObject>();

        for (int i = 0; i < elementsCount; i++)
        {
            GameObject gameObject = GameObject.Instantiate(prefabElement);
            gameObject.transform.parent = parentSpawn;

            gameObject.SetActive(false);
            elementPool.Add(gameObject);
        }
    }

    public GameObject GetNextElement()
    {
        GameObject gameObject = elementPool[currentElementId];
        currentElementId++;

        if (currentElementId >= elementPool.Count)
        {
            currentElementId = 0;
        }

        return gameObject;
    }

    public GameObject CheckPool(int index)
    {
        return elementPool[index];
    }

    public void PutComponentInPool(List<Transform> l_Pos, float l_Speed, float l_DistanceToChangeDirection)
    {
        foreach (GameObject element in elementPool)
        {
            element.GetComponent<MovementSpawnElement>().Init(l_Pos, l_Speed, l_DistanceToChangeDirection);
        }
    }

    public void SetSpawnPositionComponentInPool(SpawnPeople _function)
    {
        foreach (GameObject element in elementPool)
        {
            element.GetComponent<MovementSpawnElement>().SetSpawnPosition(_function.RandomFirstPos());
        }
    }

    public int NumOfElements()
    {
        return elementPool.Count - 1;
    }
}
