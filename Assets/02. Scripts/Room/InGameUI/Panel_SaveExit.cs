using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Panel_SaveExit : MonoBehaviour
{
    public Button Btn_back;
    public Button[] Btn_Save;

    [Header("Popup")]
    public GameObject Popup_Obj;
    public Text Popup_text;
    public Button Popup_OKBtn;


    // Start is called before the first frame update
    void Start()
    {
        if (Btn_back != null)
        {
            Btn_back.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });
        }

        if (Popup_OKBtn != null)
        {
            Popup_OKBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Title");
            });
        }

        for (int i = 0; i < Btn_Save.Length; i++)
        {
            if (Btn_Save[i] != null)
            {
                int idx = i;
                Btn_Save[idx].onClick.AddListener(() =>
                {
                    Debug.Log("Saving Data!");

                    SaveData data = new SaveData();
                    data.Level = Player_Status.Level;
                    data.Name = Player_Status.Name;
                    data.Cur_Position = Mgr_Game.inst.Player.transform.position;
                    data.Room_Position = Player_Status.Cur_RoomPos;

                    data.GotDash = Player_Status.GotDash;
                    data.GotD_Jump = Player_Status.GotD_Jump;
                    data.GotHook = Player_Status.GotHook;
                    data.GotUpSpecial = Player_Status.GotUpSpecial;

                    Mgr_SaveFileMake.inst.Save(data, idx + 1);

                    Popup_Obj.gameObject.SetActive(true);
                    Popup_text.text = "Data saved at savefile " + (idx + 1) + ".";
                });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
