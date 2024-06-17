using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_Camera : MonoBehaviour
{
    public Mgr_Player Player;
    public float Camera_offset;
    public float Camera_Xset_L;
    public float Camera_Xset_R;
    public float Camera_Yset_U;
    public float Camera_Yset_D;
    

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<Mgr_Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.position.x <= Camera_Xset_L)
        {
            if (Player.transform.position.y >= Camera_Yset_U)
            {
                transform.position = new Vector3(Camera_Xset_L, Camera_Yset_U, Camera_offset);
            }
            else if (Player.transform.position.y <= Camera_Yset_D)
            {
                transform.position = new Vector3(Camera_Xset_L, Camera_Yset_D, Camera_offset);
            }
            else
            {
                transform.position = new Vector3(Camera_Xset_L, Player.transform.position.y, Camera_offset);
            }
        }
        else if (Player.transform.position.x >= Camera_Xset_R)
        {
            if (Player.transform.position.y >= Camera_Yset_U)
            {
                transform.position = new Vector3(Camera_Xset_R, Camera_Yset_U, Camera_offset);
            }
            else if (Player.transform.position.y <= Camera_Yset_D)
            {
                transform.position = new Vector3(Camera_Xset_R, Camera_Yset_D, Camera_offset);
            }
            else
            {
                transform.position = new Vector3(Camera_Xset_R, Player.transform.position.y, Camera_offset);
            }
        }
        else
        {
            if (Player.transform.position.y >= Camera_Yset_U)
            {
                transform.position = new Vector3(Player.transform.position.x, Camera_Yset_U, Camera_offset);
            }
            else if (Player.transform.position.y <= Camera_Yset_D)
            {
                transform.position = new Vector3(Player.transform.position.x, Camera_Yset_D, Camera_offset);
            }
            else
            {
                transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Camera_offset);
            }
        }



        
    }
}
