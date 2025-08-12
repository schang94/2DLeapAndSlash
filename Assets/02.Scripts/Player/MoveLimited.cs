using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 화면 밖으로 나가는거 막기
public class MoveLimited : MonoBehaviour
{

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position); // 3D 월드 좌표를 2D 뷰포트 좌표

        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(pos); // 2D 뷰포트 좌표를 3D 월드 좌표
    }
}
