using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
public class IntroSceneSwitcher : MonoBehaviour
{
    IEnumerator SwtichOnEnd()
    {
        yield return new WaitForSeconds(1f);
        while (this.GetComponent<VideoPlayer>().isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene("Menu");
    }

    private void Awake()
    {
        this.GetComponent<VideoPlayer>().Play();
        StartCoroutine(SwtichOnEnd());
    }
}
