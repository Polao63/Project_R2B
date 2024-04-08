using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class Mgr_Player : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    [Header("Player_Status")]
    public bool D_Jump_ON = false;
    public bool Moveable = true;

    [Header("General_Movement")]
    public bool isFacingL = false;
    public float MoveSpeed = 5;
    public Vector3 MoveVec = Vector2.zero;
    public float jumpForce;

    [Header("Short_Jump")]
    public float JumpTime = 0;
    public float JumpTimeCount = 0;



    [Header("Ground_Check")]
    public LayerMask GroundLayer;
    public Transform G_Checkpoint;
    bool isGrounded;
    public float G_CheckRadius;
    public bool Jump_Hold = false;

    [Header("Double_Jump")]
    public int D_jumpCount = 0;
    public int D_jumpCountMax = 0;

    [Header("Dashing")]
    public float DashVel;
    public float DashTime;
    public Vector2 DashDir;
    public bool isDashing;
    public bool canDash = true;

    [Header("Grappling_Hook")]
    Hook_Ctrl GH;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        GH = GetComponentInChildren<Hook_Ctrl>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(G_Checkpoint.position, G_CheckRadius, GroundLayer);
        Jump_Hold = Input.GetKey(KeyCode.Space);
        

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            isFacingL = Input.GetAxisRaw("Horizontal") < 0;
        }

        spriter.flipX = isFacingL;

        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
            D_jumpCount = D_jumpCountMax;
            canDash = true;
            JumpTime = 0;
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }

        if (rigid.velocity.y < 0)
        {
            anim.SetBool("isD_Jumping", false);
        }
        anim.SetBool("isFalling", (rigid.velocity.y < 0));
        anim.SetBool("isRunning", false);

        if (Moveable)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                anim.SetBool("isRunning", true);
            }
            else anim.SetBool("isRunning", false);

            
            //anim.SetBool("isD_Jumping", false);

            if (Input.GetKeyUp(KeyCode.Space) || JumpTime >= JumpTimeCount)
            {

                if (rigid.velocity.y > 0f)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                }
            }

            if (Input.GetKey(KeyCode.Space) && isGrounded && !Jump_Hold)
            {

                JumpTime += Time.deltaTime;
                rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && D_jumpCount > 0)
            {
                if (isGrounded == false)
                {
                    anim.SetBool("isD_Jumping", true);
                }
                else
                {
                    anim.SetTrigger("isJumping");
                }
                if (isDashing)
                { return; }

                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
                D_jumpCount--;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                anim.SetBool("isDashing", true);
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

            if (Input.GetKeyDown(KeyCode.X) && (Input.GetAxisRaw("Vertical") > 0))
            {
                rigid.velocity = Vector2.zero;
                rigid.velocity = new Vector2(0, 23f);
            }
            

            MoveVec = new Vector2(Input.GetAxis("Horizontal"), 0);
            if(!isDashing) transform.position += MoveVec * MoveSpeed * Time.deltaTime;
        }
        else {
            
        }
    }

    IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(DashTime);

        rigid.velocity = Vector2.zero;
        isDashing = false;
        rigid.gravityScale = 5;
        anim.SetBool("isDashing", false);
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
   
    
}
