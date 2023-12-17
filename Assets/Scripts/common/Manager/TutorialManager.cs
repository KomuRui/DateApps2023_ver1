using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public static class TutorialManager 
{

    private static Dictionary<int, bool> playerReadyOK = new Dictionary<int, bool>();
    public static float tutorialTime = 45;
    public static bool isTutorialFinish = false;

    ///test�p
    public static bool isInitializOK = false;

    //������
    public static void Initializ()
    {
        //����������Ă����炱�̐揈�����Ȃ�
        if (isInitializOK) return;

        //�v���C���[�ԍ���������
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            playerReadyOK[i + 1] = false;

        isInitializOK = true;
    }

    //�X�V
    public static void Update()
    {
        //���������ǂ����`�F�b�N����
        if (Input.GetButtonDown("Startbutton1"))
            SetPlayerReadyOK(1);
        if (Input.GetButtonDown("Startbutton2"))
            SetPlayerReadyOK(2);
        if (Input.GetButtonDown("Startbutton3"))
            SetPlayerReadyOK(3);
        if (Input.GetButtonDown("Startbutton4"))
            SetPlayerReadyOK(4);


        //���Ԍv��
        tutorialTime -= Time.deltaTime;
        tutorialTime = Mathf.Max(tutorialTime, 0);
        if (tutorialTime <= 1)
        {
            GameManager.nowMiniGameManager.TutorialFinish();
        }
    }


    //�v���C���[������OK�ɐݒ�
    public static void SetPlayerReadyOK(int playerNum) { playerReadyOK[playerNum] = true; }

    //����OK���Ă���v���C���[���擾
    public static int GetReadyOKSum()
    {
       int sum = 0;

        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            if (playerReadyOK[i + 1]) sum++;

        return sum;
    }

}
