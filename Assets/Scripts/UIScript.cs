using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Grey()
    {
        gameObject.GetComponent<Text>().color = new Color32(150, 150, 150, 255);
    }
    public void Red()
    {
        gameObject.GetComponent<Text>().color = Color.red;
    }
    public void Green()
    {
        gameObject.GetComponent<Text>().color = Color.green;
        SceneManager.LoadScene("Level1");
    }
    public void GreenMenu()
    {
        gameObject.GetComponent<Text>().color = Color.green;
        SceneManager.LoadScene("Main menu");
    }
    public void GreenCredits()
    {
        gameObject.GetComponent<Text>().color = Color.green;
        SceneManager.LoadScene("Credits");
    }
}
