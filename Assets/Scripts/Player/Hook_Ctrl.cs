using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hook_Ctrl : MonoBehaviour
{
    Mgr_Player Player;

    LineRenderer lineR;

    LayerMask grapplableMask;
    public float maxDistance = 10f;
    public float grappleSpeed = 10f;
    public float grappleShootSpeed = 20f;

    bool isGrappling = false;
    public bool retracting = false;

    public GameObject Cursor;
    public GameObject Hook_End;
    public Vector2 target;
    public Vector2 Pointer;

    public float grappleMaxTime = 10f;
    private float currentGrappleTime = 0f;
    private bool isReverseGrapple = false;

    public float grappling_Delay = 0f;
    float grappling_DelayCur = 0f;

    public AnimationCurve grapplingCurve;
    public float grappleHookMoveTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        lineR = GetComponent<LineRenderer>();
        Player = GetComponentInParent<Mgr_Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //Pointer = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        


        if (retracting)
        {
            Player.Moveable = false;
            Hook_End.transform.localPosition = Vector3.zero;
            Hook_End.GetComponent<HookEnd_Ctrl>().targetReached = false;



            if (grappling_DelayCur >= grappling_Delay)
            {
                if (Hook_End.GetComponent<HookEnd_Ctrl>().Hookable)
                {
                    Player.GetComponent<Rigidbody2D>().gravityScale = 0;
                }

                Vector2 grapplePos = Vector2.Lerp(Player.transform.position, target, grappleSpeed * Time.deltaTime);
                transform.position = grapplePos;

                lineR.SetPosition(0, transform.position);

                grappleHookMoveTime += Time.deltaTime;
                Vector2 direction = (target - (Vector2)Player.transform.position).normalized;
                Vector2 velocity = direction * grapplingCurve.Evaluate(grappleHookMoveTime);

                Player.GetComponent<Rigidbody2D>().velocity = velocity * grappleSpeed;

                if (target.x - Player.transform.position.x != 0)
                {
                    Player.isFacingL = (target.x - Player.transform.position.x) < 0;
                }

                if (Vector2.Distance(Player.transform.position, target) < 0.5f || Input.GetKeyDown(KeyCode.Space))
                {
                    Player.GetComponent<Rigidbody2D>().velocity = Player.GetComponent<Rigidbody2D>().velocity + Vector2.up * Player.jumpForce;
                    //Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    Player.GetComponent<Rigidbody2D>().gravityScale = 5;
                    
                    //Player.D_jumpCount = Player.D_jumpCountMax;
                    Player.Moveable = true;
                    retracting = false;
                    isGrappling = false;
                    lineR.enabled = false;
                    Hook_End.SetActive(false);

                    grappling_DelayCur = 0;
                }
            }
            else 
            { 
                grappling_DelayCur += Time.deltaTime;
            }

            
        }
        else if (isGrappling)
        {
            UpdateGrapple();
            Debug.Log("target : " + target);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E) && !isGrappling)
            {                
                StartGrapple();
            }
        }

    }

    void StartGrapple()
    {
        target = transform.position + (Cursor.transform.position - transform.position).normalized * maxDistance;

        isGrappling = true;
        isReverseGrapple = false;
        currentGrappleTime = 0f;

        var hookEndCtrl = Hook_End.GetComponent<HookEnd_Ctrl>();
        hookEndCtrl.targetReached = false;
        hookEndCtrl.Hookable = false;

        lineR.enabled = true;
        lineR.positionCount = 2;
        lineR.SetPosition(0, transform.position);
        lineR.SetPosition(1, transform.position);
    }

    void UpdateGrapple()
    {
        var hookEndCtrl = Hook_End.GetComponent<HookEnd_Ctrl>();
        Vector2 newPos;
        if (!isReverseGrapple) // 갈고리 이동 & 충돌처리 체크
        {
            Hook_End.transform.localPosition = Vector2.zero;
            Hook_End.SetActive(true);

            newPos = Vector2.Lerp(Player.transform.position, target, currentGrappleTime / grappleMaxTime);
            lineR.SetPosition(0, Player.transform.position);
            lineR.SetPosition(1, newPos);

            Vector2 prevPosition = Hook_End.transform.position;
            Hook_End.transform.position = newPos;

            Vector2 dir = (newPos - prevPosition).normalized;
            float distance = Vector2.Distance(prevPosition, newPos);
            RaycastHit2D hitInfo = Physics2D.Raycast(prevPosition, dir, distance, LayerMask.GetMask("Ground"));
            if (hitInfo.collider != null)
            {
                hookEndCtrl.targetReached = true;
                hookEndCtrl.CheckGrappleDirection(hitInfo.collider, hitInfo.point);
            }

            if (hookEndCtrl.targetReached) //갈고리 발사중 걸렸을때
            {
                

                if (hookEndCtrl.Hookable)
                {
                    target = hookEndCtrl.HookedPos;
                    isGrappling = false;
                    retracting = true;
                    lineR.SetPosition(1, target);
                    grappleHookMoveTime = 0f;
                    return;
                }
                else
                {
                    target = newPos;
                }
            }

            currentGrappleTime += grappleShootSpeed * Time.deltaTime;
            if (currentGrappleTime >= grappleMaxTime)
            {
                isReverseGrapple = true;
                currentGrappleTime = 0f;
            }
        }
        else //갈고리 회수
        {
            target = Hook_End.transform.position;
            newPos = Vector2.Lerp(target, Player.transform.position, currentGrappleTime / grappleMaxTime);
            lineR.SetPosition(0, Player.transform.position);
            lineR.SetPosition(1, newPos);

            currentGrappleTime += grappleShootSpeed * Time.deltaTime;
            if (currentGrappleTime >= grappleMaxTime)
            {
                isGrappling = false;
                lineR.enabled = false;
                Hook_End.transform.localPosition = Vector2.zero;
                Hook_End.SetActive(false);                
            }
        }
    }
}
