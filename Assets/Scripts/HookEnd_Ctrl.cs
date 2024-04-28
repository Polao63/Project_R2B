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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.GetComponent<Rigidbody2D>().velocity = Mgr_Game.inst.Player.GetComponent<Rigidbody2D>().velocity;

    }

    public void CheckGrappleDirection(Collider2D collider, Vector2 hitPoint)
    {
        var Player = Mgr_Game.inst.Player;
        var Block = collider.gameObject.GetComponent<BlockStatus>();
        Vector2 playerPos = Player.transform.position;

        
        //두 지점의 각도를 비교하면 되지 않을까

        if (collider.gameObject.layer == LayerMask.NameToLayer("Ground") && Block.ableToHook)
        {
            targetReached = true;

            Vector2 GrapplePointL = Block.GroundSideL.position;
            Vector2 GrapplePointR = Block.GroundSideR.position;

            float degreeL = Mathf.Atan2((GrapplePointL - playerPos).y, (GrapplePointL - playerPos).x) * Mathf.Rad2Deg;
            float degreeR = Mathf.Atan2((GrapplePointR - playerPos).y, (GrapplePointR - playerPos).x) * Mathf.Rad2Deg;

            //Debug.Log("degreeL : " + degreeL + " degreeR : " + degreeR);

            if (collider.gameObject.transform.position.x >= Player.transform.position.x)//왼쪽 잡기
            {
                Debug.Log("왼쪽 잡기");
                Debug.Log("Cur Hookrange : " + Mathf.Abs(GrapplePointL.x - hitPoint.x));
                if (Mathf.Abs(GrapplePointL.x - hitPoint.x) <= hookRange)
                {
                    Debug.Log("Cur Angle : " + degreeL);
                    if (Mathf.Abs(degreeL) <= hookAngle)
                    {
                        Hookable = true;
                        //HookedPos = new Vector2(collision.contacts[0].point.x, collision.transform.position.y);
                        HookedPos = GrapplePointL;
                    }
                }
                else Hookable = false;
            }
            if (collider.gameObject.transform.position.x <= Player.transform.position.x)//오른쪽 잡기
            {
                Debug.Log("오른쪽 잡기");
                Debug.Log("Cur Hookrange : " + Mathf.Abs(GrapplePointR.x - hitPoint.x));
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
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    Hookable = false;
    //    targetReached = false;
    //}
    

   
}
