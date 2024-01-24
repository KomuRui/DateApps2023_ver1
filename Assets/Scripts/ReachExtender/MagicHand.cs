using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static DG.Tweening.DOTweenModuleUtils;

public class MagicHand : MonoBehaviour
{
    //[SerializeField] private GameObject onePlayer;
    // Start is called before the first frame update

    public bool bigMax = false;        //�}�W�b�N�n���h���L�т��������ǂ���
    private bool isReverse = false;       //�}�W�b�N�n���h���߂��Ă��邩�ǂ���

    [SerializeField] private GameObject nextArmParent;
    [SerializeField] private GameObject nextArmParentTop;
    [SerializeField] private GameObject myArmParentTop;
    [SerializeField] ArmHierarchy armHierarchy;
    [SerializeField] Arm arm;
    [SerializeField] private ThrowAnimation bone;

    private GameObject preArm; // �O�ɐL�тĂ��A�[��
    private int magicHandNum = 0;
    private int maxRefrect = 1;

    private Vector3 defScale;
    private float speed = 12f;

    public bool isFinish = false;


    void Start()
    {
        defScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //���傫���Ȃ肫�����珈�������Ȃ�
        if (transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<ReachExtenderOnePlayer>().GetIsMoving() && !bigMax)
        {
            isFinish = false;

            //�L�т�
            Extend();

            MagicHandIsHit magicHandHit = myArmParentTop.GetComponent<MagicHandIsHit>();

            //�X�e�[�W�ɓ��������甽�˂̏���
            if (magicHandHit == null || !magicHandHit.isHit) return;

            bigMax = true;

            //���ˉ񐔂����ɒB���Ă��Ȃ�������
            if (magicHandNum < maxRefrect)
                Refrect();
        }
    }

    public void Refrect()
    {
        if (nextArmParent != null)
        {
            //���C���������������
            RaycastHit rayHit;

            //���C���΂�����
            Vector3 ray = myArmParentTop.gameObject.transform.position - transform.parent.gameObject.transform.position;

            //���C���΂�����
            Ray ray2 = new Ray(transform.position, myArmParentTop.gameObject.transform.position - transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.position);

            //���C���΂�
            if (UnityEngine.Physics.Raycast(ray2, out rayHit, 9999))
            {
                //���˃x�N�g�����쐬
                Vector3 refrect = Vector3.Reflect(ray2.direction, rayHit.normal);

                //�ʒu��ݒ�
                nextArmParent.transform.position = rayHit.point;

                //y�𔽓]
                Vector3 newRefrect = new Vector3(refrect.x, refrect.y, refrect.z);

                Debug.DrawRay(transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.position, ray2.direction * 9999, Color.red, 10, false);
                Debug.DrawRay(rayHit.point, newRefrect * 9999, Color.green, 10, false);

                //������ݒ�
                nextArmParent.transform.LookAt(newRefrect + nextArmParent.transform.position);
            }

            //���ˉ񐔂��J�E���g
            nextArmParent.GetComponent<MagicHand>().SetMagicHandNum(magicHandNum + 1);
            nextArmParent.SetActive(true);
        }
    }

    public void SetMagicHandNum(int num)
    {
        magicHandNum = num;
    }

    //�L�т鏈��
    public void Extend()
    {
        //�A�[�����v���C���[��߂܂���悤�ɂ���
        arm.SetIsActive(true);

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + speed * Time.deltaTime);
    }

    //�߂鏈��
    public void Return()
    {
        //�A�[�����v���C���[���X�^�������Ȃ��悤�ɂ���
        arm.SetIsActive(false);


        myArmParentTop.GetComponent<MagicHandIsHit>().isHit = false;

        bigMax = true;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - speed * 1.5f * Time.deltaTime);

        if (defScale.z >= transform.localScale.z)
        {
            bigMax = false;
            transform.localScale = defScale;

            //���̃A�[��������Ȃ�����Ȃ�
            if (nextArmParent != null)
            {
                transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<ReachExtenderOnePlayer>().SetIsMoving(false);
                isFinish = true;
                if (bone != null)  bone.Throw();
            }
            else
            {
                this.gameObject.SetActive(false);
                isFinish = true;
            }
        }
    }
}