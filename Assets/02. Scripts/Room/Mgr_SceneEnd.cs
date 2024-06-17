using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_SceneEnd : MonoBehaviour
{
    public Vector2Int RoomPosAdd;
    public string linkedDataKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject Player_Obj = null;
        if (other.gameObject.CompareTag(Constant.TAG_NAME_PLAYER))
        {
            Player_Obj = other.gameObject;
        }

        if (Player_Obj == null)
        {
            return;
        }
        else
        {
            Debug.Assert(!string.IsNullOrEmpty(linkedDataKey));
            Mgr_SceneLoad.inst.NextRoom(RoomPosAdd, linkedDataKey);
        }

            
    }
}
