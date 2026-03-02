using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string _SceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            /*StartCoroutine(CanvasFadeOut());
            SceneManager.LoadScene(_SceneName);*/
            GameManager.Instance.ChangeScene(_SceneName);
        }
    }

    IEnumerator CanvasFadeOut()
    {

        yield return new WaitForSeconds(1);
    }
}
