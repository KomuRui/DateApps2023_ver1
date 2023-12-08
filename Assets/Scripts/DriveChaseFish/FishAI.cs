using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishAI : MonoBehaviour
{
    [SerializeField] private Transform[] goal;
    private int lookNum = 0;
    private NavMeshAgent agent = null;

    // Start is called before the first frame update
    void Start()
    {
        //nullならこのさきしょりしない
        if (goal == null) return;
        
        lookNum = Random.Range(0, goal.Length);
        float scale = Random.Range(1.0f, 2.1f);
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

        if (agent == null) return;

        if (Vector3.Distance(agent.destination, transform.position) < 1.0f)
            MoveChange();

        // パスの方向を計算し、Look At コンストレイントに適用します
        Vector3 pathDirection = agent.steeringTarget - transform.position;
        if (pathDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(pathDirection) * Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }

    void MoveChange()
    {
        lookNum = Random.Range(0, goal.Length);
        agent.SetDestination(new Vector3(goal[lookNum].position.x, this.transform.position.y, goal[lookNum].position.z));
    }
}
