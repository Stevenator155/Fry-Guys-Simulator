using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class JoinSeeverRoomstuff : MonoBehaviourPunCallbacks
{
    public InputField name;
    public InputField joinn;
    public float RoomCode=0000;
    public Text lbc;
    public Text[] p; 
    public GameObject main, cr,jr,jdr;
    public GameObject[] games;
    public Toggle pb;

    public void hostroom()
    {
        if (name.text != "")
        {
            PhotonNetwork.LocalPlayer.NickName = name.text;
              RoomCode = Random.Range(1000, 9000);
            //  RoomCode = PhotonNetwork.NickName;
          //  RoomOptions roomopts = new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 12 };

            var rps = new ExitGames.Client.Photon.Hashtable();
            rps.Add("n", PhotonNetwork.NickName);
            rps.Add("pb", System.Convert.ToString(pb.isOn));
            rps.Add("ig", "");
            var cps = new string[3];
            cps[0] = "n";
            cps[1] = "pb";
            cps[2] = "ig";
            var ro = new RoomOptions();
            ro.CustomRoomProperties = rps;
            ro.CustomRoomPropertiesForLobby = cps;
            PhotonNetwork.CreateRoom(System.Convert.ToString(RoomCode),ro);
            
            
        }
    }
    public void joinbutton()
    {
        if (name.text != "")
        {
            PhotonNetwork.LocalPlayer.NickName = name.text;
            main.SetActive(false);
            jr.SetActive(true);
        }
    }


    public void joinroom()
    {

        PhotonNetwork.JoinRoom(joinn.text);
    }



    public void StartGame()
    {
        PhotonNetwork.LoadLevel("ingame");
       
    }
    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            jr.SetActive(false);
            jdr.SetActive(true);
        }else
        {
         
            main.SetActive(false);
            cr.SetActive(true);
        }
    }
   public void cancelbutton()
    {

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        main.SetActive(true);
        cr.SetActive(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player Joined");
        for (int i = 0; i < p.Length; i++)
        {
            p[i].text = "";
        }
        Player[] people = PhotonNetwork.PlayerList;
        for(int i = 0; i < people.Length;i++)
        {
            p[i].text = people[i].NickName;
        }
        
    }
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        for(int i=0; i < p.Length;i++)
        {
            p[i].text = "";
        }
        Debug.Log("Player Left");
        Player[] people = PhotonNetwork.PlayerList;
        for (int i = 0; i < people.Length; i++)
        {
            p[i].text = people[i].NickName;
        }

    }
    public void back2()
    {
        jr.SetActive(false);
        main.SetActive(true);
    }

    public void lobbycode()
    {
        lbc.text = System.Convert.ToString(RoomCode);
    }
    public void Cancel()
    {
        PhotonNetwork.LeaveRoom();
        main.SetActive(true);
        jdr.SetActive(false);

    }



    //  public override void OnRoomListUpdate(List<RoomInfo> roomList)
    // {

    ////    for(int i=0;i < games.Length;i++)
    //    {
    //        games[i].GetComponent<JoinPublicRoom>().RoomCode = "";
    //        games[i].SetActive(false);
    //}
    //    print("roomadded");
    // print(roomList.Count);




    //     for (int i = 0; i < roomList.Count; i++)
    //    {

    //       if (roomList[i].PlayerCount > 0)
    //       {
    //   print(roomList[i].CustomProperties["pb"]);
    //    if (System.Convert.ToString(roomList[i].CustomProperties["pb"]) == "True")
    //    {
    //  games[i].SetActive(true);
    //       games[i].transform.GetChild(0).GetComponent<Text>().text = roomList[i].CustomProperties["n"] + "'s Game";
    //games[i].GetComponent<JoinPublicRoom>().RoomCode = roomList[i].Name;
    //}
    //}
    //}
    //}


}
