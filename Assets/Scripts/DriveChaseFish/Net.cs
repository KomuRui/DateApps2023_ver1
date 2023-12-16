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
    private Vector3 initialScale;  //�����g�嗦
    private float startTime;       //�ړ��J�n����
    public bool isNetMove;        //�l�b�g�ړ�����
    private bool isNetImposition = false;  //�l�b�g�������Ă邩
    private bool isNetReturn = false;      //�l�b�g�����ɖ߂��Ă��邩
    private List<GameObject> getFish = new List<GameObject>();  //�擾������

    // Start is called before the first frame update
    void Start()
    {
        //�g�嗦��ۑ����Ă���
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //�Ԕ���
        if (!isNetImposition && Input.GetButtonDown("Abutton" + transform.parent.GetComponent<PlayerNum>().playerNum)) NetExecute();

        //�ړ����Ȃ�v�Z
        if (isNetMove) NetPosCalc();

    }

    //�Ԃ𔭓�
    public void NetExecute()
    {
        //������C���߂܂���Ȃ�������
        if(netCollider.fishObj.Count <= 0)
        {
            transform.parent.GetComponent<DriveChaseFishPlayer>().isMove = false;
            startTime = Time.time;
            isNetImposition = true;
            isNetMove = true;
            isNetReturn = false;
            this.transform.DOScale(new Vector3(impositionScale.x, initialScale.y, impositionScale.z), 1f).OnComplete(NetReturn);
            return;
        }

        //�߂܂����������ׂĊo���Ă���
        getFish = netCollider.fishObj;

        //�Ԃɓ������Ă��鋛��߂܂���
        foreach (var fish in netCollider.fishObj)
        {
            fish.layer = 9;
            fish.GetComponent<NavMeshAgent>().enabled = false;
            fish.GetComponent<Rigidbody>().isKinematic = true;
            fish.transform.parent = transform.parent;
        }

        //�Ԃ̃}�[�J�[�ɓ����蔻�������
        netMark.GetComponent<MeshCollider>().enabled = true;

        //�Ԃ̃R���C�_�[���O��
        netCollider.GetComponent<CapsuleCollider>().enabled = false;

        //�������ݒ�
        transform.parent.GetComponent<DriveChaseFishPlayer>().isMove = false;
        startTime = Time.time;
        isNetImposition = true;
        isNetMove = true;
        isNetReturn = false;
        this.transform.DOScale(new Vector3(impositionScale.x, initialScale.y,impositionScale.z), 1f).OnComplete(ParentMoveOK);
    }

    //�Ԃ̈ʒu�v�Z
    private void NetPosCalc()
    {
        float distCovered = (Time.time - startTime) / moveSpeed; // �ړ������������v�Z

        if(isNetReturn)
            transform.position = Vector3.Lerp(transform.position, transform.parent.GetComponent<DriveChaseFishPlayer>().transform.position, distCovered);  //�ړ�
        else
            transform.position = Vector3.Lerp(transform.position, impositionBase.position, distCovered); //�ړ�
    }

    //�߂܂��������v�[���ɗ��Ƃ�
    public void FishGoPool(Transform[] fallPoint, Transform[] goalPoint)
    {
        //���̑���
        int fishSum = 0;

        //���Ǘ�
        DriveChaseFishGameManager mana = ((DriveChaseFishGameManager)GameManager.nowMiniGameManager);

        //���Ƀv�[���Ɍ����킹��
        foreach (var fish in getFish)
        {
            int fallLookNum = Random.Range(0, fallPoint.Length);
            fish.GetComponent<Rigidbody>().isKinematic = false;
            fish.GetComponent<FishAI>().SetPoolMove(goalPoint, fallPoint[fallLookNum].position);

            //�����̋��Ȃ�
            if (fish.tag == "GoldFishes")
            {
                mana.fishManager.goldFishCount--;
                fishSum += 3;
            }
            else
                fishSum++;

            mana.fishManager.fishSumCount--;
        }

        //��������̕����_�ǉ�
        ((DriveChaseFishGameManager)GameManager.nowMiniGameManager).FishScorePlus(transform.parent.GetComponent<PlayerNum>().playerNum, fishSum);

        //�Ԃ̃R���C�_�[������
        netCollider.GetComponent<CapsuleCollider>().enabled = true;
        netCollider.fishObj.Clear();
    }

    //�e�̈ړ�������
    private void ParentMoveOK() { transform.parent.GetComponent<DriveChaseFishPlayer>().isMove = true; isNetMove = false; }

    //�e�̈ړ�������
    private void ParentMoveOK2() { transform.parent.GetComponent<DriveChaseFishPlayer>().isMove = true; isNetMove = false; isNetImposition = false; }


    //�Ԃ��Ƃɖ߂�
    public void NetReturn()
    {
        startTime = Time.time;
        isNetReturn = true;
        isNetImposition = true;
        isNetMove = true;
        this.transform.DOScale(new Vector3(initialScale.x, initialScale.y, initialScale.z), 1f).OnComplete(ParentMoveOK2);
    }

}
