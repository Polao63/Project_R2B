using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mgr_Game : MonoBehaviour
{
    public static Mgr_Game inst;
    public GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.Find("Player");

        if (Player.transform.position.y < -25f)
        {
            Player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            Player.transform.position = Vector2.zero;
        }
    }


    public void SceneLoad(string NextSceneName,Vector2 NextPos)
    {
        if (NextSceneName == "" || NextSceneName == null)
        {
            return;
        }
        else
        {
            SceneManager.LoadSceneAsync(NextSceneName);
            Player.transform.position = NextPos;
        }
        
    }
}
