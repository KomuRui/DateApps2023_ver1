using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class OnePlayer : MonoBehaviour
{
    public GameObject penguinP;
    public GameObject sharkP;
    public GameObject fishesP;
    public GameObject dolphinP;
    public float playerSpeed;

    Quaternion penguinRotate = Quaternion.Euler(0, -90, 90);
    Quaternion sharkRotate = Quaternion.Euler(0, 270, 0);
    Quaternion fishesRotate = Quaternion.Euler(0, 180, 0);
    Quaternion dolphinRotate = Quaternion.Euler(250, 180, 0);
    bool isPenguin;
    bool isShark;
    bool isFishes;
    bool isDolphin;
    bool isStop;
    bool isCoroutineStart;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(penguinP, this.transform.position, penguinRotate);
        isPenguin = true;
        isShark = true;
        isFishes = true;
        isDolphin = true;
        isStop = false;
        isCoroutineStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStop)
        {
            if (!isCoroutineStart) 
            {
                StartCoroutine(AllCoolCorou());
            }           
        }
        else if (Input.GetKeyDown(KeyCode.P) && isPenguin)
        {
            Instantiate(penguinP, new Vector3(this.transform.position.x, -0.53f, 10), penguinRotate);
            StartCoroutine(PenguinCoolCorou());
            isStop = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Instantiate(sharkP, new Vector3(this.transform.position.x, -1, 10), sharkRotate);

            isStop = true;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(fishesP, new Vector3(this.transform.position.x, -0.9f, 10), fishesRotate);

            isStop = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Instantiate(dolphinP, new Vector3(this.transform.position.x, -3, 10), dolphinRotate);

            isStop = true;
        }

        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 3.5)
        {
            transform.position += new Vector3(playerSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -3.5)
        {
            transform.position += new Vector3(-playerSpeed, 0, 0);
        }


        
    }


    //コルーチン関数を定義
    private IEnumerator AllCoolCorou() //コルーチン関数の名前
    {
        //オブジェクト表示（斜線）（禁止マーク）
        //objHu[transform.childCount - 1].GetComponent<MeshRenderer>().enabled = true;
        //クリックできなくする
        //click = false;
        //駒ごとの待ち時間
        isCoroutineStart = true;
        yield return new WaitForSeconds(0.2f);
        isStop = false;
        isCoroutineStart = false;
        //戻す
        //click = true;
        //オブジェクト非表示
        //objHu[transform.childCount - 1].GetComponent<MeshRenderer>().enabled = false;
    }

    private IEnumerator PenguinCoolCorou() //コルーチン関数の名前
    {
        //オブジェクト表示（斜線）（禁止マーク）
        //objHu[transform.childCount - 1].GetComponent<MeshRenderer>().enabled = true;
        //クリックできなくする
        //click = false;
        //駒ごとの待ち時間
        isPenguin = false;
        yield return new WaitForSeconds(1.0f);
        isPenguin = true;
        //戻す
        //click = true;
        //オブジェクト非表示
        //objHu[transform.childCount - 1].GetComponent<MeshRenderer>().enabled = false;
    }


    ////ジャンプ
    //private void Jump()
    //{
    //    //ジャンプしているならこの先処理しない
    //    if (isButton) return;

    //    //ペンギン発射
    //    if (Input.GetButtonDown("Abutton" + playerNum) && lockA <= 0)
    //    {
    //        lockA = 60;


    //        //通常状態に変更
    //        ChangeStateTo(SlimeAnimationState.Idle);

    //        //上に力を加える
    //        rb.AddForce(Vector3.up * jumpPower);
    //        isJump = true;
    //        return;
    //    }














    //    int beforeStage = nowStageNum;

    //    //自動ジャンプ(別の足場に)
    //    if (Input.GetAxis("L_Stick_V" + playerNum) < -0.8f)
    //    {
    //        nowStageNum--;
    //        nowStageNum = Math.Max(nowStageNum, 0);
    //        if (beforeStage == nowStageNum) return;
    //        transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
    //        ChangeStateTo(SlimeAnimationState.Idle);
    //        rb.AddForce(Vector3.up * jumpPower);
    //        isJump = true;
    //    }
    //    else if (Input.GetAxis("L_Stick_V" + playerNum) > 0.8f)
    //    {
    //        nowStageNum++;
    //        nowStageNum = Math.Min(nowStageNum, stage.Length - 1);
    //        if (beforeStage == nowStageNum) return;
    //        transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
    //        ChangeStateTo(SlimeAnimationState.Idle);
    //        rb.AddForce(Vector3.up * jumpPower);
    //        isJump = true;
    //    }

    //    //ジャンプ状態に変更    
    //    //ChangeStateTo(SlimeAnimationState.Jump);
    //}
}
