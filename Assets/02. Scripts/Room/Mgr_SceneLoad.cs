using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mgr_SceneLoad : MonoBehaviour
{
    public static Mgr_SceneLoad inst;
    public Vector2 Cur_RoomPos;

    public Mgr_SceneEnd[] SceneEnds;

    Mgr_Player Player;

    private void Awake()
    {
        inst = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Player = Mgr_Game.inst.Player.GetComponent<Mgr_Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Player = Mgr_Game.inst.Player.GetComponent<Mgr_Player>();

        Cur_RoomPos = Player_Status.Cur_RoomPos;
    }


    public void NextRoom(Vector2Int RoomPos, string startingDataKey)
    {
        Debug.Log("Cur_RoomPos : " + Player_Status.Cur_RoomPos);
        Debug.Log("SceneLoad : Room_" + (Player_Status.Cur_RoomPos.x + RoomPos.x) + "_" + (Player_Status.Cur_RoomPos.y + RoomPos.y));
        StartCoroutine(RoomChangeCoroutine(RoomPos, startingDataKey));
    }

    IEnumerator RoomChangeCoroutine(Vector2Int RoomPos, string startingDataKey)
    {
        var loadAsync = SceneManager.LoadSceneAsync("Room_" + (Player_Status.Cur_RoomPos.x + RoomPos.x) + "_" + (Player_Status.Cur_RoomPos.y + RoomPos.y), LoadSceneMode.Additive);
        while (!loadAsync.isDone)
        {
            yield return null;
        }
        //GameObject.FindObjectOfType<Mgr_InGameUI>().Panel_Load.SetActive(true);

        yield return new WaitForEndOfFrame();

        var loadMapRoot = MapRoot.TotalMapRoots.Find(x => x.mapIndex == Player_Status.Cur_RoomPos + RoomPos);
        if (loadMapRoot != null)
        {
            var startingData = loadMapRoot.roomStartingDatas.Find(x => x.dataKey.Equals(startingDataKey));

            Player.transform.position = startingData.point.position;
            //if (Player.transform.position.y != startingData.point.position.y)
            //{
            //    Player.transform.position = new Vector2(startingData.point.position.x, Player.transform.position.y);
            //}
            //else
            //{
                
            //}
        }

        SceneManager.UnloadSceneAsync("Room_" + Cur_RoomPos.x + "_" + Cur_RoomPos.y);
        Player_Status.Cur_RoomPos = Player_Status.Cur_RoomPos + RoomPos;
        //GameObject.FindObjectOfType<Mgr_InGameUI>().Panel_Load.SetActive(false);
    }
}
