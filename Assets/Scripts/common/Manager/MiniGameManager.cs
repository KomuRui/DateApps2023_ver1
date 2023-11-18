using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] protected GameObject onePlayerParent;                                   //1�l���v���C���[�̐e�I�u�W�F�N�g(Player1,PLayer2....�݂����Ȃ��)
    [SerializeField] protected List<GameObject> threePlayerParent = new List<GameObject>();  //3�l���v���C���[�̐e�I�u�W�F�N�g(Player1,PLayer2....�݂����Ȃ��)

    protected byte onePlayer;                                                           //1�l���v���C���[
    protected Dictionary<byte,bool> threePlayer = new Dictionary<byte, bool>();         //3�l���v���C���[(bool�͎��񂾂��ǂ���)

    protected bool isPlayerAllDead;   //�v���C���[���S������ł��邩�ǂ���
    protected bool isStart;           //�~�j�Q�[���J�n���Ă��邩
    protected bool isFinish;          //�~�j�Q�[�����I�����Ă��邩

    void Start()
    {
        /////������
        isPlayerAllDead = false;
        isStart = false;
        isFinish = false;

        //�e�v���C���[�ԍ��ݒ�
        onePlayer = PlayerManager.GetOnePlayer();

        List<byte> threeP = PlayerManager.GetThreePlayer();
        foreach(byte num in threeP)
            threePlayer[num] = false;

        //�v���C���[����
        GameObject obj = Instantiate(PlayerManager.GetPlayerVisual(onePlayer), this.transform.position, Quaternion.identity);
        obj.transform.parent = onePlayerParent.transform;

        int i = 0;
        foreach (byte num in threePlayer.Keys)
        {
           obj = Instantiate(PlayerManager.GetPlayerVisual(num), this.transform.position, Quaternion.identity);
           obj.transform.parent = threePlayerParent[i].transform;
           i++;
        }

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

        //�v���C���[�S�����񂾂ɐݒ�
        isPlayerAllDead = true;
        PlayerAllDead();
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
