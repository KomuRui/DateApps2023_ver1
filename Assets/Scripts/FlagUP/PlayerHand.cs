using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private int playerNum;      // �v���C���[�ԍ�
    [SerializeField] public GameObject leftOb;   // ���̊�
    [SerializeField] public GameObject rightOb;  // �E�̊�

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ////�X�g�b�v���ĂȂ�&�����̃^�[��
        //if (flagUpGameManager.isStop == false && flagUpGameManager.isAloneTurn == false)
        //{
        //    if (isInput)
        //    {
        //        if (Input.GetButtonDown("LBbutton" + playerNum))
        //        {
        //            leftOb.transform.DORotate(Vector3.forward * -90f, 0.1f);
        //        }
        //        else if (Input.GetButtonDown("RBbutton" + playerNum))
        //        {
        //            rightOb.transform.DORotate(Vector3.forward * 90f, 0.1f);
        //        }
        //        else if (Input.GetButtonDown("Abutton" + playerNum))
        //        {
        //            leftOb.transform.DORotate(Vector3.forward * 0, 0.1f);
        //            rightOb.transform.DORotate(Vector3.forward * 0, 0.1f);
        //        }

        //        if (isFirst)
        //        {
        //            isFirst = false;
        //            //�グ��鎞��
        //            Invoke("StopPlayer", 5.0f);
        //        }
        //    }
        //    else
        //    {
        //        if (flagMax > flagUpNum && isFirst)
        //        {
        //            isFirst = false;

        //            //1�l���^�[���ɂ���
        //            flagUpGameManager.isAloneTurn = true;

        //            if (flagMax == flagUpNum)
        //            {

        //                //���グ��0�ɖ߂�
        //                flagUpNum = 0;
        //                //���E���h3�ȏゾ��������グ��5
        //                if (flagUpGameManager.roundNum >= 2)
        //                {
        //                    flagMax = 5;
        //                }

        //                flagUpGameManager.roundNum++;
        //            }

        //            //������Ȃ�����
        //            Invoke("PlayPlayer", 5.0f);
        //        }

        //        if(flagUpGameManager.roundNum >= 3)
        //        {
        //            flagUpGameManager.isStop = true;
        //        }
        //    }
        //}
    }

    //�グ��Ȃ�
    void StopPlayer()
    {
        //isInput = false;
        //isFirst = true;
        //flagUpNum++;
        
        ////1�l���^�[���ɂ���
        //flagUpGameManager.isAloneTurn = true;

        //if (flagMax == flagUpNum)
        //{
            
        //    //���グ��0�ɖ߂�
        //    flagUpNum = 0;
        //    //���E���h3�ȏゾ��������グ��5
        //    if (flagUpGameManager.roundNum >= 2)
        //    {
        //        flagMax = 5;
        //    }
        //}
    }

    //�グ���
    void PlayPlayer()
    {
        //�J�炷


        //isInput = true;
        //isFirst = true;
    }
}
