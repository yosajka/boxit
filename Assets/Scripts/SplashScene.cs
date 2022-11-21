using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScene : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(ChangeScene());       
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("InitialScene");
    }
}
