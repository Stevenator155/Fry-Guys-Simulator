using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFramework : MonoBehaviour
{
     IEnumerator StartGame()
    {
        transform.Find("AS").gameObject.SetActive(false);
        transform.Find("fd").GetComponent<Animation>().Play("Fade");
        while (transform.Find("fd").GetComponent<Animation>().isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene("InGame");
    }

    public void StartStartCoro()
    {
        StartCoroutine(StartGame());
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
