using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePlayerHand : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�
    public GameObject leftOb;
    public GameObject rightOb;
    public GameObject GMOb;

    public bool isInput;//1��
    public bool isFirst;//1��

    private int flagUpNum;
    private int flagMax;

    FlagUpGameManager flagUpGameManager;

    // Start is called before the first frame update
    void Start()
    {
        //isInput = true;
        //isFirst = true;
        //flagUpNum = 0;
        //flagMax = 3;
        ////�J�n�܂�
        //Invoke("PlayPlayer",5.0f);
        //flagUpGameManager = GMOb.GetComponent<FlagUpGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //�X�g�b�v���ĂȂ�&�����̃^�[��
        //if(flagUpGameManager.isStop == false && flagUpGameManager.isAloneTurn == true)
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
        //            leftOb.transform.DORotate(Vector3.forward * 0f, 0.1f);
        //            rightOb.transform.DORotate(Vector3.forward * 0f, 0.1f);
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

        //            //3�l���^�[���ɂ���
        //            flagUpGameManager.isAloneTurn = false;
        //            Debug.Log("3�l����");

        //            if (flagMax == flagUpNum)
        //            {
        //                //���グ��0�ɖ߂�
        //                flagUpNum = 0;
        //                //���E���h3�ȏゾ��������グ��5
        //                if (flagUpGameManager.roundNum >= 2)
        //                {
        //                    flagMax = 5;
        //                }
        //            }

        //            //������Ȃ�����
        //            Invoke("PlayPlayer", 5.0f);
        //        }
        //    }
        //}

        
    }

    //�グ��Ȃ�
    void StopPlayer()
    {
        //isInput = false;
        //isFirst = true;
       // flagUpNum++;

        
    }

    //�グ���
    void PlayPlayer()
    {
        //�J�炷


        //isInput = true;
        //isFirst = true;
    }
}
