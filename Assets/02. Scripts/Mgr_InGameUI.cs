using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mgr_InGameUI : MonoBehaviour
{
    Mgr_Player Player;


    [Header("====IN GAME====")]
    public Text LevelShowText;
    public Image HP_Bar;

    [Header("====MENU====")]
    public GameObject Panel_Menu;

    public Button Btn_Guide_inputSystem;
    public Button Btn_Menu;
    public Button Btn_SaveMenu;
    public Button Btn_SettingsMenu;
    public Button Btn_SkilltreeMenu;
    public Button Btn_CharInfoMenu;

    public Button Btn_Menu_Return;

    [Header("====MENUPANEL====")]
    public GameObject Panel_Guide_inputSystem;
    public bool Panel_Guide_On;
    public GameObject Panel_Save;
    public GameObject Panel_Settings;
    public GameObject Panel_Skilltree;
    public GameObject Panel_CharInfo;

    [Header("====INVENTORY====")]
    public GameObject Panel_Inventory;
    public Button Btn_Inventory;

    [Header("====QUEST====")]
    public GameObject Panel_Quest;
    public Button Btn_Quest;


    public GameObject Panel_SceneLoad;
    

    // Start is called before the first frame update
    void Start()
    {
        Player = Mgr_Game.inst.Player.GetComponent<Mgr_Player>();

        Panel_Guide_On = false;

        if (Btn_Menu != null)
        {
            Btn_Menu.onClick.AddListener(() => 
            {
                Panel_Menu.SetActive(true);
            });
        }

        if (Btn_Menu_Return != null)
        {
            Btn_Menu_Return.onClick.AddListener(() =>
            {
                Panel_Menu.SetActive(false);
            });
        }

        if (Btn_SaveMenu != null)
        {
            Btn_SaveMenu.onClick.AddListener(() =>
            {
                Panel_Save.SetActive(true);
            });
        }

        if (Btn_SettingsMenu != null)
        {
            Btn_SettingsMenu.onClick.AddListener(() =>
            {
                Panel_Settings.SetActive(true);
            });
        }

        if (Btn_CharInfoMenu != null)
        {
            Btn_CharInfoMenu.onClick.AddListener(() => 
            {
                Panel_CharInfo.SetActive(true);
            });
        }

        if (Btn_SkilltreeMenu != null)
        {
            Btn_SkilltreeMenu.onClick.AddListener(() =>
            {
                Panel_Skilltree.SetActive(true);
            });
        }

        if (Btn_Inventory != null)
        {
            Btn_Inventory.onClick.AddListener(() => 
            {
                Panel_Inventory.SetActive(true);
            });
        }

        if (Btn_Quest != null)
        {
            Btn_Quest.onClick.AddListener(() => 
            {
                Panel_Quest.SetActive(true);
            });
        }

    }

    public void PanelONOFF()
    {
        Panel_Guide_On = !Panel_Guide_On;
    }


    // Update is called once per frame
    void Update()
    {
        Panel_Guide_inputSystem.SetActive(Panel_Guide_On);

        LevelShowText.text = Player_Status.Level.ToString();
        HP_Bar.fillAmount = (float)Player.Health / (float)Player_Status.HP_Max;

    }
}
