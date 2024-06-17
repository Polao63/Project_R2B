using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RoomSizeCheck : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(RoomInterval_X, RoomInterval_Y));
    }

    public int RoomSize_X = 1;
    public int RoomSize_Y = 1;
    public float RoomInterval_X = 0;
    public float RoomInterval_Y = 0;

    public GameObject Room_Prefab = null;
    public Borders[] ROOMS;

    private void Awake()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < RoomSize_X; x++)
        {
            for (int y = 0; y < RoomSize_Y; y++)
            {
                GameObject Room_Obj = Instantiate(Room_Prefab,this.transform);
                Room_Obj.transform.position = new Vector2(RoomInterval_X * x, RoomInterval_Y * y);
                Room_Obj.name = $"Room {x} {y}";

                Borders borders = Room_Obj.GetComponent<Borders>();

                if (x != 0)
                {
                    borders.R = false;
                }

                if (x != RoomSize_X - 1)
                {
                    borders.L = false;
                }

                if (y != RoomSize_Y - 1)
                {
                    borders.U = false;
                }

                if (y != 0)
                {
                    borders.D = false;
                }



            }
        }

       
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ROOMS.Length > 0)
        {
            ROOMS = GetComponentsInChildren<Borders>();
        }


        
    }
}
