using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        if(gameObject.name == "PlayerHealth")
        {
            slider.minValue = 0f;
            if (PlayerScript.health > 10f)
            {
                slider.maxValue = PlayerScript.health;
            }
            else
            {
                slider.maxValue = 10f;
            }
        }
        else if(gameObject.name == "BossHealth")
        {
            slider.minValue = 0f;
            slider.maxValue = GameObject.Find("Enemy").GetComponent<EnemyScript>().health;
        }
        else if(gameObject.name == "CoolDown")
        {
            slider.minValue = 0f;
            slider.maxValue = 10f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "PlayerHealth")
        {
            slider.value = PlayerScript.health;
        }
        else if(gameObject.name == "BossHealth")
        {
            slider.value = GameObject.Find("Enemy").GetComponent<EnemyScript>().health;
            if(slider.value <= 0f)
            {
                Invoke("NextLevel", 0f);
            }
        }
        else if(gameObject.name == "CoolDown")
        {
            slider.value = GameObject.Find("Player").GetComponent<PlayerScript>().cooldown;
        }
    }
    void NextLevel()
    {
        GameObject.Find("DropsManager").GetComponent<DropScript>().Drop();
    }
}
