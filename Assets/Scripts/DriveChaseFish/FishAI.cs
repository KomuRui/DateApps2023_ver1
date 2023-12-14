using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishAI : MonoBehaviour
{
    [SerializeField] private Transform[] goal;
    private int lookNum = 0;
    private NavMeshAgent agent = null;
    private float time = 0.0f;

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

        MoveChange();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,-0.35f, transform.position.z);

        if (Vector3.Distance(agent.destination, transform.position) < 1.0f)
            MoveChange();

        // パスの方向を計算し、Look At コンストレイントに適用します
        Vector3 pathDirection = agent.steeringTarget - transform.position;
        if (pathDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(pathDirection) * Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }

        //時間経過
        time += Time.deltaTime;
        if (time > 3) MoveChange();

    }

    void MoveChange()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        time = 0.0f;
        lookNum = Random.Range(0, goal.Length);
        agent.SetDestination(new Vector3(goal[lookNum].position.x, this.transform.position.y, goal[lookNum].position.z));
    }
}
