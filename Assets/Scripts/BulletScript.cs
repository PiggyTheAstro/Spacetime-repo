using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletScript : MonoBehaviour
{
    float diceroll;
    public GameObject bossBullet;
    public List<GameObject> firePoints;
    float health;
    // Start is called before the first frame update
    void Start()
    {
        if (!gameObject.name.Contains("Minion"))
        {
            Invoke("Die", 4f);
        }
        else
        {
            health = 2f;
            InvokeRepeating("Shoot", 0.3f, 0.3f);
            Invoke("Stop", 2f);
            Invoke("Die", 4f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health < 0f)
        {
            Destroy(gameObject);
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!PlayerScript.rewind)
        {
            if (gameObject.tag == "Friendly")
            {
                if (col.gameObject.name == "Enemy")
                {
                    if (SceneManager.GetActiveScene().name != "Level4")
                    {
                        if (!PlayerScript.weapon.Contains("Sniper") && !PlayerScript.weapon.Contains("Rifle") && !PlayerScript.weapon.Contains("Submachine"))
                        {
                            col.gameObject.GetComponent<EnemyScript>().health -= 1f;
                        }
                        else if (PlayerScript.weapon.Contains("Sniper"))
                        {
                            col.gameObject.GetComponent<EnemyScript>().health -= 6f;
                        }
                        else if (PlayerScript.weapon.Contains("Rifle"))
                        {
                            col.gameObject.GetComponent<EnemyScript>().health -= 1.5f;
                        }
                        else if (PlayerScript.weapon.Contains("Submachine"))
                        {
                            col.gameObject.GetComponent<EnemyScript>().health -= 0.3f;
                        }
                        col.gameObject.GetComponent<Renderer>().sharedMaterial = col.gameObject.GetComponent<EnemyScript>().flash;
                        Destroy(gameObject);
                    }
                    else
                    {
                        diceroll = Mathf.Round(Random.Range(1f, 2f));
                        if(diceroll == 1f)
                        {
                            if (!PlayerScript.weapon.Contains("Sniper") && !PlayerScript.weapon.Contains("Rifle"))
                            {
                                col.gameObject.GetComponent<EnemyScript>().health -= 1f;
                            }
                            else if (PlayerScript.weapon.Contains("Sniper"))
                            {
                                col.gameObject.GetComponent<EnemyScript>().health -= 6f;
                            }
                            else if (PlayerScript.weapon.Contains("Rifle"))
                            {
                                col.gameObject.GetComponent<EnemyScript>().health -= 3f;
                            }
                            Destroy(gameObject);
                        }
                        else
                        {
                            gameObject.GetComponent<Rigidbody2D>().velocity = -GetComponent<Rigidbody2D>().velocity;
                            gameObject.tag = "Hostile";
                            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                        }
                        
                    }
                }
            }
            else if (gameObject.tag == "Hostile")
            {
                if (col.gameObject.name == "Player")
                {
                    if (PlayerScript.reflectChance < 11f)
                    {
                        diceroll = Mathf.Round(Random.Range(1f, PlayerScript.reflectChance));
                        if (diceroll > 1f)
                        {
                            col.gameObject.GetComponent<Renderer>().material = col.gameObject.GetComponent<PlayerScript>().enemyMat;
                            PlayerScript.health -= 2f;
                            Destroy(gameObject);
                        }
                        else
                        {
                            gameObject.GetComponent<Rigidbody2D>().velocity = -GetComponent<Rigidbody2D>().velocity;
                            gameObject.tag = "Friendly";
                            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                        }
                    }
                    else
                    {
                        col.gameObject.GetComponent<Renderer>().sharedMaterial = col.gameObject.GetComponent<PlayerScript>().enemyMat;
                        PlayerScript.health -= 2f;
                        Destroy(gameObject);
                    }
                }
                if (gameObject.name.Contains("Minion"))
                {
                    if(col.gameObject.tag == "Friendly")
                    {
                        health -= 1f;
                        Destroy(col.gameObject);
                    }
                }
            }
        }
    }
    void Shoot()
    {
        foreach(GameObject point in firePoints)
        {
            GameObject bullet = Instantiate(bossBullet, point.transform.position, point.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * 20000f, ForceMode2D.Impulse);
        }
    }
    void Stop()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }

}
