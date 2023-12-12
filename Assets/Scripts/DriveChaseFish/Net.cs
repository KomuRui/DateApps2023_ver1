using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.AI;

public class Net : MonoBehaviour
{

    [SerializeField] private Vector3 impositionScale;  //����������̊g�嗦
    [SerializeField] private Transform impositionBase; //����������̊�ɂȂ�transform
    [SerializeField] private float moveSpeed = 5.0f;   //�ړ����x
    [SerializeField] private NetCollider netCollider;  //�Ԃ̃R���C�_�[
    [SerializeField] private GameObject netMark;       //�Ԃ̃}�[�J�[
    private Vector3 initialPos;    //�����ʒu
    private Vector3 initialScale;  //�����g�嗦
    private float startTime;       //�ړ��J�n����
    private bool isNetMove;        //�l�b�g�ړ�����

    // Start is called before the first frame update
    void Start()
    {
        //�ʒu�Ɗg�嗦��ۑ����Ă���
        initialPos = transform.position;
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //�Ԕ���
        if (Input.GetButtonDown("Abutton" + transform.parent.GetComponent<PlayerNum>().playerNum)) NetExecute();

        //�ړ����Ȃ�v�Z
        if (isNetMove) NetPosCalc();

    }

    //�Ԃ𔭓�
    public void NetExecute()
    {
        //�Ԃɓ������Ă��鋛��߂܂���
        foreach(var fish in netCollider.fishObj)
        {
            fish.GetComponent<NavMeshAgent>().enabled = false;
            fish.transform.parent = transform.parent;
        }

        //�Ԃ̃}�[�J�[�ɓ����蔻�������
        netMark.GetComponent<NavMeshObstacle>().enabled = true;
        netMark.GetComponent<MeshCollider>().enabled = true;

        //�������ݒ�
        transform.parent.GetComponent<DriveChaseFishPlayer>().isMove = false;
        startTime = Time.time;
        isNetMove = true;
        this.transform.DOScale(new Vector3(impositionScale.x, initialScale.y,impositionScale.z), 1f).OnComplete(ParentMoveOK);
    }

    //�Ԃ̈ʒu�v�Z
    private void NetPosCalc()
    {
        float distCovered = (Time.time - startTime) / moveSpeed; // �ړ������������v�Z
        transform.position = Vector3.Lerp(transform.position, impositionBase.position, distCovered); // �ړ�
    }

    //�e�̈ړ�������
    private void ParentMoveOK() { transform.parent.GetComponent<DriveChaseFishPlayer>().isMove = true; isNetMove = false; }
}
