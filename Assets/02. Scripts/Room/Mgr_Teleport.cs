using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_Teleport : MonoBehaviour
{

    public Vector2Int TargetRoomPos;
    public string linkedDataKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Assert(!string.IsNullOrEmpty("teleport_start"));
                Mgr_SceneLoad.inst.NextRoom(TargetRoomPos - Player_Status.Cur_RoomPos, "teleport_start");
            }
            
        }


    }
}
