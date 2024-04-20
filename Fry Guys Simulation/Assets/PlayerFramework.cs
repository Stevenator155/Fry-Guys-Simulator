using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFramework : MonoBehaviour
{
    public GameObject Cursor,ObjectText;
    GameObject Cam,FlashL,NoRot;
    RaycastHit hit;
    bool CursorOut = false,mDown=false,HasFlashlight=false;
    public LayerMask RayLayer;

    void Awake()
    {
        Cam = transform.GetChild(0).Find("Player Camera").gameObject;
        NoRot = transform.Find("NoRot").gameObject;
        FlashL = NoRot.transform.Find("FlashL").gameObject;
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

     void Mouse(bool UpOrDown)
    {
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
