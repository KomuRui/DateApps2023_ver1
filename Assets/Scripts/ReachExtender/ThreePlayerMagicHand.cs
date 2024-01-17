using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.XR;

public class ThreePlayerMagicHand : MonoBehaviour
{
    private float speed = 8f;
    public bool bigMax = false;        //�}�W�b�N�n���h���L�т��������ǂ���
    public bool isFinish = false;
    [SerializeField] private GameObject myArmParentTop;
    [SerializeField] ThreeArm arm;
    private Vector3 defScale;

    // Start is called before the first frame update
    void Start()
    {
        defScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //���傫���Ȃ肫�����珈�������Ȃ�
        if (transform.parent.gameObject.GetComponent<ReachExtenderThreePlayer>().GetIsMoving() && !bigMax)
        {
            isFinish = false;

            //�L�т�
            Extend();

            MagicHandIsHit magicHandHit = myArmParentTop.GetComponent<MagicHandIsHit>();

            //�X�e�[�W�ɓ��������甽�˂̏���
            if (magicHandHit == null || !magicHandHit.isHit) return;

            bigMax = true;
        }
        //���傫���Ȃ肫������߂�
        if (transform.parent.gameObject.GetComponent<ReachExtenderThreePlayer>().GetIsMoving())
        {
            //�߂鏈��
            Return();
        }
    }

    //�L�т鏈��
    public void Extend()
    {
        //�A�[�����v���C���[���X�^�������Ȃ��悤�ɂ���
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
            transform.parent.gameObject.GetComponent<ReachExtenderThreePlayer>().SetIsMoving(false);
        }
    }
}
