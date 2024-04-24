using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] Mgr_Player Player;

    bool CanDetected;

    

    private void Update()
    {        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CanDetected = false;
            Debug.Log(CanDetected);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CanDetected = true;
            Debug.Log(CanDetected);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
