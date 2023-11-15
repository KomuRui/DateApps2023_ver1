using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShakeMove : MonoBehaviour
{
    public Transform[] goal;
    private int lookNum = 0;
    private NavMeshAgent agent = null;

    void Start()
    {
        //nullならこのさきしょりしない
        if (goal == null) return;

        lookNum = Random.Range(0, goal.Length);

         //nullならこのさきしょりしない
        if (goal[lookNum] == null) return;
        
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(goal[lookNum].position.x,this.transform.position.y, goal[lookNum].position.z));
        
        StartCoroutine(MoveChange(1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if(agent == null) return;
        if(agent.hasPath == false) return;

        // パスの方向を計算し、Look At コンストレイントに適用します
        Vector3 pathDirection = agent.steeringTarget - transform.position;
        if (pathDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(pathDirection) * Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
                
        }
        
    }

    IEnumerator MoveChange(float delay)
    {
        yield return new WaitForSeconds(delay);

        lookNum = Random.Range(0, goal.Length);
        agent.SetDestination(new Vector3(goal[lookNum].position.x, this.transform.position.y, goal[lookNum].position.z));
        
        StartCoroutine(MoveChange(1.5f));
    }
}
