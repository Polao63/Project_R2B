using System.Collections;
using System.Collections.Generic;
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

    public GameObject[] PiercedObject;

    // Start is called before the first frame update
    void Start()
    {
        lineR = GetComponent<LineRenderer>();
        Player = GetComponentInParent<Mgr_Player>();
        PiercedObject = GameObject.FindGameObjectsWithTag(Constant.TAG_NAME_ENEMY);
    }

    // Update is called once per frame
    void Update()
    {
        //Pointer = new Vector2(Input.mousePosition.x,Input.mousePosition.y);

        if (retracting)
        {
            var PlayerRigid = Player.GetComponent<Rigidbody2D>();
            var hookEndCtrl = Hook_End.GetComponent<HookEnd_Ctrl>();

            Player.Moveable = false;
            hookEndCtrl.transform.localPosition = Vector3.zero;
            hookEndCtrl.targetReached = false;

            if (grappling_DelayCur >= grappling_Delay)
            {
                if (hookEndCtrl.attachedLayer == LayerMask.NameToLayer(Constant.LAYER_NAME_GROUND))
                {
                    Player.anim.SetTrigger(Player.animatorHashKey_isJumping);

                    if (hookEndCtrl.Hookable)
                    {
                        PlayerRigid.gravityScale = 0;
                    }

                    Vector2 grapplePos = Vector2.Lerp(Player.transform.position, target + Vector2.up, grappleSpeed * Time.deltaTime);
                    transform.position = grapplePos;

                    lineR.SetPosition(0, transform.position);

                    grappleHookMoveTime += Time.deltaTime;
                    Vector2 direction = (target - (Vector2)Player.transform.position).normalized;
                    Vector2 velocity = direction * grapplingCurve.Evaluate(grappleHookMoveTime);

                    Player.Moveable = false;
                    PlayerRigid.velocity = velocity * grappleSpeed;

                    if (Vector2.Distance(Player.transform.position, target) < 0.5f || Input.GetKeyDown(InputSystem.Key_Jump))
                    {
                        PlayerRigid.velocity = Vector2.zero;
                        PlayerRigid.velocity = PlayerRigid.velocity + Vector2.up * Player.jumpForce;
                        PlayerRigid.gravityScale = 5;

                        //Player.D_jumpCount = Player.D_jumpCountMax;
                        Player.Moveable = true;
                        Player.canFlip = true;
                        retracting = false;
                        isGrappling = false;
                        lineR.enabled = false;
                        Hook_End.SetActive(false);

                        grappling_DelayCur = 0;
                    }
                }
                else if (hookEndCtrl.attachedLayer == LayerMask.NameToLayer(Constant.LAYER_NAME_ENEMY))
                {
                    if (!Player.isDashing)
                    {
                        Player.anim.SetTrigger(Player.animatorHashKey_isJumping);

                        if (hookEndCtrl.Hookable)
                        {
                            PlayerRigid.gravityScale = 0;
                        }

                        Vector2 grapplePos = Vector2.Lerp(Player.transform.position, target + Vector2.up, grappleSpeed * Time.deltaTime);
                        transform.position = grapplePos;

                        lineR.SetPosition(0, transform.position);

                        grappleHookMoveTime += Time.deltaTime;
                        Vector2 direction = (target - (Vector2)Player.transform.position).normalized;
                        Vector2 velocity = direction * grapplingCurve.Evaluate(grappleHookMoveTime);

                        Player.Moveable = false;
                        PlayerRigid.velocity = velocity * grappleSpeed;

                        if (Vector2.Distance(Player.transform.position, target) < 0.5f || Input.GetKeyDown(InputSystem.Key_Jump))
                        {
                            PlayerRigid.velocity = Vector2.zero;
                            PlayerRigid.gravityScale = 5;

                            //Player.D_jumpCount = Player.D_jumpCountMax;
                            Player.Moveable = true;
                            Player.canFlip = true;
                            retracting = false;
                            isGrappling = false;
                            lineR.enabled = false;
                            Hook_End.SetActive(false);
                            

                            grappling_DelayCur = 0;
                        }
                    }
                    else
                    {

                    }



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
            //Debug.Log("target : " + target);
        }
        else if (Player.isHookPireceAttacking)
        {
            UpdateGrapplePirece();
        }
        else
        {
            if (!Player.isUsingSpecial)
            {
                if (InputSystem.Instance.IsPressedWheelButton)
                {
                    if (Player.isDashing)
                    {
                        target = Vector2.zero;
                        target = (Vector2)transform.position + (Player.isFacingL ? Vector2.left : Vector2.right) * maxDistance;

                        isReverseGrapple = false;
                        currentGrappleTime = 0f;

                        var hookEndCtrl = Hook_End.GetComponent<HookEnd_Ctrl>();
                        hookEndCtrl.targetReached = false;
                        hookEndCtrl.Hookable = false;

                        lineR.enabled = true;
                        lineR.positionCount = 2;
                        lineR.SetPosition(0, transform.position);
                        lineR.SetPosition(1, transform.position);

                        Player.canFlip = false;
                        Player.isHookPireceAttacking = true;
                    }
                    else if (!isGrappling)
                    {
                        StartGrapple();
                    }

                }
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

        Player.canFlip = false;
        if ((target.x - Player.transform.position.x) != 0)
        {
            if ((target.x - Player.transform.position.x) < 0)
            {
                Player.isFacingL = true;
                Player.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                Player.isFacingL = false;
                Player.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
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

            RaycastHit2D[] hitInfo = Physics2D.RaycastAll(prevPosition, dir, distance);


            for (int idx = 0; idx < hitInfo.Length; idx++)
            {
                if (hitInfo[idx].collider != null && hitInfo[idx].collider.name != Constant.LAYER_NAME_PLAYER)
                {

                    hookEndCtrl.targetReached = true;
                    hookEndCtrl.CheckGrappleDirection(hitInfo[idx].collider, hitInfo[idx].point);
                }
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
            hookEndCtrl.targetReached = false;
            target = Hook_End.transform.position;
            newPos = Vector2.Lerp(target, Player.transform.position, currentGrappleTime / grappleMaxTime);
            lineR.SetPosition(0, Player.transform.position);
            lineR.SetPosition(1, newPos);



            currentGrappleTime += grappleShootSpeed * Time.deltaTime;
            if (currentGrappleTime >= grappleMaxTime)
            {
                isGrappling = false;
                lineR.enabled = false;
                Player.canFlip = true;
                Hook_End.transform.localPosition = Vector2.zero;
                Hook_End.SetActive(false);
            }
        }
    }

    void UpdateGrapplePirece()
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

            //TODO : 레이캐스트 올 해서 오브젝트배열 저장 후 하나하나 데미지 줄때 이미 줬는가를 체크하면 되지 않을까
            RaycastHit2D[] hitInfo = Physics2D.RaycastAll(prevPosition, dir, distance, LayerMask.GetMask(new string[] { Constant.LAYER_NAME_ENEMY }));

            for (int idx = 0; idx < hitInfo.Length; idx++)
            {
                if (hitInfo[idx].collider != null)
                {
                    PiercedObject[idx] = hitInfo[idx].collider.gameObject;
                    if (PiercedObject[idx].GetComponent<Enemy_Status>().isPireceAttacked == false)
                    {
                        hookEndCtrl.CheckGrappleDirection(hitInfo[idx].collider, hitInfo[idx].point);
                        PiercedObject[idx].GetComponent<Enemy_Status>().isPireceAttacked = true;
                    }
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
            hookEndCtrl.targetReached = false;
            target = Hook_End.transform.position;
            newPos = Vector2.Lerp(target, Player.transform.position, currentGrappleTime / grappleMaxTime);
            lineR.SetPosition(0, Player.transform.position);
            lineR.SetPosition(1, newPos);



            currentGrappleTime += grappleShootSpeed * Time.deltaTime;
            if (currentGrappleTime >= grappleMaxTime)
            {
                isGrappling = false;
                lineR.enabled = false;
                Player.canFlip = true;
                Hook_End.transform.localPosition = Vector2.zero;
                Hook_End.SetActive(false);
                Player.isHookPireceAttacking = false;
            }

            for (int idx = 0; idx < PiercedObject.Length; idx++)
            {
                PiercedObject[idx].GetComponent<Enemy_Status>().isPireceAttacked = false;
                PiercedObject[idx].GetComponent<Enemy_Ctrl>().isAttacked = false;
            }
        }
    }
}
