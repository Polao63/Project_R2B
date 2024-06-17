using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mgr_TitleUI : MonoBehaviour
{
    public Button Btn_NewGame = null;
    public Button Btn_LoadGame = null;
    public Button Btn_Exit = null;

    public GameObject Panel_SaveFileLoad = null;


    private void Awake()
    {
        if (Btn_NewGame != null)
        {
            Btn_NewGame.onClick.AddListener(() =>
            {
                //SceneManager.LoadScene("World Test 1");
                SceneManager.LoadScene("Player");
                SceneManager.LoadSceneAsync("Management_Scene", LoadSceneMode.Additive);
                SceneManager.LoadSceneAsync("Room_0_0", LoadSceneMode.Additive);
                
            });
        }
        if (Btn_LoadGame != null)
        {
            Btn_LoadGame.onClick.AddListener(() =>
            {
                Panel_SaveFileLoad.SetActive(true);
            });
        }
        if (Btn_Exit != null)
        {
            Btn_Exit.onClick.AddListener(() => 
            {
                Application.Quit();
            });
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
