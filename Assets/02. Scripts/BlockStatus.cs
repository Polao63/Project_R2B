using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Normal,
    Breakable,
    Control,
    Moving,
    Penalty
}

public enum BreakSkill
{
    None,
    Upper,
}

public class BlockStatus : MonoBehaviour
{
    public BlockType Type;
    public Collider2D Collider;
    public Collider2D Trigger;

    [Header("HOOK")]
    public bool ableToHook = false;
    public Transform GroundSideL;
    public Transform GroundSideR;


    [Header("Breakable")]
    public bool Req_Skill = true;
    public BreakSkill Skill;

    [Header("Control")]
    public bool isSwitch = false;

    [Header("Moving")]
    public float MoveSpeed;
    public GameObject MovePointRoot;
    public Transform[] MovePointList;
    public bool MoveOpposite = false;
    public int nextIdx = 1;

    Vector3 m_DirVec;
    GameObject Target_Obj = null;//타겟 참조 변수
    Vector3 m_DesiredDir; //타겟을 향하는 방향 변수

    [Header("Penalty")]
    public bool isLiquid;
    public int Damage = 1;

    float a = 0f;
    void MoveWayPoint()
    {
        

        MovePointList = MovePointRoot.GetComponentsInChildren<Transform>();

        Vector2 NextPos = Vector2.Lerp((Vector2)MovePointList[1].localPosition, (Vector2)MovePointList[2].localPosition, MoveSpeed * (a += Time.deltaTime));
        transform.position = NextPos;



        //Vector2 a_CacVec = (Vector2)MovePointList[nextIdx].position - (Vector2)transform.position;
        //m_DirVec = a_CacVec;

        //if (-1f >= a_CacVec.y)
        //{
        //    float angle = Mathf.Atan2(a_CacVec.x, -a_CacVec.y) * Mathf.Rad2Deg;
        //    Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        //    transform.rotation = angleAxis;
        //}
        //m_DirVec = transform.up;
        //transform.Translate(Vector2.up * MoveSpeed * Time.deltaTime, Space.Self);

        Debug.Log(Vector2.Distance(transform.position, MovePointList[nextIdx].position));

        if (Vector2.Distance(transform.position, MovePointList[nextIdx].position) < 0.1f)
        {
            a = 0f;

            if (!MoveOpposite)
            {
                

                if (nextIdx < MovePointList.Length-1)
                {
                    nextIdx++;
                }
                else
                {
                    MoveOpposite = true;
                }
            }
            else
            {
                

                if (nextIdx > 1)
                {
                    nextIdx--;
                }
                else
                {
                    MoveOpposite = false;
                }
            }
        }

    }

    private void Update()
    {
        if (isLiquid)
        {
            this.gameObject.layer = LayerMask.NameToLayer(Constant.LAYER_NAME_LIQUID);
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer(Constant.LAYER_NAME_GROUND);
        }

        switch (Type)
        {
            case BlockType.Normal:
                break;
            case BlockType.Breakable:
                break;
            case BlockType.Control:
                break;
            case BlockType.Moving:
                MoveWayPoint();
                break;
            case BlockType.Penalty:
                Collider.enabled = !isLiquid;
                Trigger.enabled = isLiquid;
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(Type == BlockType.Penalty)
        {
            if (!isLiquid)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_PLAYER))
                {
                    Mgr_Player Player = collision.gameObject.GetComponent<Mgr_Player>();

                    if (!Player.isInvincible)
                    {
                        Player.DamageRecieved(Damage, this.gameObject, false);
                    }
                }
            }
          
        }  
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Type == BlockType.Breakable)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_PLAYER_ATTACKTRIGGER))
            {
                Player_AttackTrigger Attack = collision.gameObject.GetComponent<Player_AttackTrigger>();

                if (Req_Skill && Attack.IsSpecial && Attack.SkillType != SkillType.None)
                {
                    if (Attack.SkillType.ToString() == Skill.ToString())
                    {
                        Destroy(this.gameObject);
                    }
                }
                else if(!Req_Skill)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        if (isLiquid && Type == BlockType.Penalty)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_PLAYER))
            {
                Mgr_Player Player = collision.gameObject.GetComponent<Mgr_Player>();

                if (!Player.isInvincible)
                {
                    Player.DamageRecieved(Damage, this.gameObject, false);
                }
            }
        }
    }

}
