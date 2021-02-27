using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class LevelScript : MonoBehaviour
{
    public List<string> levels;
    public List<string> unmarkedLevels;
    public static string marked;
    public static LevelScript instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(levels[levels.IndexOf(SceneManager.GetActiveScene().name) + 1]);

    }
    public void RewindScene()
    {
        if (marked != null && unmarkedLevels.Contains(marked))
        {
            SceneManager.LoadScene(marked, LoadSceneMode.Single);
            unmarkedLevels.Remove(marked);
            marked = null;
        }
        else
        {
            Destroy(GameObject.Find("Player"));
            marked = null;
            SceneManager.LoadScene("Main menu");
            Destroy(gameObject);
        }
    }
}
