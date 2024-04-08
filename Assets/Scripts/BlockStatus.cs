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
    public BlockType blockType;

    public Color Origin_color;
    public bool Hooked;
    public bool Damage;

    // Start is called before the first frame update
    void Start()
    {
        Origin_color = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hooked)
        { GetComponent<SpriteRenderer>().color = Color.red; }
        else
        { GetComponent<SpriteRenderer>().color = Origin_color; }
    }

    private void OnMouseOver()
    {
        Hooked = true;
    }

    private void OnMouseExit()
    {
        Hooked = false;
    }
}
