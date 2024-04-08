using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_SceneEnd : MonoBehaviour
{
    public string NextScene;
    public GameObject EnterPos;

    public Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = EnterPos.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
