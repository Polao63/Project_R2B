using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookEnd_Ctrl : MonoBehaviour
{
    public bool Hookable = false;
    public float hookRange = 0;
    public Vector2 HookedPos = Vector2.zero;
    public float hookAngle = 0;
    public bool targetReached = false;
    public GameObject HookedObject = null;

    public int attachedLayer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckGrappleDirection(Collider2D collider, Vector2 hitPoint)
    {
        var Player = Mgr_Game.inst.Player.GetComponent<Mgr_Player>();
        var Block = collider.gameObject.GetComponent<BlockStatus>();
        var Enemy = collider.gameObject.GetComponent<Enemy_Ctrl>();
        Vector2 playerPos = Player.transform.position;

        attachedLayer = collider.gameObject.layer;


        if (collider.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_GROUND) && Block.ableToHook)
        {
            Debug.Log("Ground");
            targetReached = true;
            attachedLayer = collider.gameObject.layer;

            Vector2 GrapplePointL = Block.GroundSideL.position;
            Vector2 GrapplePointR = Block.GroundSideR.position;

            float degreeL = Mathf.Atan2((GrapplePointL - playerPos).y, (GrapplePointL - playerPos).x) * Mathf.Rad2Deg;
            float degreeR = Mathf.Atan2((GrapplePointR - playerPos).y, (GrapplePointR - playerPos).x) * Mathf.Rad2Deg;

            //Debug.Log("degreeL : " + degreeL + " degreeR : " + degreeR);

            //왼쪽 잡기
            if (collider.gameObject.transform.position.x >= Player.transform.position.x)
            {
                Debug.Log("왼쪽 잡기");
                //Debug.Log("Cur Hookrange : " + Mathf.Abs(GrapplePointL.x - hitPoint.x));
                if (Mathf.Abs(GrapplePointL.x - hitPoint.x) <= hookRange)
                {
                    Debug.Log("Cur Angle : " + (180 - degreeL));
                    if (Mathf.Abs(degreeL) <= 180 - hookAngle)
                    {
                        Hookable = true;
                        //HookedPos = new Vector2(collision.contacts[0].point.x, collision.transform.position.y);
                        HookedPos = GrapplePointL;
                    }
                }
                else Hookable = false;
            }
            //오른쪽 잡기
            if (collider.gameObject.transform.position.x <= Player.transform.position.x)
            {
                Debug.Log("오른쪽 잡기");
                //Debug.Log("Cur Hookrange : " + Mathf.Abs(GrapplePointR.x - hitPoint.x));
                if (Mathf.Abs(GrapplePointR.x - hitPoint.x) <= hookRange)
                {
                    Debug.Log("Cur Angle : " + degreeR);
                    if (Mathf.Abs(degreeR) >= hookAngle)
                    {
                        Hookable = true;
                        //HookedPos = new Vector2(collision.contacts[0].point.x, collision.transform.position.y);
                        HookedPos = GrapplePointR;
                    }
                }
                else Hookable = false;
            }
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_ENEMY))
        {
            if (!Player.isHookPireceAttacking)
            {
                Hookable = true;
                HookedPos = transform.position;
                HookedObject = collider.gameObject;
            }
            else
            {
                Enemy.isPierceCalled = true;
                Enemy.isAttacked = true;
            }
        }

    }

 
   
}
