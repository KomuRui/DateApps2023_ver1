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

    [SerializeField] private string miniGameName;  //ミニゲームシーンの名前

    ////////////////////////////////////プレイヤー情報////////////////////////////////////////////

    [SerializeField] protected GameObject onePlayerParent;                                   //1人側プレイヤーの親オブジェクト
    [SerializeField] protected GameObject onePlayerChild;                                    //1人側プレイヤーの子オブジェクト
    [SerializeField] protected bool onePlayerParentDelete;                                   //1人側プレイヤーの親削除するか
    [SerializeField] protected Vector3 onePlayerPos;                                         //1人側プレイヤーの初期位置
    [SerializeField] protected Vector3 onePlayerScale;                                       //1人側プレイヤーの拡大率
    [SerializeField] protected Vector3 onePlayerRotate;                                      //1人側プレイヤーの角度
    [SerializeField] protected List<GameObject> threePlayerParent = new List<GameObject>();  //3人側プレイヤーの親オブジェクト
    [SerializeField] protected List<GameObject> threePlayerChild = new List<GameObject>();   //3人側プレイヤーの子オブジェクト
    [SerializeField] protected bool threePlayerParentDelete;                                 //3人側プレイヤーの親削除するか
    [SerializeField] protected List<Vector3> threePlayerPos = new List<Vector3>();           //3人側プレイヤーの初期位置
    [SerializeField] protected List<Vector3> threePlayerScale = new List<Vector3>();         //3人側プレイヤーの拡大率
    [SerializeField] protected List<Vector3> threePlayerRotate = new List<Vector3>();        //3人側プレイヤーの角度

    [SerializeField] protected Image onePlayerImage;                                                       //1人側プレイヤーの画像
    [SerializeField] protected List<Image> threePlayerImage = new List<Image>();                           //3人側プレイヤーの画像
    [SerializeField] protected Dictionary<byte,Image> playerImageTable = new Dictionary<byte, Image>();    //プレイヤーの画像

    [SerializeField] protected List<Vector3> rankAnnouncementPos = new List<Vector3>();     //ランク発表時のプレイヤー初期位置
    [SerializeField] protected List<Vector3> rankAnnouncementScale = new List<Vector3>();   //ランク発表時のプレイヤー拡大率
    [SerializeField] protected List<Vector3> rankAnnouncementRotate = new List<Vector3>();  //ランク発表時のプレイヤー角度

    public GameObject onePlayerObj;                                      //1人側プレイヤーオブジェクト
    public List<GameObject> threePlayerObj = new List<GameObject>();     //3人側プレイヤーオブジェクト
    protected bool isPlayerAllDead;                                                      //プレイヤーが全員死んでいるかどうか
    protected byte onePlayer;                                                            //1人側プレイヤー
    public Dictionary<byte, bool> threePlayer = new Dictionary<byte, bool>();         //3人側プレイヤー(boolは死んだかどうか)
    public Dictionary<byte, float> lifeTime = new Dictionary<byte, float>();          //3人側プレイヤーの生きてる時間

    ////////////////////////////////////カメラ////////////////////////////////////////////

    [SerializeField] protected Vector3 rankCameraPos;
    [SerializeField] protected Vector3 rankCameraRotate;

    ////////////////////////////////////必要UI////////////////////////////////////////////

    public GameObject endText;          //終了テキスト
    public GameObject rankText;         //順位テキスト
    public List<GameObject> killCanvas; //固有のキャンバス(各ミニゲームに表示してるUI,結果発表の時に消したいキャンバス)
    public TextMeshProUGUI redayText;   //準備のテキスト

    ////////////////////////////////////ミニゲーム情報////////////////////////////////////////////

    public Dictionary<byte, byte> nowMiniGameRank = new Dictionary<byte, byte>(); //現在のミニゲームのランク表(key : プレイヤー番号)
    protected bool isStart;             //ミニゲーム開始しているか
    protected bool isFinish;            //ミニゲームが終了しているか
    protected bool nowRankAnnouncement; //順位発表しているかどうか
    protected bool isWinPlayerPrint;    //勝利プレイヤーを表示しているか
    protected bool isTutorial;          //チュートリアルかどうか

    void Start()
    {
        /////////////////////////////////α版だけ
        if (!GameManager.isTitleStart)
        {
            PlayerManager.Initializ();
            ScoreManager.Initializ();
            TutorialManager.Initializ();
        }

        /////初期化
        GameManager.nowMiniGameManager = this;
        nowRankAnnouncement = false;　
        isPlayerAllDead = false;
        isStart = false;
        isFinish = false;
        isTutorial = true;

        //各プレイヤー番号設定
        onePlayer = PlayerManager.GetOnePlayer();

        List<byte> threeP = PlayerManager.GetThreePlayer();
        foreach (byte num in threeP)
        {
            threePlayer[num] = false;
            lifeTime[num] = 0;
        }

        //プレイヤーと画像生成
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

    //更新
    void Update()
    {
        //順位発表しているかつBボタンが押されたのなら
        if (nowRankAnnouncement && Input.GetButtonDown("Abutton1"))
        {
            StageSelectManager.NextRound();
            PlayerManager.NextOnePlayer();
            ScoreManager.ReCalcRank();
            SceneManager.LoadScene("MainMode");
        }

        //勝利側を発表しているかつBボタンが押されたのなら
        if (isWinPlayerPrint && Input.GetButtonDown("Abutton1"))
            ChangeRankAnnouncement();

        //生きている3人側の時間記録
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

        //継承先の更新
        MiniGameUpdate();
    }

    /////////////////////////////////プレイヤー//////////////////////////////////////

    public void SetOnePlayer(byte player) { onePlayer = player; }
    public void SetThreePlayer(byte player) { threePlayer[player] = false; }
    public void PlayerFinish(byte player) 
    { 
        //死んでいないのなら
        if(!threePlayer[player]) threePlayer[player] = true; 

        //1人でも死んでいなかったらこの先処理しない
        foreach(var item in threePlayer.Values)
            if(!item) return;

        //プレイヤー全員死んだに設定
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


    //プレイヤーすべて削除
    public void PlayerAllDelete()
    {
        //親がいるのなら
        if (onePlayerParent != null && onePlayerParentDelete)
            Destroy(onePlayerParent.gameObject);
        else
            Destroy(onePlayerObj.gameObject);

        //親がいるやつは消す
        for (int i = 0; i < threePlayerParent.Count; i++)
             if(threePlayerParentDelete) Destroy(threePlayerParent[i].gameObject);

        //親がいないやつを消す
        for (int i = 0; i < threePlayerObj.Count; i++)
            if (threePlayerObj[i] != null) Destroy(threePlayerObj[i].gameObject);
    }

    /////////////////////////////////ミニゲーム情報//////////////////////////////////////

    public bool IsPlayerAllDead() {  return isPlayerAllDead; }
    public bool IsStart() {  return isStart; }
    public bool IsFinish() { return isFinish; }

    //ミニゲーム開始にセット
    public void SetMiniGameStart() { isStart = true; MiniGameStart(); }

    //ミニゲーム終了にセット
    public void SetMiniGameFinish() 
    {
        //すでに終わっているならこの先処理しない
        if (isFinish) return;

        //チュートリアルなら
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

    //勝利して側のテキストを表示
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

    //順位発表に変更
    public void ChangeRankAnnouncement()
    {
        nowRankAnnouncement = true;
        isWinPlayerPrint = false;

        var sortedDictionary = nowMiniGameRank.OrderBy((pair) => pair.Value);
        Dictionary<byte,byte> rankTable = new Dictionary<byte,byte>();

        foreach (var rank in sortedDictionary)
            rankTable[rank.Key] = rank.Value;

        //順位テキスト表示
        for (byte i = 0; i < rankTable.Count; i++)
        {
            rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = rankTable.Values.ElementAt(i).ToString() + "位 " 
            + ScoreManager.GetRankScore(rankTable.Keys.ElementAt(i), rankTable.Values.ElementAt(i)) + "P";
        }
        Instantiate(rankText, new Vector3(0, 0, 0), Quaternion.identity);

        //新たにプレイヤー召喚
        for (byte i = 0; i < rankTable.Count; i++)
        {
            GameObject obj = (GameObject)Resources.Load("Prefabs/" + PlayerManager.GetPlayerVisual(rankTable.Keys.ElementAt(i)));
            obj = Instantiate(obj, rankAnnouncementPos[i], Quaternion.identity);
            obj.transform.position = rankAnnouncementPos[i];
            obj.transform.localScale = rankAnnouncementScale[i];
            obj.transform.localEulerAngles = rankAnnouncementRotate[i];
        }

        //カメラのもろもろ変更
        Camera.main.transform.position = rankCameraPos;
        Camera.main.transform.localEulerAngles = rankCameraRotate;

        //プレイヤーとテキスト削除
        PlayerAllDelete();
        endText.SetActive(false);
        foreach(var obj in killCanvas)
            obj.SetActive(false);
    }

    //シーン開始
    public virtual void SceneStart() {}

    //更新
    public virtual void MiniGameUpdate() {}

    //ゲーム終了時に呼ばれる
    public virtual void MiniGameFinish(){}

    //ゲーム開始時に呼ばれる
    public virtual void MiniGameStart(){}

    //プレイヤーが全員死んだときに呼ばれる関数
    public virtual void PlayerAllDead(){}
}
