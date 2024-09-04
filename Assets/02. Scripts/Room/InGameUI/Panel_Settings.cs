using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Settings : MonoBehaviour
{
    public Button Btn_back;


    [Header("Panels")]
    public GameObject Panel_Control;
    public GameObject Panel_Graphics;
    public GameObject Panel_Sound;

    [Header("Buttons")]
    public Button Btn_Tab_Control;
    public Button Btn_Tab_Graphics;
    public Button Btn_Tab_Sound;

    public Color Selected_Color;
    public Color NotSelected_Color;




    // Start is called before the first frame update
    void Start()
    {
        Btn_back.onClick.AddListener(() => 
        {
            this.gameObject.SetActive(false);
        });



        Btn_Tab_Control.onClick.AddListener(() => 
        {
            Btn_Tab_Control.image.color = Selected_Color;
            Btn_Tab_Graphics.image.color = NotSelected_Color;
            Btn_Tab_Sound.image.color = NotSelected_Color;

            Panel_Control.SetActive(true);
            Panel_Graphics.SetActive(false);
            Panel_Sound.SetActive(false);
        });
        Btn_Tab_Graphics.onClick.AddListener(() =>
        {
            Btn_Tab_Control.image.color = NotSelected_Color;
            Btn_Tab_Graphics.image.color = Selected_Color;
            Btn_Tab_Sound.image.color = NotSelected_Color;

            Panel_Control.SetActive(false);
            Panel_Graphics.SetActive(true);
            Panel_Sound.SetActive(false);
        });
        Btn_Tab_Sound.onClick.AddListener(() =>
        {
            Btn_Tab_Control.image.color = NotSelected_Color;
            Btn_Tab_Graphics.image.color = NotSelected_Color;
            Btn_Tab_Sound.image.color = Selected_Color;

            Panel_Control.SetActive(false);
            Panel_Graphics.SetActive(false);
            Panel_Sound.SetActive(true);
        });



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
