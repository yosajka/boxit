using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    public bool winRound = false;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    [System.Serializable]
    public class GameLevel
    {
        public Sprite Image;
        public bool IsCompleted = false;
        
    }
    public List<GameLevel> LevelList;

    public void Start()
    {
        SceneManager.LoadScene("Level1");
    }

}