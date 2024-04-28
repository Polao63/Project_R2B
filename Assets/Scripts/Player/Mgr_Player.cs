using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_Player : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    [Header("Player Control")]
    public KeyCode key_Jump;
    public KeyCode key_Dash;


    [Header("Player_Status")]
    public bool D_Jump_ON = false;
    public bool Moveable = true;
    public bool canFlip = true;
    public bool isDashing;
    public bool isAttacking = false;
    public bool canDash = true;

    [Header("General_Movement")]
    public bool isFacingL = false;
    public float MoveSpeed = 5;
    public Vector3 MoveVec = Vector2.zero;
    public float jumpForce;

    [Header("Ground_Check")]
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    bool isGrounded;
    public float GroundCheckRadius;
    public bool Jump_Hold = false;

    [Header("Double_Jump")]
    public int D_jumpCount = 0;
    public int D_jumpCountMax = 0;

    [Header("Dashing")]
    public float DashVel;
    public float DashTime;
    public Vector2 DashDir;


    private int animatorHashKey_isRunning = 0;
    private int animatorHashKey_isJumping = 0;
    private int animatorHashKey_isDashing = 0;
    private int animatorHashKey_isFalling = 0;
    private int animatorHashKey_isDJumping = 0;
    private int animatorHashKey_isGrounded = 0;
    private int animatorHashKey_isAttacking = 0;
    private int animatorHashKey_isAttacking2 = 0;


    [Header("Grappling_Hook")]
    Hook_Ctrl GH;

    [Header("Attack")]
    public int attack_Force = 5;
    float attack_Speed = 0;
    public float attack_SpeedMax = 5;
    public bool attack_Delay = false;

    [Header("Special")]
    public bool isUsingSpecial = false;
    public bool specialAirUsed = false;
    public float specialAirUpForce = 0;



    [Header("Wall_Check")]
    public Transform WallCheck;
    public Vector2 WallCheckSize;
    [SerializeField] bool isTouchingWall;


    //private bool isMouseLeftDown = false;
    //private bool isMouseRightDown = false;
    //private float specialInputCheckLimitTime = 0.1f;
    //private float specialInputCheckTime = 0f;
    //private bool wasMouseLeftPressed = false;
    //private bool wasMouseRightPressed = false;


    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        GH = GetComponentInChildren<Hook_Ctrl>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        AnimatorHashSet();
    }
    void AnimatorHashSet()
    {
        animatorHashKey_isRunning = Animator.StringToHash("isRunning");
        animatorHashKey_isJumping = Animator.StringToHash("isJumping");
        animatorHashKey_isDashing = Animator.StringToHash("isDashing");
        animatorHashKey_isFalling = Animator.StringToHash("isFalling");
        animatorHashKey_isDJumping = Animator.StringToHash("isD_Jumping");
        animatorHashKey_isGrounded = Animator.StringToHash("isGrounded");
        animatorHashKey_isAttacking = Animator.StringToHash("isAttacking");
        animatorHashKey_isAttacking2 = Animator.StringToHash("isAttacking2");
    }

    private void Update()
    {
        CheckFlip();
        CheckSurroundings();
        UpdateAnim();


        if (Moveable)
        {
            Movement();
            Jump(key_Jump);
            Dash(key_Dash);
        }
        Special();
        Attack();


    }

    private void FixedUpdate()
    {

    }

    //캐릭터 좌우 반전
    void CheckFlip()
    {
        if (canFlip)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                isFacingL = Input.GetAxisRaw("Horizontal") < 0;
            }

            if (isFacingL)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    //캐릭터 지형충돌 체크
    void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayer);
        isTouchingWall = Physics2D.BoxCast(WallCheck.position, WallCheckSize, 0, Vector2.zero, GroundLayer);

        if (isGrounded)
        {
            D_jumpCount = D_jumpCountMax;
            canDash = true;
            specialAirUsed = false;
        }
    }

    //애니메이션 관리
    void UpdateAnim()
    {
        if (rigid.velocity.y < 0)
        {
            anim.SetBool(animatorHashKey_isFalling, false);
        }

        anim.SetBool(animatorHashKey_isGrounded, isGrounded);
        anim.SetBool(animatorHashKey_isFalling, (rigid.velocity.y < 0));
    }


    //기본적인 이동
    void Movement()
    {
        if (isGrounded)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 && !isTouchingWall)
            {
                anim.SetBool(animatorHashKey_isRunning, true);
            }
            else
            {
                anim.SetBool(animatorHashKey_isRunning, false);
            }
        }

        MoveVec = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (!isDashing && !isTouchingWall)
        {
            transform.position += MoveVec * MoveSpeed * Time.deltaTime;
        }
    }

    //점프
    void Jump(KeyCode Key)
    {
        if (Input.GetKeyDown(Key))
        {
            if (isDashing)
            {
                return;
            }

            if (isGrounded)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
                anim.SetTrigger(animatorHashKey_isJumping);
            }
            else if (D_jumpCount > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
                anim.SetTrigger(animatorHashKey_isDJumping);
                D_jumpCount--;
            }
        }

        /*
        //Jump_Hold = Input.GetKey(KeyCode.Space);

        //if (Input.GetKeyUp(KeyCode.Space) || JumpTime >= JumpTimeCount)
        //{

        //    if (rigid.velocity.y > 0f)
        //    {
        //        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        //    }
        //}


        //if (Input.GetKeyDown(KeyCode.Space) && D_jumpCount > 0)
        //{
        //    if (isGrounded == false)
        //    {
        //        anim.SetBool("isD_Jumping", true);
        //    }
        //    else
        //    {
        //        anim.SetTrigger("isJumping");
        //    }
        //    if (isDashing)
        //    { return; }

        //    rigid.velocity = new Vector2(rigid.velocity.x, 0);
        //    rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
        //    D_jumpCount--;
        //}
        //else if (Input.GetKey(KeyCode.Space) && isGrounded && !Jump_Hold)
        //{

        //    JumpTime += Time.deltaTime;
        //    rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
        //}
        */
    }

    //대시
    void Dash(KeyCode Key)
    {
        if (Input.GetKeyDown(Key) && canDash && !isAttacking)
        {
            anim.SetBool(animatorHashKey_isDashing, true);
            isDashing = true;
            canDash = false;
            DashDir = new Vector2(MoveVec.x, 0);
            if (DashDir == Vector2.zero)
            {
                DashDir = new Vector2(isFacingL ? -1 : 1, 0);
            }
            StartCoroutine(StopDashing());
        }

        if (isDashing)
        {
            rigid.gravityScale = 0;
            rigid.velocity = DashDir.normalized * DashVel;
            return;
        }
    }

    //대시 끝
    IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(DashTime);

        rigid.velocity = Vector2.zero;
        isDashing = false;
        rigid.gravityScale = 5;
        anim.SetBool(animatorHashKey_isDashing, false);
    }

    void Attack()
    {
        if (!isUsingSpecial)
        {
            if (attack_Delay)
            {
                attack_Speed += Time.deltaTime;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                isAttacking = true;
                if (isDashing)
                {
                    anim.SetTrigger(animatorHashKey_isAttacking2);
                    if (isGrounded)
                    {
                        Moveable = false;
                    }
                }
                else
                {
                    anim.SetTrigger(animatorHashKey_isAttacking);

                    if (isGrounded)
                    {
                        Moveable = false;
                    }
                    else
                    {
                        //rigid.velocity = Vector2.zero;
                    }
                }

                canFlip = false;
                attack_Delay = true;
            }

            if (attack_Speed >= attack_SpeedMax)
            {
                attack_Speed = 0;
                attack_Delay = false;
            }
        }
    }

    public void AttackEnd()
    {
        Moveable = true;
        canFlip = true;
        isAttacking = false;
    }

    void Special()
    {
        //isMouseLeftDown = Input.GetMouseButtonDown(0);
        //isMouseRightDown = Input.GetMouseButtonDown(1);
        //if ((isMouseLeftDown || isMouseRightDown) && specialInputCheckTime < specialInputCheckLimitTime)
        //{
        //    specialInputCheckTime += Time.deltaTime;
        //    if (isMouseLeftDown) wasMouseLeftPressed = true;
        //    if (isMouseRightDown) wasMouseRightPressed = true;
        //}
        //else
        //{
        //    wasMouseLeftPressed = false;
        //    wasMouseRightPressed = false;
        //    specialInputCheckTime = 0f;
        //}

        //if (wasMouseLeftPressed && wasMouseRightPressed && !isUsingSpecial)
        //{
        //    // To do : Sepcial Action
        //    Debug.Log("SPECIAL ON");
        //    isUsingSpecial = true;
        //    if (!isGrounded && !specialAirUsed && rigid.velocity.y >= 0)
        //    {
        //        rigid.velocity = Vector2.zero;
        //        rigid.velocity = new Vector2(rigid.velocity.x, specialAirUpForce);
        //        specialAirUsed = true;
        //    }

        //    Invoke(nameof(SpecialEnd), 1f);
        //    //SpecialEnd();
        //}


        if ((Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1)) || Input.GetMouseButtonDown(2))
        {
            Debug.Log("SPECIAL ON");
            isUsingSpecial = true;

            if (!isGrounded && !specialAirUsed && rigid.velocity.y >= 0)
            {
                spriter.color = Color.red;
                rigid.velocity = Vector2.zero;
                rigid.velocity = new Vector2(rigid.velocity.x, specialAirUpForce);
                specialAirUsed = true;
            }
            //Invoke(nameof(SpecialEnd), 1f);
            //SpecialEnd();
        }

        if (isUsingSpecial)
        {
            if ((specialAirUsed && rigid.velocity.y < 0) || isGrounded)
            {
                Debug.Log("SPECIAL OFF");
                SpecialEnd();
            }
        }

        

    }

    public void SpecialEnd()
    {
        spriter.color = Color.white;
        isUsingSpecial = false;
        //wasMouseLeftPressed = false;
        //wasMouseRightPressed = false;
        //specialInputCheckTime = 0f;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Scene_End"))
        {
            string NextScene = collision.gameObject.GetComponent<Mgr_SceneEnd>().NextScene;
            Vector2 NextPosition = collision.gameObject.GetComponent<Mgr_SceneEnd>().pos;

            Mgr_Game.inst.SceneLoad(NextScene, NextPosition);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
        Gizmos.DrawWireCube(WallCheck.position, WallCheckSize);
    }
}
