using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{

    private byte onePlayer;                            //1�l���v���C���[
    private Dictionary<byte,bool> threePlayer;         //3�l���v���C���[(bool�͎��񂾂��ǂ���)

    private bool isPlayerAllDead;   //�v���C���[���S������ł��邩�ǂ���
    private bool isStart;           //�~�j�Q�[���J�n���Ă��邩
    private bool isFinish;          //�~�j�Q�[�����I�����Ă��邩

    void Start()
    {
        /////������
        threePlayer = new Dictionary<byte, bool>();
        isPlayerAllDead = false;
        isStart = false;
        isFinish = false;
    }

    /////////////////////////////////�v���C���[//////////////////////////////////////

    public void SetOnePlayer(byte player) { onePlayer = player; }
    public void SetThreePlayer(byte player) { threePlayer[player] = false; }
    public void playerDead(byte player) 
    { 
        //����ł��Ȃ��̂Ȃ�
        if(!threePlayer[player]) threePlayer[player] = true; 

        //1�l�ł�����ł��Ȃ������炱�̐揈�����Ȃ�
        foreach(var item in threePlayer.Values)
            if(!threePlayer[player]) return;

        isPlayerAllDead = true;
    }

    /////////////////////////////////�~�j�Q�[�����//////////////////////////////////////

    public bool IsPlayerAllDead() {  return isPlayerAllDead; }
    public bool IsStart() {  return isStart; }
    public bool IsFinish() { return isFinish; }

    //�~�j�Q�[���J�n�ɃZ�b�g
    public void SetMiniGameStart() { isStart = true; MiniGameStart(); }

    //�~�j�Q�[���I���ɃZ�b�g
    public void SetMiniGameFinish() { isFinish = true; isStart = false; MiniGameFinish(); }

    //�Q�[���I�����ɌĂ΂��
    public virtual void MiniGameFinish(){}

    //�Q�[���J�n���ɌĂ΂��
    public virtual void MiniGameStart(){}

    //�v���C���[���S�����񂾂Ƃ��ɌĂ΂��֐�
    public virtual void PlayerAllDead(){}
}
