using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class FishAI : MonoBehaviour
{
    [SerializeField] private Transform[] goal;
    private int lookNum = 0;
    private NavMeshAgent agent = null;
    private float time = 0.0f;
    public bool isAIMove = true;
    public float maxHeight = 2f; 

    // Start is called before the first frame update
    void Start()
    {
        //nullならこのさきしょりしない
        if (goal == null) return;
        
        lookNum = Random.Range(0, goal.Length);
        float scale = 1.7f;
        this.transform.localScale = new Vector3 (scale, scale, scale);
        this.GetComponent<NavMeshAgent>().speed = Random.Range(2, 11);

        //nullならこのさきしょりしない
        if (goal[lookNum] == null) return;

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(goal[lookNum].position.x, this.transform.position.y, goal[lookNum].position.z));

        GoalChange();
    }

    // Update is called once per frame
    void Update()
    {
        //AIの動きなら
        if (isAIMove)
            AIMove();
        else
            GoPoolMove();

    }

    //AIの動き
    private void AIMove()
    {
        transform.position = new Vector3(transform.position.x, -0.35f, transform.position.z);

        if (Vector3.Distance(agent.destination, transform.position) < 1.0f)
            GoalChange();

        // パスの方向を計算し、Look At コンストレイントに適用します
        Vector3 pathDirection = agent.steeringTarget - transform.position;
        if (pathDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(pathDirection) * Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }

        //時間経過
        time += Time.deltaTime;
        if (time > 3) GoalChange();
    }

    //プールに向かう動き
    private void GoPoolMove()
    {

    }

    //ゴール先変更
    private void GoalChange()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        time = 0.0f;
        lookNum = Random.Range(0, goal.Length);
        agent.SetDestination(new Vector3(goal[lookNum].position.x, this.transform.position.y, goal[lookNum].position.z));
    }

    //ゴールに向かう動きに設定
    public void SetPoolMove(Transform[] goolPoint,Vector3 poolfallPoint)
    {
        isAIMove = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 startPos = transform.position;
        Vector3 endPos = poolfallPoint;

        // 開始地点と終了地点の距離に基づいて、放物線の高さを計算
        float distance = Vector3.Distance(startPos, endPos);
        float  gravity = -(maxHeight * 2) / (distance * distance);

        // 放物線の計算
        Vector3 startToEnd = endPos - startPos;
        startToEnd.y = 0; // y方向の影響を除外
        float horizontalDistance = startToEnd.magnitude;
        float verticalDistance = endPos.y - startPos.y;
        float time = Mathf.Sqrt(-2 * maxHeight / gravity) + Mathf.Sqrt(2 * (verticalDistance - maxHeight) / gravity);
        Vector3 horizontalDirection = startToEnd.normalized;
        horizontalDirection *= horizontalDistance / time;

        // オブジェクトを放物線の軌道に移動させる
        rb.velocity = new Vector3(horizontalDirection.x, Mathf.Sqrt(-2 * gravity * maxHeight), horizontalDirection.z);
    }
}
