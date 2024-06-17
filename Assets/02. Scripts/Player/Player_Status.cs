using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Status
{
    public static bool IsLoaded = false;
    public static Vector2 LastLoadPosition = Vector2.zero;
    public static Vector2Int Cur_RoomPos = Vector2Int.zero;

    public static int Level = 1;
    public static int Exp = 0;
    public static int HP_Max = 100;
    public static int HP_Cur = 100;

    public static int MP_Max = 100;
    public static int MP_Cur = 100;

    public static string Name = "Player";

    public static bool GotDash = false;
    public static bool GotD_Jump = false;
    public static bool GotHook = false;
    public static bool GotUpSpecial = false;

 
}
