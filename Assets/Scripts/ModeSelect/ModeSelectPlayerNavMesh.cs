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

    // �Q�[�����s���ɖ��t���[�����s���鏈��
    void Update()
    {
        //�^�[�Q�b�g�I�u�W�F�N�g�̍��W���i�[
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);

        //�������i�[
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > stopDistance)
        {
            transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
