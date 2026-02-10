using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private float m_currentScore; //Serialized for debug
    [SerializeField] private float m_extractedCubesCounter; //Serialized for debug
    private List<GameObject> m_carriedCubes = new List<GameObject>();


    public void Collect(GameObject cube)
    {
        m_carriedCubes.Add(cube);
    }
    /*public void Extract(GameObject cube, CubeBase type)
    {
        m_extractedCubesCounter++;
        m_currentScore += type.score;

        m_carriedCubes.Remove(cube);
    }*/
    public void Extract()
    {
        for (int i = 0; i < m_carriedCubes.Count; i++)
        {
            m_extractedCubesCounter++;
            //CubeBase type = obj.GetComponent<CubeBase>();
            //m_currentScore += type.score;
            Debug.Log(m_carriedCubes.Count);
            m_carriedCubes.RemoveAt(i);
            Debug.Log("Extracted");
            i--;
            continue;
        }
        //m_carriedCubes = new List<GameObject>();
    }
    public bool CarryingCubes()
    {
        if(m_carriedCubes.Count>0)
        {
            return true;
        }
        return false;
    }
}
