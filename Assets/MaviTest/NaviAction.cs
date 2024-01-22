using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NaviAction : MonoBehaviour
{
    public enum STATUS
    {
        WAIT, RANDOM_WALK
    }

    public STATUS status_;
    NavMeshAgent navMeshAgent;
    Animator animator;

    Vector3 prevPos;
    Vector3 prevFront;





    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();


        Vector3 randomDirection = Random.insideUnitSphere * Random.Range(0.5f, 10.0f);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position + randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
            navMeshAgent.speed = Random.Range(0.8f, 2.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);


        if (navMeshAgent.remainingDistance < 0.2f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(5, 50);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position + randomDirection, out hit, 10f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
                navMeshAgent.speed = Random.Range(0.8f, 2.0f);
            }
        }

        //�i�r
        {
            // ���ɖڎw���ׂ��ʒu���擾
            var nextPoint = navMeshAgent.steeringTarget;
            Vector3 targetDir = nextPoint - transform.position;

            // ���̕����Ɍ����Đ��񂷂�(360�x/�b)
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime / 2);

            // �����̌����Ǝ��̈ʒu�̊p�x����30�x�ȏ�̏ꍇ�A���̏�Ő���
            float angle = Vector3.Angle(targetDir, transform.forward);
            //if (angle < 30f)
            {
                transform.position += transform.forward * navMeshAgent.speed * Time.deltaTime;
                // �������̏ꍇ�̕␳
                //if (Vector3.Distance(nextPoint, transform.position) < 0.5f)�@transform.position = nextPoint;
            }

            // target�Ɍ������Ĉړ����܂��B
           // navMeshAgent.SetDestination(target.position);
            navMeshAgent.nextPosition = transform.position;
        }

        animator.SetFloat("Speed", (prevPos - transform.position).magnitude * 100);
        animator.SetFloat("Rotate", Vector3.Angle(prevFront ,transform.forward)/1);

        prevPos = transform.position;
        prevFront = transform.forward;
    }
}
