using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{

    [SerializeField] private string miniGameName;  //�~�j�Q�[���V�[���̖��O

    ////////////////////////////////////�v���C���[���////////////////////////////////////////////

    [SerializeField] protected GameObject onePlayerParent;                                   //1�l���v���C���[�̐e�I�u�W�F�N�g
    [SerializeField] protected GameObject onePlayerChild;                                    //1�l���v���C���[�̎q�I�u�W�F�N�g
    [SerializeField] protected bool onePlayerParentDelete;                                   //1�l���v���C���[�̐e�폜���邩
    [SerializeField] protected Vector3 onePlayerPos;                                         //1�l���v���C���[�̏����ʒu
    [SerializeField] protected Vector3 onePlayerScale;                                       //1�l���v���C���[�̊g�嗦
    [SerializeField] protected Vector3 onePlayerRotate;                                      //1�l���v���C���[�̊p�x
    [SerializeField] protected List<GameObject> threePlayerParent = new List<GameObject>();  //3�l���v���C���[�̐e�I�u�W�F�N�g
    [SerializeField] protected List<GameObject> threePlayerChild = new List<GameObject>();   //3�l���v���C���[�̎q�I�u�W�F�N�g
    [SerializeField] protected bool threePlayerParentDelete;                                 //3�l���v���C���[�̐e�폜���邩
    [SerializeField] protected List<Vector3> threePlayerPos = new List<Vector3>();           //3�l���v���C���[�̏����ʒu
    [SerializeField] protected List<Vector3> threePlayerScale = new List<Vector3>();         //3�l���v���C���[�̊g�嗦
    [SerializeField] protected List<Vector3> threePlayerRotate = new List<Vector3>();        //3�l���v���C���[�̊p�x

    [SerializeField] protected Image onePlayerImage;                                                       //1�l���v���C���[�̉摜
    [SerializeField] protected List<Image> threePlayerImage = new List<Image>();                           //3�l���v���C���[�̉摜
    [SerializeField] protected Dictionary<byte,Image> playerImageTable = new Dictionary<byte, Image>();    //�v���C���[�̉摜

    [SerializeField] protected List<Vector3> rankAnnouncementPos = new List<Vector3>();     //�����N���\���̃v���C���[�����ʒu
    [SerializeField] protected List<Vector3> rankAnnouncementScale = new List<Vector3>();   //�����N���\���̃v���C���[�g�嗦
    [SerializeField] protected List<Vector3> rankAnnouncementRotate = new List<Vector3>();  //�����N���\���̃v���C���[�p�x

    public GameObject onePlayerObj;                                      //1�l���v���C���[�I�u�W�F�N�g
    public List<GameObject> threePlayerObj = new List<GameObject>();     //3�l���v���C���[�I�u�W�F�N�g
    protected bool isPlayerAllDead;                                                      //�v���C���[���S������ł��邩�ǂ���
    protected byte onePlayer;                                                            //1�l���v���C���[
    public Dictionary<byte, bool> threePlayer = new Dictionary<byte, bool>();         //3�l���v���C���[(bool�͎��񂾂��ǂ���)
    public Dictionary<byte, float> lifeTime = new Dictionary<byte, float>();          //3�l���v���C���[�̐����Ă鎞��

    ////////////////////////////////////�J����////////////////////////////////////////////

    [SerializeField] protected Vector3 rankCameraPos;
    [SerializeField] protected Vector3 rankCameraRotate;

    ////////////////////////////////////�K�vUI////////////////////////////////////////////

    public GameObject endText;          //�I���e�L�X�g
    public GameObject rankText;         //���ʃe�L�X�g
    public List<GameObject> killCanvas; //�ŗL�̃L�����o�X(�e�~�j�Q�[���ɕ\�����Ă�UI,���ʔ��\�̎��ɏ��������L�����o�X)
    public TextMeshProUGUI redayText;   //�����̃e�L�X�g

    ////////////////////////////////////�~�j�Q�[�����////////////////////////////////////////////

    public Dictionary<byte, byte> nowMiniGameRank = new Dictionary<byte, byte>(); //���݂̃~�j�Q�[���̃����N�\(key : �v���C���[�ԍ�)
    protected bool isStart;             //�~�j�Q�[���J�n���Ă��邩
    protected bool isFinish;            //�~�j�Q�[�����I�����Ă��邩
    protected bool nowRankAnnouncement; //���ʔ��\���Ă��邩�ǂ���
    protected bool isWinPlayerPrint;    //�����v���C���[��\�����Ă��邩
    protected bool isTutorial;          //�`���[�g���A�����ǂ���

    void Start()
    {
        /////////////////////////////////���ł���
        if (!GameManager.isTitleStart)
        {
            PlayerManager.Initializ();
            ScoreManager.Initializ();
            TutorialManager.Initializ();
        }

        /////������
        GameManager.nowMiniGameManager = this;
        nowRankAnnouncement = false;�@
        isPlayerAllDead = false;
        isStart = false;
        isFinish = false;
        isTutorial = true;

        //�e�v���C���[�ԍ��ݒ�
        onePlayer = PlayerManager.GetOnePlayer();

        List<byte> threeP = PlayerManager.GetThreePlayer();
        foreach (byte num in threeP)
        {
            threePlayer[num] = false;
            lifeTime[num] = 0;
        }

        //�v���C���[�Ɖ摜����
        onePlayerObj = (GameObject)Resources.Load("Prefabs/" + miniGameName + "/One/" + PlayerManager.GetPlayerVisual(onePlayer));
        onePlayerObj = Instantiate(onePlayerObj, onePlayerPos, Quaternion.identity);
        onePlayerObj.transform.position = onePlayerPos;
        onePlayerObj.transform.localScale = onePlayerScale;
        onePlayerObj.transform.localEulerAngles = onePlayerRotate;
        onePlayerObj.transform.GetComponent<PlayerNum>().playerNum = onePlayer;

        if (onePlayerParent != null)
            onePlayerObj.transform.parent = onePlayerParent.transform;

        if (onePlayerChild != null)
            onePlayerChild.transform.parent = onePlayerObj.transform;

        if (onePlayerImage != null)
        {
            onePlayerImage.sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage(onePlayer));
            playerImageTable[onePlayer] = onePlayerImage;
        }

        int lookNum = 0;
        foreach (byte num in threePlayer.Keys)
        {
            threePlayerObj.Add((GameObject)Resources.Load("Prefabs/" + miniGameName + "/Three/" + PlayerManager.GetPlayerVisual(num)));
            threePlayerObj[lookNum] = Instantiate(threePlayerObj[lookNum], this.transform.position, Quaternion.identity);
            threePlayerObj[lookNum].transform.position = threePlayerPos[lookNum];
            threePlayerObj[lookNum].transform.localScale = threePlayerScale[lookNum];
            threePlayerObj[lookNum].transform.localEulerAngles = threePlayerRotate[lookNum];
            threePlayerObj[lookNum].transform.GetComponent<PlayerNum>().playerNum = num;

            if (lookNum < threePlayerParent.Count)
                threePlayerObj[lookNum].transform.parent = threePlayerParent[lookNum].transform;

            if (lookNum < threePlayerChild.Count)
                threePlayerChild[lookNum].transform.parent = threePlayerObj[lookNum].transform;

            if (threePlayerImage[lookNum] != null)
            {
                threePlayerImage[lookNum].sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage(num));
                playerImageTable[num] = threePlayerImage[lookNum];
            }
            lookNum++;
        }

        SceneStart();
    }

    //�X�V
    void Update()
    {
        //���ʔ��\���Ă��邩��B�{�^���������ꂽ�̂Ȃ�
        if (nowRankAnnouncement && Input.GetButtonDown("Abutton1"))
        {
            StageSelectManager.NextRound();
            PlayerManager.NextOnePlayer();
            ScoreManager.ReCalcRank();
            SceneManager.LoadScene("MainMode");
        }

        //�������𔭕\���Ă��邩��B�{�^���������ꂽ�̂Ȃ�
        if (isWinPlayerPrint && Input.GetButtonDown("Abutton1"))
            ChangeRankAnnouncement();

        //�����Ă���3�l���̎��ԋL�^
        if (!isTutorial)
        {
            foreach (byte num in threePlayer.Keys)
                if (!threePlayer[num]) lifeTime[num] += Time.deltaTime;
        }
        else if(redayText != null)
        {
            TutorialManager.Update();
            redayText.text = TutorialManager.GetReadyOKSum() + "/" + PlayerManager.PLAYER_MAX + " ok " + ((int)TutorialManager.tutorialTime).ToString();
        }

        //�p����̍X�V
        MiniGameUpdate();
    }

    /////////////////////////////////�v���C���[//////////////////////////////////////

    public void SetOnePlayer(byte player) { onePlayer = player; }
    public void SetThreePlayer(byte player) { threePlayer[player] = false; }
    public void PlayerFinish(byte player) 
    { 
        //����ł��Ȃ��̂Ȃ�
        if(!threePlayer[player]) threePlayer[player] = true; 

        //1�l�ł�����ł��Ȃ������炱�̐揈�����Ȃ�
        foreach(var item in threePlayer.Values)
            if(!item) return;

        //�v���C���[�S�����񂾂ɐݒ�
        isPlayerAllDead = true;
        PlayerAllDead();
        SetMiniGameFinish();
    }

    public void PlayerDead(byte player)
    {
        Color c = playerImageTable[player].color;
        c.r = 0.2f;
        c.g = 0.2f;
        c.b = 0.2f;
        playerImageTable[player].color = c;
    }

    public void PlayerHeal(byte player)
    {
        Color c = playerImageTable[player].color;
        c.r = 1.0f;
        c.g = 1.0f;
        c.b = 1.0f;
        playerImageTable[player].color = c;
    }


    //�v���C���[���ׂč폜
    public void PlayerAllDelete()
    {
        //�e������̂Ȃ�
        if (onePlayerParent != null && onePlayerParentDelete)
            Destroy(onePlayerParent.gameObject);
        else
            Destroy(onePlayerObj.gameObject);

        //�e�������͏���
        for (int i = 0; i < threePlayerParent.Count; i++)
             if(threePlayerParentDelete) Destroy(threePlayerParent[i].gameObject);

        //�e�����Ȃ��������
        for (int i = 0; i < threePlayerObj.Count; i++)
            if (threePlayerObj[i] != null) Destroy(threePlayerObj[i].gameObject);
    }

    /////////////////////////////////�~�j�Q�[�����//////////////////////////////////////

    public bool IsPlayerAllDead() {  return isPlayerAllDead; }
    public bool IsStart() {  return isStart; }
    public bool IsFinish() { return isFinish; }

    //�~�j�Q�[���J�n�ɃZ�b�g
    public void SetMiniGameStart() { isStart = true; MiniGameStart(); }

    //�~�j�Q�[���I���ɃZ�b�g
    public void SetMiniGameFinish() 
    {
        //���łɏI����Ă���Ȃ炱�̐揈�����Ȃ�
        if (isFinish) return;

        //�`���[�g���A���Ȃ�
        if(isTutorial)
        {
            SceneManager.LoadScene(miniGameName);
            return;
        }
        
        isFinish = true; 
        isStart = false;
        endText = Instantiate(endText, new Vector3(0, 0, 0), Quaternion.identity);
        MiniGameFinish();

        Invoke("WinPlayerTextPrint", 2.0f);
    }

    //�������đ��̃e�L�X�g��\��
    public void WinPlayerTextPrint()
    {
        bool isOnePlayerWin = false;
        endText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontSize = 25;

        foreach (var rank in nowMiniGameRank)
            if (rank.Key == onePlayer && rank.Value == 1) isOnePlayerWin = true;

        if (isOnePlayerWin)
            endText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "WinOnePlayer";
        else
            endText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "WinThreePlayer";

        isWinPlayerPrint = true;
    }

    //���ʔ��\�ɕύX
    public void ChangeRankAnnouncement()
    {
        nowRankAnnouncement = true;
        isWinPlayerPrint = false;

        var sortedDictionary = nowMiniGameRank.OrderBy((pair) => pair.Value);
        Dictionary<byte,byte> rankTable = new Dictionary<byte,byte>();

        foreach (var rank in sortedDictionary)
            rankTable[rank.Key] = rank.Value;

        //���ʃe�L�X�g�\��
        for (byte i = 0; i < rankTable.Count; i++)
        {
            rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = rankTable.Values.ElementAt(i).ToString() + "�� " 
            + ScoreManager.GetRankScore(rankTable.Keys.ElementAt(i), rankTable.Values.ElementAt(i)) + "P";
        }
        Instantiate(rankText, new Vector3(0, 0, 0), Quaternion.identity);

        //�V���Ƀv���C���[����
        for (byte i = 0; i < rankTable.Count; i++)
        {
            GameObject obj = (GameObject)Resources.Load("Prefabs/" + PlayerManager.GetPlayerVisual(rankTable.Keys.ElementAt(i)));
            obj = Instantiate(obj, rankAnnouncementPos[i], Quaternion.identity);
            obj.transform.position = rankAnnouncementPos[i];
            obj.transform.localScale = rankAnnouncementScale[i];
            obj.transform.localEulerAngles = rankAnnouncementRotate[i];
        }

        //�J�����̂������ύX
        Camera.main.transform.position = rankCameraPos;
        Camera.main.transform.localEulerAngles = rankCameraRotate;

        //�v���C���[�ƃe�L�X�g�폜
        PlayerAllDelete();
        endText.SetActive(false);
        foreach(var obj in killCanvas)
            obj.SetActive(false);
    }

    //�V�[���J�n
    public virtual void SceneStart() {}

    //�X�V
    public virtual void MiniGameUpdate() {}

    //�Q�[���I�����ɌĂ΂��
    public virtual void MiniGameFinish(){}

    //�Q�[���J�n���ɌĂ΂��
    public virtual void MiniGameStart(){}

    //�v���C���[���S�����񂾂Ƃ��ɌĂ΂��֐�
    public virtual void PlayerAllDead(){}
}
