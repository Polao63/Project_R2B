using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Dash,
    D_Jump,
    Hook,
    UpSpecial
}


public class PickUpItem_Ctrl : MonoBehaviour
{
    [Header("Item_Status")]
    public ItemType type;

    private void Awake()
    {
        switch (type)
        {
            case ItemType.Dash:
                if (Player_Status.GotDash)
                {
                    this.gameObject.SetActive(false);
                }
                break;
            case ItemType.D_Jump:
                if (Player_Status.GotD_Jump)
                {
                    this.gameObject.SetActive(false);
                }
                break;
            case ItemType.Hook:
                if (Player_Status.GotHook)
                {
                    this.gameObject.SetActive(false);
                }
                break;
            case ItemType.UpSpecial:
                if (Player_Status.GotUpSpecial)
                {
                    this.gameObject.SetActive(false);
                }
                break;
            default:
                Debug.LogError("Unknown Item Error");
                break;
        }
    }


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
        if (collision.CompareTag(Constant.TAG_NAME_PLAYER))
        {
            Mgr_Player Player = collision.GetComponent<Mgr_Player>();

            switch (type)
            {
                case ItemType.Dash:
                    Player.GotDash = true;
                    break;
                case ItemType.D_Jump:
                    Player.GotD_Jump = true;
                    break;
                case ItemType.Hook: 
                    Player.GotHook = true;
                    break;
                case ItemType.UpSpecial: 
                    Player.GotUpSpecial = true;
                    break;
                default:
                    Debug.LogError("Unknown Item Error");
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}
