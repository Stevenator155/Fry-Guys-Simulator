using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityDoor : MonoBehaviour
{
    public AudioClip Close, Open;
    public GameObject Lever, Door;
    public bool Closed = false;
    StressReceiver ChrSR;
    public Material Green, Red;

    private void Start()
    {
        ChrSR = GameObject.Find("Character").transform.GetChild(0).GetChild(0).GetComponent<StressReceiver>();
    }

    IEnumerator Sparks()
    {
        while (GetComponent<Animation>().isPlaying) {yield return null;}
        ChrSR.InduceStress(1);
        transform.Find("sparks").GetComponent<ParticleSystem>().Play();
    }

   public void CloseDoor()
    {
        Lever.transform.Find("LEDSocket_Baked").GetComponent<MeshRenderer>().material = Green;
        GetComponent<Animation>().Play("DoorClose");
        Lever.GetComponent<Animation>().Play("LeverDown");
        Lever.GetComponent<AudioSource>().Play();
        Door.GetComponent<AudioSource>().clip = Close;
        Closed = true;
        Door.GetComponent<AudioSource>().Play();
        StartCoroutine(Sparks());
    }
    public void OpenDoor()
    {
        Lever.transform.Find("LEDSocket_Baked").GetComponent<MeshRenderer>().material = Red;
        Lever.GetComponent<Animation>().Play("LeverUp");
        GetComponent<Animation>().Play("DoorOpen");
        Closed = false;
        Lever.GetComponent<AudioSource>().Play();
        Door.GetComponent<AudioSource>().clip = Open;
        Door.GetComponent<AudioSource>().Play();
    }
}
