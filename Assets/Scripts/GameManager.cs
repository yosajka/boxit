using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ball;
    public GameObject finishBox;
    public GameObject winCanvas;
    public GameObject ingameCanvas;
    GameObject[] listBox;
    GameObject g;
    Vector3 ballStartPosition;
    Vector3[] listBoxStartPosition;
    [SerializeField] Transform LevelScrollView;
    [SerializeField] GameObject LevelTemplate;

    // Start is called before the first frame update
    void Start()
    {
        ballStartPosition = ball.transform.position;
        listBox = GameObject.FindGameObjectsWithTag("CartonBox");
        if (listBox.Length != 0)
        {
            listBoxStartPosition = new Vector3[listBox.Length];
            //Debug.Log(listBox.Length);
            for (int i = 0; i < listBox.Length; i++)
            {
                listBoxStartPosition[i] = listBox[i].transform.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }



    public void Replay()
    {
        //ball.GetComponent<Rigidbody>().isKinematic = true;

        // ball.gameObject.SetActive(true);
        // //ball.transform.position = ballStartPosition;
        // ball.GetComponent<Rigidbody>().position = ballStartPosition;
        // for (int i = 0; i < listBox.Length; i++)
        // {
        //     listBox[i].transform.position = listBoxStartPosition[i];
        //     //listBox[i].GetComponent<Rigidbody>().position = listBoxStartPosition[i];
        // }

        // ball.GetComponent<Rigidbody>().isKinematic = false;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void ChooseLevel()
    {
        winCanvas.SetActive(true);
        ingameCanvas.SetActive(false);
        GameStateManager.Instance.winRound = true;
    }

    public void Setting()
    {

    }

    public void ExitToMenu()
    {

    }

    public void ReplayAfterWinning()
    {
        winCanvas.SetActive(false);
        ingameCanvas.SetActive(true);

        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.gameObject.SetActive(true);
        ball.transform.position = ballStartPosition;
        for (int i = 0; i < listBox.Length; i++)
        {
            listBox[i].transform.position = listBoxStartPosition[i];
        }
    }


}
