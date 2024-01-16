using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ModeSelectPlayerNavMesh : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;
    public float stopDistance;

    // ゲーム実行中に毎フレーム実行する処理
    void Update()
    {
        //ターゲットオブジェクトの座標を格納
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);

        //距離を格納
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > stopDistance)
        {
            transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
