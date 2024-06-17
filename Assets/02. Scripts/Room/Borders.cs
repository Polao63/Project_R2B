using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour
{

    public bool U = true;
    public bool D = true;
    public bool L = true;
    public bool R = true;

    public GameObject Border_U;
    public GameObject Border_D;
    public GameObject Border_L;
    public GameObject Border_R;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Border_U.SetActive(U);
        Border_D.SetActive(D);
        Border_L.SetActive(L);
        Border_R.SetActive(R);
    }
}
