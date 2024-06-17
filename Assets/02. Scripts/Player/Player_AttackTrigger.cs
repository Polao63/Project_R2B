using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    None,
    Upper,
}

public class Player_AttackTrigger : MonoBehaviour
{
    Mgr_Player Player;

    public float knockbackForce = 0;
    public float knockbackForce_Dash = 0;
    public Vector2 knockbackVec = Vector2.zero;

    public bool IsSpecial = false;
    public SkillType SkillType;


    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponentInParent<Mgr_Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Constant.TAG_NAME_ENEMY)
        {
            Enemy_Ctrl enemy = collision.gameObject.GetComponent<Enemy_Ctrl>();

            enemy.Status.HealthPoint -= Mgr_Game.inst.Player.GetComponent<Mgr_Player>().atk;


            if (enemy.transform.position.x < transform.position.x)
            {
                if (enemy.Status.HealthPoint <= 0)
                {
                    knockbackVec = new Vector2(-1, 1) * 3;
                }
                else
                {
                    knockbackVec = new Vector2(-1, 1);
                }

            }
            else if (enemy.transform.position.x > transform.position.x)
            {
                if (enemy.Status.HealthPoint <= 0)
                {
                    knockbackVec = Vector2.one * 3;
                }
                else
                {
                    knockbackVec = Vector2.one;
                }
                
            }

            if (enemy.KnockbackOn)
            {
                if (Player.isDashAttacking)
                {
                    enemy.rigid.velocity = knockbackVec * knockbackForce_Dash;
                }
                else
                {
                    enemy.rigid.velocity = knockbackVec * knockbackForce;
                }
               
            }

            enemy.isAttacked = true;
            
        }
    }
}
