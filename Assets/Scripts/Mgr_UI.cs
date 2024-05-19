using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mgr_UI : MonoBehaviour
{
    public Button Btn_Guide_inputSystem;

    public GameObject Panel_Guide_inputSystem;

    public bool Panel_Guide_On;

    // Start is called before the first frame update
    void Start()
    {
        Panel_Guide_On = false;

        //Btn_Guide_inputSystem.onClick.AddListener(() => 
        //{
        //    Panel_Guide_On = !Panel_Guide_On;
        //});

    }

    public void PanelONOFF()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            Panel_Guide_On = !Panel_Guide_On;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Panel_Guide_inputSystem.SetActive(Panel_Guide_On);
    }
}
