using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class FishAI : MonoBehaviour
{
    [SerializeField] private Transform[] goal;
    private int lookNum = 0;
    private NavMeshAgent agent = null;
    private Transform parent = null;
    private float time = 0.0f;
    public bool isAIMove = true;
    public float maxHeight = 2f; 
    public float jumpTime = 2f; 

    // Start is called before the first frame update
    void Start()
    {
        //nullならこのさきしょりしない
        if (goal == null) return;
        
        lookNum = Random.Range(0, goal.Length);
        float scale = 1.7f;
        parent = transform.parent;
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

        if (Vector3.Distance(agent.destination, transform.position) < 2.5f)
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
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
        goal = goolPoint;

        // 中点を求める
        Vector3 startPos = transform.position;
        Vector3 endPos = poolfallPoint;
        endPos.y = startPos.y;
        Vector3 half = endPos - startPos * 0.50f + startPos;
        half.y += Vector3.up.y + maxHeight;
        StartCoroutine(LerpThrow(this.gameObject, startPos, half, endPos, jumpTime));
    }

    IEnumerator LerpThrow(GameObject target, Vector3 start, Vector3 half, Vector3 end, float duration)
    {
        float startTime = Time.timeSinceLevelLoad;
        float rate = 0f;
        while (true)
        {
            if (rate >= 1.0f)
            {
                this.GetComponent<NavMeshAgent>().enabled = true;
                this.GetComponent<NavMeshAgent>().speed = 2;
                isAIMove = true;
                transform.parent = parent;
                GoalChange();
                yield break;
            }
            
            float diff = Time.timeSinceLevelLoad - startTime;
            rate = diff / (duration / 60f);
            target.transform.position = CalcLerpPoint(start, half, end, rate);

            yield return null;
        }
    }

    Vector3 CalcLerpPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        var a = Vector3.Lerp(p0, p1, t);
        var b = Vector3.Lerp(p1, p2, t);
        return Vector3.Lerp(a, b, t);
    }
}
