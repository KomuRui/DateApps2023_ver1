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


//    //�W�����v
//    private void Jump()
//    {
//        //�W�����v���Ă���Ȃ炱�̐揈�����Ȃ�
//        if (isButton) return;

//        //�y���M������
//        if (Input.GetButtonDown("Abutton" + playerNum) && lockA <= 0)
//        {
//            lockA = 60;


//            //�ʏ��ԂɕύX
//            ChangeStateTo(SlimeAnimationState.Idle);

//            //��ɗ͂�������
//            rb.AddForce(Vector3.up * jumpPower);
//            isJump = true;
//            return;
//        }














//        int beforeStage = nowStageNum;

//        //�����W�����v(�ʂ̑����)
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

//        //�W�����v��ԂɕύX    
//        //ChangeStateTo(SlimeAnimationState.Jump);
//    }
//}
