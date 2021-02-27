using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    float angle;
    Vector3 dir;
    GameObject player;
    public Rigidbody2D rigid;
    public GameObject bossBullet;
    public GameObject firingPoint;
    public List<GameObject> points;
    public List<GameObject> children;
    public float health;
    bool invoked;
    bool dirchanged;
    bool right;
    bool invisible;
    float speed;
    public static Vector3 deathPos;
    public GameObject minion;
    Material mat;
    public Material flash;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            InvokeRepeating("Shoot", 2f, 2f);
        }
        else if(SceneManager.GetActiveScene().name == "Level2")
        {
            InvokeRepeating("Shoot", 0.2f, 0.2f);
        }
        else if(SceneManager.GetActiveScene().name == "Level3")
        {
            InvokeRepeating("Shoot", 0.6f, 0.6f);
            InvokeRepeating("Cloak", 10f, 10f);
        }
        else if(SceneManager.GetActiveScene().name == "Level4")
        {
            InvokeRepeating("Shoot", 0.6f, 0.6f);
            InvokeRepeating("Charge", 5f, 10f);
        }
        else if(SceneManager.GetActiveScene().name == "Level5")
        {
            InvokeRepeating("Shoot", 0.7f, 0.7f);
            InvokeRepeating("Minion", 2.2f, 2.2f);
        }
        rigid = GetComponent<Rigidbody2D>();
        invoked = false;
        dirchanged = false;
        right = true;
        invisible = false;
        mat = GetComponent<Renderer>().material;
        if (SceneManager.GetActiveScene().name != "Level5")
        {
            speed = 7f;
        }
        else
        {
            speed = 10.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Renderer>().sharedMaterial == flash)
        {
            Invoke("Restore", 0.05f);
        }
        if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level3" || SceneManager.GetActiveScene().name == "Level5")
        {
            if (!PlayerScript.rewind)
            {

                dir = player.transform.position - transform.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                rigid.rotation = angle;
            }
            else
            {
                if (invisible)
                {
                    Uncloak();
                }
            }
        }
        else if(SceneManager.GetActiveScene().name == "Level2")
        {
            if (!PlayerScript.rewind) 
            {
                if (right)
                {
                    rigid.rotation += 100f * Time.deltaTime;
                }
                else
                {
                    rigid.rotation -= 100f * Time.deltaTime;
                }
                if(health <= 15 && invoked == false)
                {
                    InvokeRepeating("Velocity", 0f, 3f);
                    invoked = true;
                }
                if(health <= 5 && dirchanged == false)
                {
                    InvokeRepeating("Change", 0f, 1f);
                    dirchanged = true;
                }
            }

        }
        else if(SceneManager.GetActiveScene().name == "Level4")
        {
            if (!PlayerScript.rewind)
            {
                rigid.rotation -= 100f * Time.deltaTime;
                dir = player.transform.position - transform.position;
            }
        }
            if (health <= 0f)
            {
                Die();
            }
        
        
    }
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Level3" || SceneManager.GetActiveScene().name == "Level5")
        {
            if (!PlayerScript.rewind)
            {
                rigid.transform.position += transform.up * speed*Time.deltaTime;
            }
        }
    }
    void Shoot()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            if (!PlayerScript.rewind)
            {
                if (health <= 9f)
                {
                    GameObject bullet = Instantiate(bossBullet, firingPoint.transform.position, firingPoint.transform.rotation);
                    bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 15000f, ForceMode2D.Impulse);
                    if (health <= 5f)
                    {
                        Invoke("Shoot", 0.5f);
                    }
                }
            }
        }
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            if (!PlayerScript.rewind)
            {
                foreach (GameObject point in points)
                {
                    GameObject bullet = Instantiate(bossBullet, point.transform.position, point.transform.rotation);
                    bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * 13000f, ForceMode2D.Impulse);
                }
            }
        }
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            if (!PlayerScript.rewind)
            {
                if (invisible == false)
                {
                    foreach (GameObject point in points)
                    {
                        GameObject bullet = Instantiate(bossBullet, firingPoint.transform.position, firingPoint.transform.rotation);
                        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * 12000f, ForceMode2D.Impulse);
                        if (points.IndexOf(firingPoint) != 6)
                        {
                            firingPoint = points[points.IndexOf(firingPoint) + 1];
                        }
                        else
                        {
                            firingPoint = points[0];
                        }
                    }
                }
            }
        }
        if(SceneManager.GetActiveScene().name == "Level4")
        {
            if (!PlayerScript.rewind)
            {
                foreach (GameObject point in points)
                {
                    GameObject bullet = Instantiate(bossBullet, point.transform.position, point.transform.rotation);
                    bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * 8000f, ForceMode2D.Impulse);
                }
            }
        }
        if(SceneManager.GetActiveScene().name == "Level5")
        {
            if (!PlayerScript.rewind)
            {
                foreach (GameObject point in points)
                {
                    GameObject bullet = Instantiate(bossBullet, point.transform.position, point.transform.rotation);
                    bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * 21000f, ForceMode2D.Impulse);
                }
            }
        }

    }
    void Die()
    {
        deathPos = transform.position;
        GameObject.Find("Emitter").transform.position = deathPos;
        GameObject.Find("Emitter").GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }
    void Velocity()
    {
        if (health <= 15f)
        {
            if (!PlayerScript.rewind)
            {
                rigid.velocity = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            }
        }
    }
    void Change()
    {
        if (health <= 10f)
        {
            right = !right;
        }
        else
        {
            right = true;
        }
    }
    void Cloak()
    {
        if (!PlayerScript.rewind)
        {
            gameObject.layer = LayerMask.NameToLayer("Invis");
            foreach (GameObject child in children)
            {
                child.layer = LayerMask.NameToLayer("Invis");
            }
            invisible = true;
            Invoke("Uncloak", 5f);
        }
    }
    void Uncloak()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (GameObject child in children)
        {
            child.layer = LayerMask.NameToLayer("Default");
        }
        invisible = false;
        if (health <= 25)
        {
            speed = 22f;
            Invoke("Stopdash", 0.5f);
        }
    }
    void Stopdash()
    {
        speed = 7f;
    }
    void Charge()
    {
        rigid.AddForce(dir*500000, ForceMode2D.Impulse);
    }
    void Minion()
    {
        GameObject bullet = Instantiate(minion, firingPoint.transform.position, firingPoint.transform.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 17000f, ForceMode2D.Impulse);
    }
    void Restore()
    {
        GetComponent<Renderer>().sharedMaterial = mat;
    }
}
