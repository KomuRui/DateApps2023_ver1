using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Net : MonoBehaviour
{

    [SerializeField] private Vector3 impositionScale; //発動した後の拡大率
    private Vector3 impositionPos; //発動した後の位置
    private Vector3 impositionPos2;//発動した後の位置2
    private Vector3 initialPos;    //初期位置
    private Vector3 initialScale;  //初期拡大率

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
        
    }

    //網を発動
    public void NetExecute()
    {
        
    }
}
