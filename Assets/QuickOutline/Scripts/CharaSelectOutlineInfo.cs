using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharaSelectOutlineInfo : MonoBehaviour
{

    public Animator animator;
    private bool isSelect = false;     //�I������Ă��邩�ǂ���
    private byte selectPlayerNum = 0;  //�I�����Ă���v���C���[�̔ԍ�
    public CharaSelectManager.LineNum line; //�������ǂ̃��C����
    public int num;                         //���Ԗڂ�

    // Start is called before the first frame update
    void Start()
    {
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
            this.GetComponent<Outline>().OutlineColor = outlineColor;
            this.GetComponent<Outline>().enabled = true;
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
            this.GetComponent<Outline>().enabled = false;
            selectPlayerNum = 0;
            isSelect = false;
            return true;
        }
    }

    //�I��
    public void Select()
    {
        
    }
}
