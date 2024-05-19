using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Ctrl : MonoBehaviour
{
    public Enemy_Status Status;

    SpriteRenderer spriter;
    public Rigidbody2D rigid;

    public Canvas DamageTxtCanvas;
    public GameObject Damage_Text;
    public float DamageForceMax = 1f;

    public bool isAttacked = false;
    public bool isPierceCalled = false;

    public bool KnockbackOn = false;
    


    // Start is called before the first frame update
    void Start()
    {
        Status = GetComponent<Enemy_Status>();
        spriter = GetComponentInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -25f)
        {
            rigid.velocity = Vector3.zero;
            transform.position = Vector2.zero;
        }

        if (isPierceCalled)
        {
            isPierceCalled = false;
            Status.HealthPoint -= 1;
            DamageTextShow(1, (Status.HealthPoint <= 0));
        }


        if (isAttacked)
        {
            spriter.color = Color.red;
        }
        else
        {
            spriter.color = Color.gray;
        }



    }


    void DamageTextShow(int Damage, bool FinishAttack)
    {
        GameObject GO = Instantiate(Damage_Text, DamageTxtCanvas.transform);
        GO.transform.position = this.transform.position;
        if (FinishAttack)
        {
            GO.GetComponent<Text>().text = Damage.ToString() + "!";
        }
        else
        {
            GO.GetComponent<Text>().text = Damage.ToString();
        }
        Destroy(GO, 2f);

        Vector2 Force = new Vector2(Random.Range(-DamageForceMax, DamageForceMax), Random.Range(2, DamageForceMax));

        GO.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GO.GetComponent<Rigidbody2D>().velocity = Force;

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Attack"))
        {
            int Damage = Mgr_Game.inst.Player.GetComponent<Mgr_Player>().atk + Random.Range(0, 4);
            DamageTextShow(Damage, (Status.HealthPoint - Damage <= 0));
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("HookEnd"))
        {
            DamageTextShow(Random.Range(1, 4), (Status.HealthPoint <= 0));
        }
        else
        {
            isAttacked = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isAttacked = false;
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }


}
