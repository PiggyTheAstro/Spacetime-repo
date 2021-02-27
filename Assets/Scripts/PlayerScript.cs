using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public static bool rewind;
    float horizontal;
    float vertical;
    Vector3 movement;
    Rigidbody2D rb;
    public static float speed;
    List<Vector3> positions;
    List<Vector3> bossPositions;
    List<float> healthPoints;
    List<float> bossHealth;
    List<float> playerRotation;
    List<float> bossRotation;
    TrailRenderer trail;
    public static float maxSpeed;
    int seconds;
    float lookAngle;
    Vector2 lookDirection;
    Vector3 mouse;
    public GameObject bulletPrefab;
    public GameObject point;
    public static float health;
    GameObject boss;
    public float cooldown;
    public float firerate;
    public static string weapon;
    public List<GameObject> shotgunPoints;
    public static float reflectChance;
    Material greenMat;
    public Material rewindMat;
    public Material enemyMat;
    public AudioSource smg;
    public AudioSource rifle;
    public AudioSource sniper;
    public AudioSource shotgun;
    public AudioSource pistol;
    
    // Start is called before the first frame update
    void Start()
    {
        rewind = false;
        rb = GetComponent<Rigidbody2D>();
        positions = new List<Vector3>();
        bossPositions = new List<Vector3>();
        healthPoints = new List<float>();
        bossHealth = new List<float>();
        playerRotation = new List<float>();
        bossRotation = new List<float>();
        trail = GetComponent<TrailRenderer>();
        boss = GameObject.Find("Enemy");
        cooldown = 10f;
        greenMat = GetComponent<Renderer>().material;
    }
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Level1" && LevelScript.marked == null)
        {
            health = 10f;
            weapon = "Pistol";
            speed = 6000f;
            maxSpeed = 10f;
            reflectChance = 11f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Renderer>().sharedMaterial == enemyMat)
        {
            Invoke("Restore", 0.05f);
        }
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRewind();
        }
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            Stop();
        }
        if (!rewind)
        {
            trail.enabled = false;
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lookDirection = mouse - transform.position;
            lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = lookAngle;
            movement = new Vector3(horizontal, vertical, 0f);
            if(horizontal + vertical > 1f)
            {
                movement.Normalize();
            }
            if (horizontal != 0f)
            {
                if (horizontal == 1f && rb.velocity.x < maxSpeed)
                {
                    rb.AddForce(new Vector3(movement.x * speed * Time.deltaTime, 0f, 0f));
                }
                if (horizontal == -1f && rb.velocity.x > -maxSpeed)
                {
                    rb.AddForce(new Vector3(movement.x * speed * Time.deltaTime, 0f, 0f));
                }
            }
            else
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
            if(vertical != 0f)
            {
                if(vertical == 1f && rb.velocity.y < maxSpeed)
                {
                    rb.AddForce(new Vector3(0f, movement.y * speed * Time.deltaTime, 0f));
                }
                if(vertical == -1f && rb.velocity.y > -maxSpeed)
                {
                    rb.AddForce(new Vector3(0f, movement.y * speed * Time.deltaTime, 0f));
                }
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
            }
            if (Input.GetButton("Fire1"))
            {
                if (firerate <= 0f)
                {
                    
                    if (weapon.Contains("Pistol"))
                    {
                        pistol.Play();
                        GameObject bullet = Instantiate(bulletPrefab, point.transform.position, point.transform.rotation);
                        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 15f, ForceMode2D.Impulse);
                        firerate = 0.8f;
                    }
                    else if (weapon.Contains("Rifle"))
                    {
                        rifle.Play();
                        GameObject bullet = Instantiate(bulletPrefab, point.transform.position, point.transform.rotation);
                        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 19f, ForceMode2D.Impulse);
                        firerate = 0.5f;
                    }
                    else if (weapon.Contains("Sniper"))
                    {
                        sniper.Play();
                        GameObject bullet = Instantiate(bulletPrefab, point.transform.position, point.transform.rotation);
                        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 34f, ForceMode2D.Impulse);
                        firerate = 3f;
                    }
                    else if (weapon.Contains("Shotgun"))
                    {
                        shotgun.Play();
                        foreach(GameObject point in shotgunPoints)
                        {
                            GameObject bullet = Instantiate(bulletPrefab, point.transform.position, point.transform.rotation);
                            bullet.GetComponent<Rigidbody2D>().AddForce(point.transform.up * 16f, ForceMode2D.Impulse);
                            firerate = 1f;

                        }
                    }
                    else if (weapon.Contains("Submachine"))
                    {
                        smg.Play();
                        GameObject bullet = Instantiate(bulletPrefab, point.transform.position, point.transform.rotation);
                        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 14f, ForceMode2D.Impulse);
                        firerate = 0.1f;
                    }

                }
            }
            if(cooldown < 11f)
            {
                cooldown += 1f*Time.deltaTime;
            }
            if(firerate > 0f)
            {
                firerate -= 1f*Time.deltaTime;
            }
            if(Time.timeScale == 2)
            {
                Time.timeScale = 1;
            }
            
        }
        else
        {
            trail.enabled = true;
            Time.timeScale = 2;
        }
        if(health <= 0f)
        {
            Die();
        }
    }
    void FixedUpdate()
    {
        if (rewind)
        {
            Rewind();
        }
        else
        {
            AddToList();
        }
    }
    void StartRewind()
    {
        if (cooldown >= 10f && GameObject.Find("Enemy")!= null)
        {
            GetComponent<Renderer>().sharedMaterial = rewindMat;
            rewind = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
    }
    void Stop()
    {
        if (seconds > 500)
        {
            GetComponent<Renderer>().sharedMaterial = greenMat;
            rewind = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            seconds = 0;
            cooldown = 0f;
        }

    }
    void Rewind()
    {
        seconds += 1;
        if (positions.Count > 0)
        {
            if (GameObject.Find("Enemy") != null)
            {
                boss.transform.position = bossPositions[0];
                bossPositions.RemoveAt(0);
                boss.GetComponent<EnemyScript>().health = bossHealth[0];
                bossHealth.RemoveAt(0);
                boss.GetComponent<Rigidbody2D>().rotation = bossRotation[0];
                bossRotation.RemoveAt(0);
                rb.transform.position = positions[0];
                positions.RemoveAt(0);
                health = healthPoints[0];
                healthPoints.RemoveAt(0);
                rb.rotation = playerRotation[0];
                playerRotation.RemoveAt(0);
            }


        }
        else
        {
            seconds += 500;
        }
    }
    void AddToList()
    {
        if(GameObject.Find("Enemy") != null)
        {
            bossPositions.Insert(0, boss.transform.position);
            bossHealth.Insert(0, boss.GetComponent<EnemyScript>().health);
            bossRotation.Insert(0, boss.GetComponent<Rigidbody2D>().rotation);
            positions.Insert(0, rb.transform.position);
            healthPoints.Insert(0, health);
            playerRotation.Insert(0, rb.rotation);
        }
    }
    void Die()
    {
        
        GameObject.Find("LevelManager").GetComponent<LevelScript>().RewindScene();
        health = 10f;
    }
    void Restore()
    {
        if (!rewind)
        {
            GetComponent<Renderer>().sharedMaterial = greenMat;
        }
    }
}
