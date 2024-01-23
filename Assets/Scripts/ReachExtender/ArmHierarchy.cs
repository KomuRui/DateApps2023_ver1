using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ArmHierarchy : MonoBehaviour
{
    public List<GameObject> magicHandTopList = new List<GameObject>();
    public List<GameObject> magicHandList = new List<GameObject>();

    private bool isReverse = false;
    private bool isReverseHit = false;
    private bool isHitPlayer = false;
    [SerializeField] float stopTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((magicHandList.First().GetComponent<MagicHand>().bigMax && !isReverseHit) || isHitPlayer)
        {
            Invoke("SetReverseTrue", stopTime);
            isReverseHit = true;
            isHitPlayer = false;
        }

        if (isReverse)
            Return();
    }

    //�}�W�b�N�n���h��߂�����
    public void Return()
    {
        bool allFinish = true;
        for (int i = 0; i < magicHandList.Count; i++)
        {
            //�}�W�b�N�n���h��߂�����
            if (magicHandList[i].activeSelf)
            {
                magicHandList[i].GetComponent<MagicHand>().Return();
                break;
            }
        }

        for (int i = 0; i < magicHandList.Count; i++)
        {
            //���ׂẴA�[���̏������I����Ă��邩���ׂ�
            if (!magicHandList[i].GetComponent<MagicHand>().isFinish)
            {
                allFinish = false;
            }
        }

        if (allFinish)
        {
            isReverse = false;
            isReverseHit = false;
        }
    }

    public void SetReverseTrue()
    { 
        isReverse = true;
        for (int i = 0; i < magicHandList.Count; i++)
        {
            if (magicHandList[i].activeSelf)
            {
                magicHandList[i].GetComponent<MagicHand>().bigMax = false;
                return;
            }
        }
    }

    //�v���C���[�ɓ����������ɌĂ΂�鏈��
    public void HitPlayer()
    {
        for (int i = 0; i < magicHandList.Count; i++)
        {
            //�}�W�b�N�n���h��߂�����
            if (magicHandList[i].activeSelf)
            {
                magicHandList[i].GetComponent<MagicHand>().bigMax = true;
                isHitPlayer = true;
                break;
            }
            else
            {
                magicHandList[i].GetComponent<MagicHand>().isFinish = true;
            }
        }
    }

}
