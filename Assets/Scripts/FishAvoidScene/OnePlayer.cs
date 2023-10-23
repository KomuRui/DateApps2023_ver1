//using DG.Tweening;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.SceneManagement;
//using UnityEngine;

//public class OnePlayer : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }


//    //ジャンプ
//    private void Jump()
//    {
//        //ジャンプしているならこの先処理しない
//        if (isButton) return;

//        //ペンギン発射
//        if (Input.GetButtonDown("Abutton" + playerNum) && lockA <= 0)
//        {
//            lockA = 60;


//            //通常状態に変更
//            ChangeStateTo(SlimeAnimationState.Idle);

//            //上に力を加える
//            rb.AddForce(Vector3.up * jumpPower);
//            isJump = true;
//            return;
//        }














//        int beforeStage = nowStageNum;

//        //自動ジャンプ(別の足場に)
//        if (Input.GetAxis("L_Stick_V" + playerNum) < -0.8f)
//        {
//            nowStageNum--;
//            nowStageNum = Math.Max(nowStageNum, 0);
//            if (beforeStage == nowStageNum) return;
//            transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
//            ChangeStateTo(SlimeAnimationState.Idle);
//            rb.AddForce(Vector3.up * jumpPower);
//            isJump = true;
//        }
//        else if (Input.GetAxis("L_Stick_V" + playerNum) > 0.8f)
//        {
//            nowStageNum++;
//            nowStageNum = Math.Min(nowStageNum, stage.Length - 1);
//            if (beforeStage == nowStageNum) return;
//            transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
//            ChangeStateTo(SlimeAnimationState.Idle);
//            rb.AddForce(Vector3.up * jumpPower);
//            isJump = true;
//        }

//        //ジャンプ状態に変更    
//        //ChangeStateTo(SlimeAnimationState.Jump);
//    }
//}
