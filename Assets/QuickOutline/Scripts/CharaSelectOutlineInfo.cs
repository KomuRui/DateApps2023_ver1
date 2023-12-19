using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharaSelectOutlineInfo : MonoBehaviour
{

    public Animator animator;
    private bool isSelect = false;     //選択されているかどうか
    private byte selectPlayerNum = 0;  //選択しているプレイヤーの番号
    public CharaSelectManager.LineNum line; //自分がどのラインか
    public int num;                         //何番目か

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //選択する
    //bool : 選択できたかどうか
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

    //選択を解除する
    //bool : 解除できたかどうか
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

    //選択
    public void Select()
    {
        
    }
}
