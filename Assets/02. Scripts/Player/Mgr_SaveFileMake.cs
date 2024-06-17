using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Mgr_SaveFileMake : MonoBehaviour
{
    public static Mgr_SaveFileMake inst;

    private void Awake()
    {
        inst = this;
    }

    public void Save(SaveData Data2Save, int SaveFileNum)
    {
        SaveData data = new SaveData();

        data.Level = Data2Save.Level;
        data.Name = Data2Save.Name;
        data.Cur_Position = Data2Save.Cur_Position;
        data.Room_Position = Data2Save.Room_Position;

        data.GotDash = Data2Save.GotDash;
        data.GotD_Jump = Data2Save.GotD_Jump;
        data.GotHook = Data2Save.GotHook;
        data.GotUpSpecial = Data2Save.GotUpSpecial;

        if (SaveFileNum > 0 && SaveFileNum < 4)
        {
            File.WriteAllText(Application.dataPath + "/SaveFile" + SaveFileNum + ".json", JsonUtility.ToJson(data));
            Debug.Log("Saved At : " + Application.dataPath + "/SaveFile" + SaveFileNum + ".json");
        }
        else
        {
            Debug.LogWarning("SaveFileNumError. Saving at Test File...");
            File.WriteAllText(Application.dataPath + "/SaveFileTest.json", JsonUtility.ToJson(data));
            Debug.Log("Saved At : " + Application.dataPath + "/SaveFileTest.json");
        }
    }
}
