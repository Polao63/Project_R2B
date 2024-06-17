using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_Player : MonoBehaviour
{
    Rigidbody2D rigid;
    public SpriteRenderer sprite;
    public Animator anim;
    public Mgr_PlayerUI UI;

    [Header("Player Control")]
    public KeyCode key_Jump;
    public KeyCode key_Dash;

    [Header("Player_GottenItem")]
    public bool GotDash = false;
    public bool GotD_Jump = false;
    public bool GotHook = false;
    public bool GotUpSpecial = false;

    [Header("Player_Status")]
    public int Health = 10;
    public int atk = 0;

    public bool isInvincible = false;

    public bool D_Jump_ON = false;
    public bool Moveable = true;
    public bool canFlip = true;
    public bool isDashing;
    public bool isAttacking = false;
    public bool isDashAttacking = false;
    public bool canDash = true;
    public bool canAirDash = true;


    [Header("General_Movement")]
    public bool isFacingL = false;
    public float MoveSpeed = 5;
    public Vector3 MoveVec = Vector2.zero;
    public float jumpForce;

    public bool jumpMaxHeightReached = false;
    public float jumpTime;
    float jumpTime_Cur = 0;

    [Header("Ground_Check")]
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    bool isGrounded;
    public float GroundCheckRadius;


    [Header("Double_Jump")]
    
    public float D_JumpInputTime = 0f;
    float D_JumpInputTimeCur = 0;
    public int D_jumpCount = 0;
    public int D_jumpCountMax = 0;

    [Header("Dashing")]
    public float DashVel;
    public float DashTime;
    float DashCoolTimeCur;
    public float DashCoolTime;

    public Vector2 DashDir;

    [SerializeField] public int animatorHashKey_isDJumping = 0;
    [SerializeField] public int animatorHashKey_isJumping = 0;

    private int animatorHashKey_isRunning = 0;
    private int animatorHashKey_isDashing = 0;
    private int animatorHashKey_isFalling = 0;
    private int animatorHashKey_isGrounded = 0;
    private int animatorHashKey_isAttacking = 0;
    private int animatorHashKey_isAttacking2 = 0;
    private int animatorHashKey_isSpecialAttacking = 0;
    private int animatorHashKey_isSpecialLooping = 0;
    private int animatorHashKey_isHit = 0;


    [Header("Grappling_Hook")]
    Hook_Ctrl GH;
    public bool isHookPireceAttacking = false;

    [Header("Attack")]
    public int attack_Force = 5;
    float attack_Speed = 0;
    public float attack_SpeedMax = 5;
    public bool attack_Delay = false;
    //public bool air_Attacked = false;


    [Header("Special")]
    public bool isUsingSpecial = false;
    public bool specialAirUsed = false;
    public float specialAirUpForce = 0;

    [Header("Damage")]
    public bool isDamaged = false;
    public float invincibleTime = 0;
    float invincibleTime_Cur = 0;
    public float KnockbackForce = 5f;
    bool isKnockbacking = false;
    public float knockbackTime = 0;
    float knockbackTime_Cur = 0;

    [Header("Wall_Check")]
    public Transform WallCheck;
    public Vector2 WallCheckSize;
    [SerializeField] bool isTouchingWall;

    
    



    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        GH = GetComponentInChildren<Hook_Ctrl>();
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
        animatorHashKey_isSpecialAttacking = Animator.StringToHash("isSpecialAttacking");
        animatorHashKey_isSpecialLooping = Animator.StringToHash("isSpecialLooping");
        animatorHashKey_isHit = Animator.StringToHash("isHit");
    }

    private void Update()
    {
        
        CheckSurroundings();
        UpdateAnim();

        InvincibleTimeCtrl();


        if (Moveable)
        {
            Movement();
            Jump(InputSystem.Key_Jump);

            if (!isUsingSpecial && !isAttacking)
            {
                if (GotDash)
                {
                    Dash(InputSystem.Key_Dash);
                }
            }
            
        }
        Special();
        Attack();

        if (isKnockbacking)
        {
            Moveable = false;
            canFlip = false;
            knockbackTime_Cur += Time.deltaTime;


            if (knockbackTime_Cur >= knockbackTime || rigid.velocity.y == 0)
            {
                knockbackTime_Cur = 0;
                isKnockbacking = false;
                Moveable = true;
                canFlip = true;
            }
        }

        CheckFlip();


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
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
                
        }
    }

    //캐릭터 지형충돌 체크
    void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayer);
        isTouchingWall = Physics2D.BoxCast(WallCheck.position, WallCheckSize, 0, Vector2.zero, 0,GroundLayer);

        if (isGrounded)
        {
            D_jumpCount = D_jumpCountMax;
            if (!canAirDash)
            {
                canDash = true;
                canAirDash = true;
            }

            if (!canDash)
            {
                DashCoolTimeCur += Time.deltaTime;
                if (DashCoolTimeCur >= DashCoolTime)
                {
                    canDash = true;
                    DashCoolTimeCur = 0;
                }
            }

            if (specialAirUsed)
            {
                specialAirUsed = false;
                isUsingSpecial = false;
            }
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
        if ((rigid.velocity.y >= 0f && rigid.velocity.y <= jumpForce / 2) && !isGrounded)
        {
            //Debug.Log("D_JumpTimeCur : " + D_JumpInputTimeCur);
            D_JumpInputTimeCur += Time.deltaTime;
        }

        if (Mathf.Abs(rigid.velocity.y) <= 1 && !isGrounded)
        {
            jumpMaxHeightReached = true;
        }
            
        if (jumpTime_Cur < jumpTime)
        {
            if (jumpMaxHeightReached)
            {
                jumpTime_Cur += Time.deltaTime;
                rigid.gravityScale = 1;
            }
            
        }
        if(jumpTime_Cur >= jumpTime && jumpMaxHeightReached || isGrounded)
        {
            rigid.gravityScale = 5;
            jumpTime_Cur = 0;
            jumpMaxHeightReached = false;
        }

        if (Input.GetKeyDown(Key))
        {
            if (isDashing)
            {
                return;
            }

            if (isGrounded)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
                D_JumpInputTimeCur = 0;
                anim.SetTrigger(animatorHashKey_isJumping);
            }

            //else if (D_jumpCount > 0)
            //{
            //    if (!specialAirUsed)
            //    {
            //        rigid.velocity = new Vector2(rigid.velocity.x, 0);
            //        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            //        anim.SetTrigger(animatorHashKey_isDJumping);
            //        D_jumpCount--;
            //    }
            //}

            else if (D_JumpInputTimeCur <= D_JumpInputTime && D_jumpCount > 0)
            {
                if (!specialAirUsed && GotD_Jump)
                {
                    rigid.gravityScale = 5;
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
                    anim.SetTrigger(animatorHashKey_isDJumping);
                    D_jumpCount--;
                }
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
        if (Input.GetKeyDown(Key) && canDash && canAirDash)
        {
            anim.SetBool(animatorHashKey_isDashing, true);
            isDashing = true;
            canDash = false;
            canFlip = false;
            DashDir = new Vector2(MoveVec.x, 0);
            if (DashDir == Vector2.zero)
            {
                DashDir = new Vector2(isFacingL ? -1 : 1, 0);
            }
            if (!isGrounded && canAirDash)
            {
                canAirDash = false;
            }

            StartCoroutine(StopDashing());
        }
        else if (Input.GetKeyUp(Key))
        {
            //rigid.velocity = Vector2.zero;
            //isDashing = false;
            //canFlip = true;
            //rigid.gravityScale = 5;
            //anim.SetBool(animatorHashKey_isDashing, false);
        }


       

        if (isDashing && !isTouchingWall)
        {
            rigid.gravityScale = 0;
            rigid.velocity = DashDir.normalized * DashVel;

            return;
        }
    }

    //대시 끝
    IEnumerator StopDashing()
    {
        if (!isDamaged && !isDashAttacking)
        {
            yield return new WaitForSeconds(DashTime);
        }

        rigid.velocity = Vector2.zero;
        isDashing = false;
        canFlip = true;
        //canDash = true;
        rigid.gravityScale = 5;
        
        anim.SetBool(animatorHashKey_isDashing, false);
    }
    //공격
    void Attack()
    {
        if (!isUsingSpecial)
        {
            if (attack_Delay)
            {
                attack_Speed += Time.deltaTime;
            }
            else if (InputSystem.Instance.IsPressedPrimaryButton)
            {
                isAttacking = true;
                //대시공격
                if (isDashing)
                {
                    isDashAttacking = true;
                    anim.SetTrigger(animatorHashKey_isAttacking2);

                    rigid.velocity = Vector2.zero;
                    isDashing = false;
                    canFlip = true;

                    if (isGrounded)
                    {
                        Moveable = false;
                        canFlip = false;
                    }
                    else
                    {
                        rigid.velocity = Vector2.zero;
                    }
                }
                //일반공격
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
    //공격 끝
    public void AttackEnd()
    {
        Moveable = true;
        canFlip = true;
        isAttacking = false;
        isDashAttacking = false;
    }
    //특수 기술
    void Special()
    {

        if (InputSystem.Instance.IsPressedSecondaryButton && !isUsingSpecial)
        {
            Debug.Log("SPECIAL ON");
            

            //공중 특수 공격
            if (Input.GetKey(key_Jump) && !specialAirUsed && !isDashing)
            {
                if (GotUpSpecial)
                {
                    anim.SetTrigger(animatorHashKey_isSpecialAttacking);
                    isUsingSpecial = true;
                    specialAirUsed = true;
                    rigid.velocity = Vector2.zero;
                    rigid.velocity = new Vector2(rigid.velocity.x, specialAirUpForce);
                }
            }
            //대시 특수 공격
            else if(Input.GetKey(key_Dash))
            {

            }           
        }    

        if (isUsingSpecial)
        {
            
        }        
    }

    //특수 기술 끝
    public void SpecialEnd()
    {
        isUsingSpecial = false;
        specialAirUsed = false;
    }

    //데미지 받았을 때
    public void DamageRecieved(int RecievedDamage = 1, GameObject AttackedEnemy = null, bool KnockbackOn = false)
    {
        UI.DamageTextShow(RecievedDamage);


        Health -= RecievedDamage;
        isInvincible = true;
        isDamaged = true;
        anim.SetTrigger(animatorHashKey_isHit);


        Vector2 knockbackVec = Vector2.zero;

        if (AttackedEnemy != null)
        {
            if (AttackedEnemy.transform.position.x > transform.position.x)
            {
                knockbackVec = new Vector2(-1, 1.5f);
            }
            else if (AttackedEnemy.gameObject.transform.position.x < transform.position.x)
            {
                knockbackVec = new Vector2(1, 1.5f);
            }

            if (KnockbackOn)
            {
                rigid.velocity = Vector2.zero;
                rigid.velocity = knockbackVec * KnockbackForce;
                isKnockbacking = true;
            }

            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(Constant.LAYER_NAME_ENEMY), true);
        }
        else
        {
            Debug.LogError("AttackEnemyNullError!");
        }

        
    }

    void InvincibleTimeCtrl()
    {
        if (isInvincible) 
        {
            isDamaged = false;
            invincibleTime_Cur += Time.deltaTime;
            sprite.color = new Color(1, 1, 1, 0.5f);
        }

        if (invincibleTime_Cur >= invincibleTime)
        {
            invincibleTime_Cur = 0;
            isInvincible = false;
            sprite.color = new Color(1, 1, 1, 1);
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(Constant.LAYER_NAME_ENEMY), false);
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constant.TAG_NAME_ENEMY))
        {
            Enemy_Status Enemy = collision.gameObject.GetComponent<Enemy_Status>();

            if (!isInvincible)
            {
                isDamaged = true;

                if (isDashing)
                {
                    StartCoroutine(StopDashing());
                }

                DamageRecieved(Enemy.AttackDamage, Enemy.gameObject, true);
            }

        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constant.TAG_NAME_ENEMY))
        {
            Enemy_Status Enemy = collision.gameObject.GetComponent<Enemy_Status>();

            if (!isInvincible)
            {
                isDamaged = true;


                DamageRecieved(Enemy.AttackDamage, Enemy.gameObject, true);
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
        Gizmos.DrawWireCube(WallCheck.position, WallCheckSize);
    }
}
