using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Normal,
    Hookable,
    Breakable,
    Moving,
    Penalty
}

public class BlockStatus : MonoBehaviour
{
    public bool ableToHook = false;

    public Transform GroundSideL;
    public Transform GroundSideR;

    public float Hookup_Offset = 0f;


}
