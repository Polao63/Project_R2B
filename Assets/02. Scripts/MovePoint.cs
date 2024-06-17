using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public Color lineColor = Color.yellow;
    private Transform[] points;

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        //WayPointGroup ���ӿ� ��Ʈ �Ʒ��� �ִ� ��� Point ���ӿ�����Ʈ ����
        points = GetComponentsInChildren<Transform>();
        int nextIdx = 1;
        Vector3 currPos = points[nextIdx].position;
        Vector3 nextPos;
        //Point ���ӿ�����Ʈ�� ��ȸ�ϸ鼭 ������ �׸�
        for (int i = 0; i <= points.Length; i++)
        {

            //������ Point �� ��� ù ��° Point �� ����
            nextPos = (++nextIdx >= points.Length) ? points[1].position :
                points[nextIdx].position;
            //���� ��ġ���� ���� ��ġ���� ������ �׸�
            Gizmos.DrawLine(currPos, nextPos);
            currPos = nextPos;
        }
    }
}
