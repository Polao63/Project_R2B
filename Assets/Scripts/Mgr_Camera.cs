using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_Camera : MonoBehaviour
{
    public Transform Player;
    public float Camera_offset;
    public float Camera_Yset;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.position.y >= Camera_Yset)
        {
            transform.position = new Vector3(Player.position.x, Player.position.y, Camera_offset);
        }
        else
        { transform.position = new Vector3(Player.position.x, 0, Camera_offset); }

        
    }
}
