using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookAim_Ctrl : MonoBehaviour
{
    public bool Hooked;
    public GameObject HookedBlock;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hooked = true;
        GameObject Coll_Obj = collision.gameObject;
        if (Coll_Obj.CompareTag("Block"))
        {
            if (Coll_Obj.GetComponent<BlockStatus>().blockType == BlockType.Hookable)
            {
                Coll_Obj.GetComponent<BlockStatus>().Hooked = true;
                HookedBlock = Coll_Obj.gameObject;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        
    }
}
