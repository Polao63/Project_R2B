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

    public Vector2 target;
    public Vector2 Pointer;




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
            Vector2 grapplePos = Vector2.Lerp(Player.transform.position, target, grappleSpeed * Time.deltaTime);
            transform.position = grapplePos;

            lineR.SetPosition(0, transform.position);


            Player.GetComponent<Rigidbody2D>().velocity = (target - (Vector2)Player.transform.position).normalized * grappleSpeed;

            


            if (Vector2.Distance(Player.transform.position, target) < 2f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    Player.GetComponent<Rigidbody2D>().velocity = new Vector2(Player.GetComponent<Rigidbody2D>().velocity.x, 20f);
                    Player.D_jumpCount--;

                    retracting = false;
                    isGrappling = false;
                    lineR.enabled = false;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    retracting = false;
                    isGrappling = false;
                    lineR.enabled = false;
                }
            }
        }
        else
        {
            
        }
        

        
    }

    

    void StartGrapple()
    {
        Vector2 dir = target - (Vector2)Player.transform.position;

        //RaycastHit2D hit = Physics2D.Raycast(Player.transform.position, dir, maxDistance, grapplableMask);

        if(target != null)
        {
            isGrappling = true;
            lineR.enabled = true;
            lineR.positionCount = 2;

            StartCoroutine(Grapple());
            Player.GetComponent<Rigidbody2D>().gravityScale = 5;
        }

    }

    IEnumerator Grapple()
    {
        float t = 0;
        float time = 10;
        lineR.SetPosition(0,transform.position);
        lineR.SetPosition(1,transform.position);

        Player.GetComponent<Rigidbody2D>().gravityScale = 0;

        Vector2 newPos;

        for (; t < time; t += grappleShootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(Player.transform.position, target, t / time);
            lineR.SetPosition(0, Player.transform.position);
            lineR.SetPosition(1, newPos);
            yield return null;
        }



        lineR.SetPosition(1, target);
        retracting = true;

    }
}
