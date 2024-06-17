using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Status : MonoBehaviour
{
    public int HealthPoint = 10;
    public int HealthPointMax = 10;
    public int AttackDamage = 10;
    public bool HookUp = false;

    public bool isPireceAttacked = false;



    // Start is called before the first frame update
    void Start()
    {
        HealthPoint = HealthPointMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
