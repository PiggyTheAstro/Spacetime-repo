using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropScript : MonoBehaviour
{
    public List<GameObject> drops;
    GameObject drop;
    public GameObject mark;
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name == "DropsManager")
        {
            drop = drops[Random.Range(0, 10)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Drop()
    {
        if (drop.name != "Shotgun" && !drop.name.Contains("Heart") && !drop.name.Contains("Speed") && !drop.name.Contains("Shield"))
        {
            Instantiate(drop, EnemyScript.deathPos - new Vector3(4f, 0f, 0f), transform.rotation);
        }
        else if(drop.name == "Shotgun")
        {
            Instantiate(drop, EnemyScript.deathPos - new Vector3(4f, 0f, 0f), drop.transform.rotation);
        }
        else
        {
            Instantiate(drop, EnemyScript.deathPos - new Vector3(4f, 0f, 0f), Quaternion.identity);
        }
        if (GameObject.Find("LevelManager").GetComponent<LevelScript>().unmarkedLevels.Contains(SceneManager.GetActiveScene().name))
        {
            Instantiate(mark, EnemyScript.deathPos + new Vector3(4f, 0f, 0f), drop.transform.rotation);

        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            if (!gameObject.name.Contains("Heart") && !gameObject.name.Contains("Speed") && !gameObject.name.Contains("Mark") && !gameObject.name.Contains("Shield"))
            {
                PlayerScript.weapon = gameObject.name;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().ChangeScene();
            }
            else if(gameObject.name.Contains("Heart"))
            {
                PlayerScript.health += 3f;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().ChangeScene();
            }
            else if (gameObject.name.Contains("Mark"))
            {
                LevelScript.marked = SceneManager.GetActiveScene().name;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().ChangeScene();
            }
            else if (gameObject.name.Contains("Shield"))
            {
                if (PlayerScript.reflectChance != 3f)
                {
                    PlayerScript.reflectChance -= 2f;
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().ChangeScene();
                }
            }
            else
            {
                PlayerScript.speed += 1000f;
                PlayerScript.maxSpeed += 1f;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().ChangeScene();
            }
        }
    }
}
