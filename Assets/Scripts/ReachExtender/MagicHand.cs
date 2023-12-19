using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    private GameObject preArm; // �O�ɐL�тĂ��A�[��
    private int magicHandNum = 0;
    private int maxRefrect = 1; 

    private Vector3 defScale;
    private float speed = 0.002f;


    void Start()
    {
        defScale = transform.localScale;
    }
    
    // Update is called once per frame
    void Update()
    {
        //���傫���Ȃ肫�����珈�������Ȃ�
        if (!bigMax)
        {
            //�L�т�
            Extend();

            //�X�e�[�W�ɓ��������甽�˂̏���
            if (myArmParentTop.GetComponent<MagicHandIsHit>() == null || !myArmParentTop.GetComponent<MagicHandIsHit>().isHit) return;

            //���ˉ񐔂����ɒB���Ă��Ȃ�������
            if (magicHandNum < maxRefrect)
            {
                BigMax();
            }
            else
            {
                bigMax = true;
            }
        }
    }

    public void BigMax()
    {
        bigMax = true;
        if (nextArmParent != null)
        {
            //���C���������������
            RaycastHit rayHit;

            //���C���΂�����
            Vector3 ray = myArmParentTop.gameObject.transform.position - transform.parent.gameObject.transform.position;

            //���C���΂�����
            Ray ray2 = new Ray(transform.position, myArmParentTop.gameObject.transform.position - transform.parent.gameObject.transform.position);

            //���C���΂�
            if (UnityEngine.Physics.Raycast(ray2, out rayHit, 9999))
            {
                //���˃x�N�g�����쐬
                Vector3 refrect = Vector3.Reflect(ray, rayHit.normal);

                //�ʒu��ݒ�
                nextArmParent.transform.position = rayHit.point;

                //������ݒ�
                nextArmParent.transform.LookAt(refrect);
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
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + speed);
    }

    //�߂鏈��
    public void Return()
    {
        bigMax = true;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - speed);

        if (defScale.z >= transform.localScale.z)
        {
            bigMax = false;
            transform.localScale = defScale;
            this.gameObject.SetActive(false);
        }
    }
}
