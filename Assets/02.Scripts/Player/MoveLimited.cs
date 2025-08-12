using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ȭ�� ������ �����°� ����
public class MoveLimited : MonoBehaviour
{

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position); // 3D ���� ��ǥ�� 2D ����Ʈ ��ǥ

        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(pos); // 2D ����Ʈ ��ǥ�� 3D ���� ��ǥ
    }
}
