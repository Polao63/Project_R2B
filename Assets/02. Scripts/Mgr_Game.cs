using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_Game : MonoBehaviour
{
    public static Mgr_Game inst;
    public GameObject Player;
    public CinemachineConfiner2D Virtual_Camera;

    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Player = GameObject.Find("Player");
        if (Player_Status.IsLoaded)
        { 
            Player.transform.position = Player_Status.LastLoadPosition; 
            Player_Status.IsLoaded = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.Find("Player");

        //if (Player.transform.position.y < -25f)
        //{
        //    Player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        //    Player.transform.position = Vector2.zero;
        //}
    }


    
}
