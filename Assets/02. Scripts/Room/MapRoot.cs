using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomStartingData
{
    public string dataKey; //��� ������ ���ð��ΰ�
    public Transform point; //�� �ȿ� �������� �÷��̾��� ��ġ
    
}

public class MapRoot : MonoBehaviour //��
{
    public static List<MapRoot> TotalMapRoots = new List<MapRoot>();
    public Collider2D BackGround;

    public Vector2Int mapIndex = Vector2Int.zero; //���� �� ��ġ (������Ʈ���� ���� ����)
    public List<RoomStartingData> roomStartingDatas = new List<RoomStartingData>();

    private void Awake()
    {
        TotalMapRoots.Add(this); //�ε� ������ ����Ʈ�� �߰�

        //Player_Status.Cur_RoomPos = mapIndex;
        Mgr_Game.inst.Virtual_Camera.m_BoundingShape2D = BackGround;
    }

    private void OnDestroy()
    {
        TotalMapRoots.Remove(this); //�ε� ���� �ʾ����� ����Ʈ���� ����
    }

    private void Update()
    {
        
    }

}
