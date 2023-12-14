using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.AI;

public class Net : MonoBehaviour
{

    [SerializeField] private Vector3 impositionScale;  //発動した後の拡大率
    [SerializeField] private Transform impositionBase; //発動した後の基になるtransform
    [SerializeField] private float moveSpeed = 5.0f;   //移動速度
    [SerializeField] private NetCollider netCollider;  //網のコライダー
    [SerializeField] private GameObject netMark;       //網のマーカー
    private Vector3 initialPos;    //初期位置
    private Vector3 initialScale;  //初期拡大率
    private float startTime;       //移動開始時間
    private bool isNetMove;        //ネット移動中か
    private bool isNetImposition = false;  //ネット発動してるか
    private bool isNetReturn = false;      //ネットを元に戻しているか
    private List<GameObject> getFish = new List<GameObject>();  //取得した魚

    // Start is called before the first frame update
    void Start()
    {
        //位置と拡大率を保存しておく
        initialPos = transform.position;
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //網発動
        if (!isNetImposition && Input.GetButtonDown("Abutton" + transform.parent.GetComponent<PlayerNum>().playerNum)) NetExecute();

        //移動中なら計算
        if (isNetMove) NetPosCalc();

    }

    //網を発動
    public void NetExecute()
    {
        //魚を一匹も捕まえれなかったら
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

        //捕まえた魚をすべて覚えておく
        getFish = netCollider.fishObj;

        //網に当たっている魚を捕まえる
        foreach (var fish in netCollider.fishObj)
        {
            fish.layer = 9;
            fish.GetComponent<NavMeshAgent>().enabled = false;
            fish.transform.parent = transform.parent;
        }

        //網のマーカーに当たり判定をつける
        //netMark.GetComponent<NavMeshObstacle>().enabled = true;
        netMark.GetComponent<MeshCollider>().enabled = true;

        //もろもろ設定
        transform.parent.GetComponent<DriveChaseFishPlayer>().isMove = false;
        startTime = Time.time;
        isNetImposition = true;
        isNetMove = true;
        isNetReturn = false;
        this.transform.DOScale(new Vector3(impositionScale.x, initialScale.y,impositionScale.z), 1f).OnComplete(ParentMoveOK);
    }

    //網の位置計算
    private void NetPosCalc()
    {
        float distCovered = (Time.time - startTime) / moveSpeed; // 移動した距離を計算

        if(isNetReturn)
            transform.position = Vector3.Lerp(transform.position, transform.parent.GetComponent<DriveChaseFishPlayer>().transform.position, distCovered);  //移動
        else
            transform.position = Vector3.Lerp(transform.position, impositionBase.position, distCovered); //移動
    }

    //親の移動を許可
    private void ParentMoveOK() { transform.parent.GetComponent<DriveChaseFishPlayer>().isMove = true; isNetMove = false; }

    //親の移動を許可
    private void ParentMoveOK2() { transform.parent.GetComponent<DriveChaseFishPlayer>().isMove = true; isNetMove = false; isNetImposition = false; }


    //網もとに戻す
    private void NetReturn()
    {
        startTime = Time.time;
        isNetReturn = true;
        this.transform.DOScale(new Vector3(initialScale.x, initialScale.y, initialScale.z), 1f).OnComplete(ParentMoveOK2);
    }

}
