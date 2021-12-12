using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    List<Animator> skullDoorAnims;
    List<Animator> ribsDoorAnims;

    private Vector3 start;
    private Vector3 end;
    private Vector3 offest = new Vector3(0, 0, -10);

    private bool moveFlag;
    private float count;

    bool locked;
    bool cleared;

    public GameObject minimapCover;

    void Start()
    {
        minimapCover = this.transform.Find("Canvas/MinimapCover").gameObject;
    }

    void Update()
    {
        if (moveFlag && count <=1.1f)
        {
            count += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(start, end, count);
        }
        else if(count > 1.1f)
        {
            moveFlag = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            start = Camera.main.transform.position;
            end = this.transform.position + offest;
            count = 0;
            moveFlag = true;
            //StartCoroutine("MoveCamera");
            if (minimapCover.activeSelf)
            {
                minimapCover.SetActive(false);
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            start = Camera.main.transform.position;
            end = this.transform.position + offest;
            count = 0;
            moveFlag = false;
            //StartCoroutine("MoveCamera");
        }
    }

    private IEnumerator MoveCamera()
    {
        for (float i = 0f; i <= 1; i += 0.01f)
        {
            Camera.main.transform.position = Vector3.Lerp(start, end, i);
            yield return new WaitForSeconds(.01f);
        }
    }

    public void LockRoom()
    {
        locked = true;

        SetDoorsAnim(skullDoorAnims, false);
        SetDoorsAnim(ribsDoorAnims, false);
    }

    public void RoomCleared()
    {
        cleared = true;
        locked = false;

        SetDoorsAnim(skullDoorAnims, true);
    }

    public void ResetRoom()
    {
        cleared = false;
        locked = false;
        SetDoorsAnim(skullDoorAnims, true);
        SetDoorsAnim(ribsDoorAnims, false);
    }

    public bool isLocked()
    {
        return locked;
    }

    void SetDoorsAnim(List<Animator> animators, bool isOpen)
    {
        foreach (Animator anim in animators)
        {
            anim.SetBool("Open", isOpen);
        }
    }
}
