using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharaSelectOutlineInfo : MonoBehaviour
{

    public Animator animator;
    private bool isSelect = false;     //選択されているかどうか
    public bool isAnimation = false;   //アニメーション中かどうか
    private byte selectPlayerNum = 0;  //選択しているプレイヤーの番号
    public CharaSelectManager.LineNum line; //自分がどのラインか
    public int num;                         //何番目か
    private Vector3 initialRotate;          //初期回転
    public GameObject marubatu;             //◎×
    public GameObject marubatuParent;       //◎×の親

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

    //選択する
    //bool : 選択できたかどうか
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

    //選択を解除する
    //bool : 解除できたかどうか
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

    //選択
    public void Select()
    {
        isAnimation = true;
        this.transform.DORotate(new Vector3(initialRotate.x, initialRotate.y + 360.0f, initialRotate.z), 1.5f, RotateMode.FastBeyond360).SetEase(Ease.InOutBack).OnComplete(SelectMove);
    }

    //選択の時の移動
    private void SelectMove()
    {
        marubatu.gameObject.SetActive(true);
        marubatuParent.transform.DORotateQuaternion(Quaternion.AngleAxis(140, marubatu.transform.right) * marubatuParent.transform.rotation, 0.5f).OnComplete(() => isAnimation = false);
    }

    //解除
    public void Release()
    {
        isAnimation = true;
        ReleaseMove();
    }

    //選択の時の移動
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
