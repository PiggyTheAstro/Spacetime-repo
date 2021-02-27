using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public static MusicScript music;
    // Start is called before the first frame update
    void Start()
    {
        if (music != null)
        {
            Destroy(gameObject);
        }
        else
        {
            music = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
        if (PlayerScript.rewind)
        {
            GetComponent<AudioSource>().pitch = -2f;
        }
        else
        {
            GetComponent<AudioSource>().pitch = 1f;
        }
    }
}
