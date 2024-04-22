using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFramework : MonoBehaviour
{
    public GameObject Cursor,ObjectText;
    GameObject Cam,FlashL,NoRot,Nokia,Canvas;
    public AudioClip[] Call1;
    AudioSource NokiaSpeaker;
    RaycastHit hit;
    bool CursorOut = false,mDown=false,HasFlashlight=false,NokiaOut=false,GettingCall=false,InCall=false;
    public int Night = 1;
    public LayerMask RayLayer;
    public Material NokiaNormal, NokiaInCall;

    void Awake()
    {
        Cam = transform.GetChild(0).Find("Player Camera").gameObject;
        NoRot = transform.Find("NoRot").gameObject;
        FlashL = NoRot.transform.Find("FlashL").gameObject;
        Nokia = NoRot.transform.Find("nokia").gameObject;
        Canvas = GameObject.Find("Canvas");
        NokiaSpeaker = Nokia.GetComponent<AudioSource>();
        Nokia.SetActive(false);
    }

    IEnumerator FirstPhoneCall()
    {
        yield return new WaitForSeconds(8);
        Nokia.transform.Find("Main").GetComponent<MeshRenderer>().material = NokiaInCall;
        Cam.transform.Find("Ringtone").GetComponent<AudioSource>().Play();
        Canvas.transform.Find("PhoneText").GetComponent<Text>().enabled = true;
        GettingCall = true;
    }

    IEnumerator Debris(float Time,GameObject _object)
    {
        yield return new WaitForSeconds(Time);
        Destroy(_object);
    }

    IEnumerator EquipedNewItem(string name)
    {
        GameObject Atext = Instantiate(ObjectText);
        Atext.transform.parent = GameObject.Find("Canvas").transform;
        Atext.transform.position = ObjectText.transform.position;
        Atext.GetComponent<Animation>().Play("ObjectTextAnim");
        Atext.GetComponent<Text>().text = "Equiped " + name;
        Atext.SetActive(true);
        while (Atext.GetComponent<Animation>().isPlaying)   { yield return null; }
        Destroy(Atext.gameObject);
    }

    IEnumerator PhoneReset()
    {
        while (NokiaSpeaker.isPlaying) { yield return null; }
        Nokia.transform.Find("Main").GetComponent<MeshRenderer>().material = NokiaNormal;
        Nokia.GetComponent<Animation>().Play("ResetPhone");
        InCall = false;
        Nokia.transform.Find("Slider").GetComponent<AudioSource>().Play();
    }

     void Mouse(bool UpOrDown)
    {
        if(UpOrDown)
        {
            if(GettingCall && NokiaOut)
            {
                if(Night == 1)
                {
                    Nokia.transform.Find("Slider").GetComponent<AudioSource>().Play();
                    Cam.transform.Find("Ringtone").GetComponent<AudioSource>().Stop();
                    Nokia.GetComponent<Animation>().Play("AnswerCallAnim");
                    GettingCall = false;
                    InCall = true;
                    Canvas.transform.Find("PhoneText").GetComponent<Text>().enabled = false;
                    NokiaSpeaker.clip = Call1[0];
                    NokiaSpeaker.Play();
                    StartCoroutine(PhoneReset());
                }
            }
        }

        if (!UpOrDown)
        {
            if (hit.transform.tag == "Interactable" && hit.transform.GetComponent<InteractableBase>()&&CursorOut)
            {
                InteractableBase iBase = hit.transform.GetComponent<InteractableBase>();
                if(iBase.playSound)
                {
                    GameObject snd = new GameObject("InteractSound");
                    snd.AddComponent<AudioSource>(); snd.GetComponent<AudioSource>().clip = iBase.GetComponent<AudioSource>().clip;
                    snd.transform.position = iBase.transform.position;
                    snd.GetComponent<AudioSource>().Play();
                    StartCoroutine(Debris(5, snd));
                }
                if(iBase.IsFlashlight)
                {
                    HasFlashlight = true;
                    Destroy(GameObject.Find("StartBarrier"));
                    StartCoroutine(FirstPhoneCall());
                }
                if (iBase.IsLever)
                {
                    GameObject MainDoor = iBase.transform.parent.parent.gameObject;
                    if(MainDoor.GetComponent<SecurityDoor>().Closed)
                    {
                        MainDoor.GetComponent<SecurityDoor>().OpenDoor();

                    }else
                    {
                        MainDoor.GetComponent<SecurityDoor>().CloseDoor();
                    }
                }
                if(iBase.doesequip)
                {
                    StartCoroutine(EquipedNewItem(iBase.transform.name));
                }
                if(iBase.destroy)
                {
                    Destroy(iBase.gameObject);
                }
            }
        }
    }

    void CursorController()
    {
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, 500, RayLayer))
        {
            if (hit.transform.tag == "Interactable" && (hit.transform.position - Cam.transform.position).magnitude < 2.5f)
            {
                if (!CursorOut)
                {
                    CursorOut = true;
                    Cursor.GetComponent<Animation>().Play("HilightObject");
                }
            }
            else
            {
                if (CursorOut)
                {
                    CursorOut = false;
                    Cursor.GetComponent<Animation>().Play("UnHilight");
                }
            }
        }
}

    void Update()
    {
        NoRot.transform.rotation = Quaternion.identity;
        FlashL.transform.rotation = Quaternion.Lerp(FlashL.transform.rotation, Cam.transform.rotation,8f*Time.deltaTime);
        Nokia.transform.rotation = Quaternion.Lerp(Nokia.transform.rotation, transform.rotation, 10f * Time.deltaTime);
        if(Input.GetKeyDown("x"))
        {
            if(NokiaOut)
            { if (!InCall)  {
                    Cam.transform.Find("PhoneEquip").GetComponent<AudioSource>().Play();
                    NokiaOut = false;
                    Nokia.SetActive(false); }
            } else { NokiaOut = true; Nokia.SetActive(true); Cam.transform.Find("PhoneEquip").GetComponent<AudioSource>().Play(); }
        }


        if (Input.GetKeyDown("f") && HasFlashlight)
        {
            if (FlashL.GetComponent<Light>().enabled)
            {
                FlashL.GetComponent<Light>().enabled = false;
                FlashL.GetComponent<AudioSource>().Play();
            }
            else
            {
                FlashL.GetComponent<Light>().enabled = true;
                FlashL.GetComponent<AudioSource>().Play();
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (!mDown) { mDown = true; Mouse(true); }
        }
        else if (mDown) { mDown = false; Mouse(false); }

        CursorController();
    }
}
