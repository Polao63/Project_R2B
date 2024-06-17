using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomStartingData
{
    public string dataKey; //어디 문에서 나올것인가
    public Transform point; //방 안에 들어왔을때 플레이어의 위치
    
}

public class MapRoot : MonoBehaviour //맵
{
    public static List<MapRoot> TotalMapRoots = new List<MapRoot>();
    public Collider2D BackGround;

    public Vector2Int mapIndex = Vector2Int.zero; //현재 맵 위치 (컴포넌트에서 직접 지정)
    public List<RoomStartingData> roomStartingDatas = new List<RoomStartingData>();

    private void Awake()
    {
        TotalMapRoots.Add(this); //로드 됐을때 리스트에 추가

        //Player_Status.Cur_RoomPos = mapIndex;
        Mgr_Game.inst.Virtual_Camera.m_BoundingShape2D = BackGround;
    }

    private void OnDestroy()
    {
        TotalMapRoots.Remove(this); //로드 되지 않았을때 리스트에서 삭제
    }

    private void Update()
    {
        
    }

}
