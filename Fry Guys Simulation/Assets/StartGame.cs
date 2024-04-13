using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartGame : MonoBehaviourPunCallbacks
{
    public GameObject Wifi;
    // Start is called before the first frame update


    public void StartTheGame()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        this.transform.Find("Start").gameObject.SetActive(false);
        Wifi.SetActive(true);

    }


    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Wifi.SetActive(false);
        this.GetComponent<Animation>().Play("Started");
    }

    public override void OnJoinedLobby()
    {
        //    SceneManager.LoadScene("ingame");
    }


}
