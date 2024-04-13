using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartingMenu : MonoBehaviour
{
    VideoPlayer Opening;

    // Start is called before the first frame update
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Opening = this.transform.Find("StartAnimation").GetComponent<VideoPlayer>();
        Opening.Play();
        Opening.loopPointReached += OpeningEnded;
    }

    void OpeningEnded(VideoPlayer Vidp)
    {
        Opening.gameObject.SetActive(false);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
