using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_Player : MonoBehaviour
{
    Rigidbody2D rigid;
    public SpriteRenderer sprite;
    public Animator anim;
    public PlayerUI_Mgr UI;

    [Header("Player Control")]
    public KeyCode key_Jump;
    public KeyCode key_Dash;


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


    [Header("Double_Jump")]
    
    public float D_JumpInputTime = 0f;
    float D_JumpInputTimeCur = 0;
    public int D_jumpCount = 0;
    public int D_jumpCountMax = 0;

    [Header("Dashing")]
    public float DashVel;
    public float DashTime;
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
            Dash(InputSystem.Key_Dash);
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
        
        if ((rigid.velocity.y <= 0f && rigid.velocity.y <= jumpForce/2) && !isGrounded)
        {
            //Debug.Log("D_JumpTimeCur : " + D_JumpInputTimeCur);
            D_JumpInputTimeCur += Time.deltaTime;
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
            else if(D_JumpInputTimeCur <= D_JumpInputTime && D_jumpCount > 0)
            {
                if (!specialAirUsed)
                {
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
        if (Input.GetKeyDown(Key) && canDash && !isAttacking)
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
            StartCoroutine(StopDashing());
        }

       

        if (isDashing && !isTouchingWall)
        {
            rigid.gravityScale = 0;
            rigid.velocity = DashDir.normalized * DashVel;

            if (isDamaged)
            {
                rigid.velocity = Vector2.zero;
                isDashing = false;
                canFlip = true;
                rigid.gravityScale = 5;
                anim.SetBool(animatorHashKey_isDashing, false);
            }

            return;
        }
    }

    //대시 끝
    IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(DashTime);

        rigid.velocity = Vector2.zero;
        isDashing = false;
        canFlip = true;
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
                if (!isGrounded)
                {
                    rigid.velocity = Vector2.zero;
                }
                attack_Speed += Time.deltaTime;
            }
            else if (InputSystem.Instance.IsPressedPrimaryButton)
            {
                isAttacking = true;
                if (isDashing)
                {
                    isDashAttacking = true;
                    anim.SetTrigger(animatorHashKey_isAttacking2);
                    if (isGrounded)
                    {
                        Moveable = false;
                        canFlip = false;
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

        if (InputSystem.Instance.IsPressedSecondaryButton)
        {
            Debug.Log("SPECIAL ON");
            
            //if (!specialAirUsed)
            //{
            //    isUsingSpecial = true;
            //    anim.SetTrigger(animatorHashKey_isSpecialAttacking);
            //}
            

            //if (!isGrounded && !specialAirUsed && rigid.velocity.y >= 0)

            if(Input.GetKey(key_Jump) && !specialAirUsed && rigid.velocity.y >= 0)
            {
                anim.SetTrigger(animatorHashKey_isSpecialAttacking);
                rigid.velocity = Vector2.zero;
                rigid.velocity = new Vector2(rigid.velocity.x, specialAirUpForce);
                specialAirUsed = true;
            }
            else
            {
                
            }
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
    //특수 기술 끝
    public void SpecialEnd()
    {
        sprite.color = Color.white;
        isUsingSpecial = false;
    }
    //데미지 받았을 때
    public void DamageRecieved(int RecievedDamage = 1, GameObject AttackedEnemy = null)
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
                knockbackVec = new Vector2(-1, 2);
            }
            else if (AttackedEnemy.gameObject.transform.position.x < transform.position.x)
            {
                knockbackVec = new Vector2(1, 2);
            }

            GetComponent<Rigidbody2D>().velocity = knockbackVec * KnockbackForce;
            isKnockbacking = true;

            Physics2D.IgnoreLayerCollision(gameObject.layer, AttackedEnemy.layer,true);
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
        if (collision.gameObject.CompareTag("Scene_End"))
        {
            string NextScene = collision.gameObject.GetComponent<Mgr_SceneEnd>().NextScene;
            Vector2 NextPosition = collision.gameObject.GetComponent<Mgr_SceneEnd>().pos;

            Mgr_Game.inst.SceneLoad(NextScene, NextPosition);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constant.TAG_NAME_ENEMY))
        {
            Enemy_Status Enemy = collision.gameObject.GetComponent<Enemy_Status>();

            if (!isInvincible)
            {
                DamageRecieved(Enemy.AttackDamage, Enemy.gameObject);
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
        Gizmos.DrawWireCube(WallCheck.position, WallCheckSize);
    }
}
