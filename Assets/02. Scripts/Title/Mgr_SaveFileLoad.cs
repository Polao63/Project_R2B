using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Mgr_SaveFileLoad : MonoBehaviour
{
    public GameObject obj_FileSel;
    public Button[] Btn_FileSel;

    private void Awake()
    {
        Btn_FileSel = GetComponentsInChildren<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Btn_FileSel.Length; i++)
        {
            if (Btn_FileSel[i] != null)
            {
                int idx = i;
                Btn_FileSel[idx].onClick.AddListener(() => 
                {
                    if (File.Exists(Application.dataPath + "/SaveFile" + (idx + 1).ToString() + ".json") == false)
                    {
                        Debug.LogError("세이브 파일을 찾을수 없습니다.");
                    }
                    else if (File.ReadAllText(Application.dataPath + "/SaveFile" + (idx + 1).ToString() + ".json") == "")
                    {
                        Debug.LogError("세이브 파일이 비어있습니다.");
                    }
                    else
                    {
                        string SaveFileAddress = File.ReadAllText(Application.dataPath + "/SaveFile" + (idx + 1).ToString() + ".json");

                        SaveData data = JsonUtility.FromJson<SaveData>(SaveFileAddress);

                        Debug.Log((idx + 1).ToString() + "번 파일을 불러왔습니다.");
                        Debug.Log("Level : " + data.Level);
                        Debug.Log("Name : " + data.Name);
                        Debug.Log("Position : " + data.Cur_Position);
                        Debug.Log("Room : " + data.Room_Position);

                        Player_Status.Level = data.Level;
                        Player_Status.Name = data.Name;
                        Player_Status.LastLoadPosition = data.Cur_Position;
                        Player_Status.Cur_RoomPos = data.Room_Position;

                        Player_Status.GotDash = data.GotDash;
                        Player_Status.GotD_Jump = data.GotD_Jump;
                        Player_Status.GotHook = data.GotHook;
                        Player_Status.GotUpSpecial = data.GotUpSpecial;

                        Player_Status.IsLoaded = true;

                        SceneManager.LoadScene("Player");
                        SceneManager.LoadScene("Management_Scene", LoadSceneMode.Additive);
                        SceneManager.LoadScene("Room_" + data.Room_Position.x + "_" + data.Room_Position.y, LoadSceneMode.Additive);                       
                    }
                });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
