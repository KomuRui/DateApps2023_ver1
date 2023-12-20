using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharaSelectOutlineInfo : MonoBehaviour
{

    public Animator animator;
    private bool isSelect = false;     //�I������Ă��邩�ǂ���
    public bool isAnimation = false;   //�A�j���[�V���������ǂ���
    private byte selectPlayerNum = 0;  //�I�����Ă���v���C���[�̔ԍ�
    public CharaSelectManager.LineNum line; //�������ǂ̃��C����
    public int num;                         //���Ԗڂ�
    private Vector3 initialRotate;          //������]
    public GameObject marubatu;             //���~
    public GameObject marubatuParent;       //���~�̐e

    // Start is called before the first frame update
    void Start()
    {
        initialRotate = transform.localEulerAngles;
        marubatuParent.transform.rotation = Quaternion.AngleAxis(-140, marubatu.transform.right) * marubatuParent.transform.rotation;
        marubatu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�I������
    //bool : �I���ł������ǂ���
    public bool SetSelect(byte playerNum,Color outlineColor)
    {
        if (isSelect) 
            return false;
        else
        {
            selectPlayerNum = playerNum;
            isSelect = true;
            return true;
        }
    }

    //�I������������
    //bool : �����ł������ǂ���
    public bool SetSelectRelease(byte playerNum)
    {
        if (!isSelect && playerNum == selectPlayerNum)
            return false;
        else
        {
            selectPlayerNum = 0;
            isSelect = false;
            return true;
        }
    }

    //�I��
    public void Select()
    {
        isAnimation = true;
        this.transform.DORotate(new Vector3(initialRotate.x, initialRotate.y + 360.0f, initialRotate.z), 1.5f, RotateMode.FastBeyond360).SetEase(Ease.InOutBack).OnComplete(SelectMove);
    }

    //�I���̎��̈ړ�
    private void SelectMove()
    {
        marubatu.gameObject.SetActive(true);
        marubatuParent.transform.DORotateQuaternion(Quaternion.AngleAxis(140, marubatu.transform.right) * marubatuParent.transform.rotation, 0.5f).OnComplete(() => isAnimation = false);
    }

    //����
    public void Release()
    {
        isAnimation = true;
        ReleaseMove();
    }

    //�I���̎��̈ړ�
    private void ReleaseMove()
    {
        marubatuParent.transform.DORotateQuaternion(Quaternion.AngleAxis(-140, marubatu.transform.right) * marubatuParent.transform.rotation, 0.5f).OnComplete(AnimationFinish);
    }

    private void AnimationFinish()
    {
        isAnimation = false;
        marubatu.gameObject.SetActive(false);
    }
}
