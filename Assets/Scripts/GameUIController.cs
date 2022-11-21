using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inGameCanvas;
    public GameObject winCanvas;
    GameObject g;
    Button levelBtn;
    [SerializeField] Transform LevelScrollView;
    [SerializeField] GameObject LevelTemplate;
    void Start()
    {
        inGameCanvas.SetActive(true);
        winCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameStateManager.Instance.winRound)
        {
            inGameCanvas.SetActive(false);
            winCanvas.SetActive(true);
            GameStateManager.Instance.winRound = false;
            int len = GameStateManager.Instance.LevelList.Count;
            for (int i = 0; i < len; i++)
            {
                g = Instantiate(LevelTemplate, LevelScrollView);
                g.transform.GetChild(0).GetComponent<Image>().sprite = GameStateManager.Instance.LevelList[i].Image;
                levelBtn = g.transform.GetChild(2).GetComponent<Button>();
                levelBtn.AddEventListener(i, OnLevelBtnClicked);
            }
        }
    }

    void OnLevelBtnClicked(int levelIndex)
    {
        Debug.Log(levelIndex + "....................");
        SceneManager.LoadScene("Level"+((levelIndex+1).ToString()));
    }
}
