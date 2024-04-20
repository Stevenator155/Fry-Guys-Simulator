using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFramework : MonoBehaviour
{
    public AudioSource music,spooky;
    public Animation H, H1, H2;
    public Animator A, A1, A2;
     IEnumerator StartGame()
    {
        transform.Find("AS").gameObject.SetActive(false);
        transform.Find("fd").GetComponent<Animation>().Play("Fade");
        H.Play();   A.speed = 0;
        H1.Play(); A1.speed = 0;
        H2.Play(); A2.speed = 0;
        while (transform.Find("fd").GetComponent<Animation>().isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene("InGame");
    }

    public void StartStartCoro()
    {
        StartCoroutine(StartGame());
        music.Stop();
        spooky.Play();
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
