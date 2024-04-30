using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFramework : MonoBehaviour
{
    public GameObject CCursor,ObjectText,InCamUI;
    GameObject Cam,FlashL,NoRot,Nokia,Canvas,OfficeTracker;
    Transform Objectives;
    public AudioClip[] Call1;
    AudioSource NokiaSpeaker;
    RaycastHit hit;
    public bool InCameras = false;
    bool CursorOut = false,mDown=false,HasFlashlight=false,NokiaOut=false,GettingCall=false,InCall=false;
    public int Night = 1,ActiveCam=-1;
    public LayerMask RayLayer;
    public Material NokiaNormal, NokiaInCall;
    public int CurrentEvent = 0;
    public GameObject[] AvailableCameras;

    void Awake()
    {
        OfficeTracker = GameObject.Find("OfficeMarker"); OfficeTracker.SetActive(false);
        Cam = transform.GetChild(0).Find("Player Camera").gameObject;
        NoRot = transform.Find("NoRot").gameObject;
        FlashL = NoRot.transform.Find("FlashL").gameObject;
        Nokia = NoRot.transform.Find("nokia").gameObject;
        Canvas = GameObject.Find("Canvas");
        NokiaSpeaker = Nokia.GetComponent<AudioSource>();
        Nokia.SetActive(false);
        Objectives = Canvas.transform.Find("Objectives");
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
        Atext.transform.localScale = ObjectText.transform.localScale;
        Atext.GetComponent<Animation>().Play("ObjectTextAnim");
        Atext.GetComponent<Text>().text = "Equiped " + name;
        Atext.SetActive(true);
        while (Atext.GetComponent<Animation>().isPlaying)   { yield return null; }
        Destroy(Atext.gameObject);
    }

    IEnumerator PhoneReset(int Event)
    {
        while (NokiaSpeaker.isPlaying) { yield return null; }
        Nokia.transform.Find("Main").GetComponent<MeshRenderer>().material = NokiaNormal;
        Nokia.GetComponent<Animation>().Play("ResetPhone");
        InCall = false;
        Nokia.transform.Find("Slider").GetComponent<AudioSource>().Play();
        if(Event==1)
        {
            CurrentEvent = 1;
            Objectives.Find("GTO").GetComponent<Text>().enabled = true;
            OfficeTracker.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Trigger")
        {
            if(other.name == "OfficeTrigger")
            {
                if (CurrentEvent == 1)    {
                    Objectives.Find("GTO").GetComponent<Text>().enabled = false;
                    OfficeTracker.SetActive(false);
                    Nokia.transform.Find("Main").GetComponent<MeshRenderer>().material = NokiaInCall;
                    Cam.transform.Find("Ringtone").GetComponent<AudioSource>().Play();
                    GettingCall = true;
                    CurrentEvent = 2;
                }
            }
        }
    }

    public void RightArrow()
    {
        if(ActiveCam!=AvailableCameras.Length-1)
        {
            SwitchCamera(ActiveCam += 1);
        }
    }
    public void LeftArrow()
    {
        if (ActiveCam != 0)
        {
            SwitchCamera(ActiveCam -= 1);
        }
    }

    void SwitchCamera(int NextCam)
    {
        if(ActiveCam==-1) { Cam.GetComponent<Camera>().enabled = false; Cam.GetComponent<AudioListener>().enabled = false; Debug.Log("DisCam"); }else
        {
            AvailableCameras[ActiveCam].GetComponent<Camera>().enabled = false; AvailableCameras[ActiveCam].GetComponent<AudioListener>().enabled = false;
        }
        ActiveCam = NextCam;
        AvailableCameras[ActiveCam].GetComponent<Camera>().enabled = true; AvailableCameras[ActiveCam].GetComponent<AudioListener>().enabled = true;

    }


    void Mouse(bool UpOrDown)
    {
        if(UpOrDown)
        {
            if(GettingCall && NokiaOut)
            {
                if(Night == 1&&CurrentEvent==0)
                {
                    Nokia.transform.Find("Slider").GetComponent<AudioSource>().Play();
                    Cam.transform.Find("Ringtone").GetComponent<AudioSource>().Stop();
                    Nokia.GetComponent<Animation>().Play("AnswerCallAnim");
                    GettingCall = false;
                    InCall = true;
                    Canvas.transform.Find("PhoneText").GetComponent<Text>().enabled = false;
                    NokiaSpeaker.clip = Call1[0];
                    NokiaSpeaker.Play();
                    StartCoroutine(PhoneReset(1));
                }
                if (Night == 1 && CurrentEvent == 2)
                {
                    Nokia.transform.Find("Slider").GetComponent<AudioSource>().Play();
                    Cam.transform.Find("Ringtone").GetComponent<AudioSource>().Stop();
                    Nokia.GetComponent<Animation>().Play("AnswerCallAnim");
                    GettingCall = false;
                    InCall = true;
                    Canvas.transform.Find("PhoneText").GetComponent<Text>().enabled = false;
                    NokiaSpeaker.clip = Call1[1];
                    NokiaSpeaker.Play();
                    StartCoroutine(PhoneReset(2));
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
                if(iBase.IsCams)
                {
                    if(InCameras==false)
                    {
                        InCameras = true;
                        GetComponent<FirstPersonAIO>().enabled = false;
                        InCamUI.SetActive(true);
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        SwitchCamera(0);   
                    }
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
                    CCursor.GetComponent<Animation>().Play("HilightObject");
                }
            }
            else
            {
                if (CursorOut)
                {
                    CursorOut = false;
                    CCursor.GetComponent<Animation>().Play("UnHilight");
                }
            }
        }
}

    void Update()
    {
        OfficeTracker.transform.LookAt(Cam.transform.position);
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
