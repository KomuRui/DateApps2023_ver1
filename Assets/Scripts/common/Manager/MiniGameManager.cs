using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{

    [SerializeField] public string miniGameName;  //ミニゲームシーンの名前

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
    [SerializeField] protected Image onePlayerImageTutorial;                                               //1人側プレイヤーの画像(チュートリアル用)
    [SerializeField] protected List<Image> threePlayerImage = new List<Image>();                           //3人側プレイヤーの画像
    [SerializeField] protected List<Image> threePlayerImageTutorial = new List<Image>();                   //3人側プレイヤーの画像(チュートリアル用)
    [SerializeField] protected Dictionary<byte,Image> playerImageTable = new Dictionary<byte, Image>();    //プレイヤーの画像
    [SerializeField] protected List<Image> tutorialPlayerImage = new List<Image>();

    [SerializeField] protected List<Vector3> rankAnnouncementPos = new List<Vector3>();     //ランク発表時のプレイヤー初期位置
    [SerializeField] protected List<Vector3> rankAnnouncementScale = new List<Vector3>();   //ランク発表時のプレイヤー拡大率
    [SerializeField] protected List<Vector3> rankAnnouncementRotate = new List<Vector3>();  //ランク発表時のプレイヤー角度

    public GameObject onePlayerObj;                                      //1人側プレイヤーオブジェクト
    public List<GameObject> threePlayerObj = new List<GameObject>();     //3人側プレイヤーオブジェクト
    protected bool isPlayerAllDead;                                                      //プレイヤーが全員死んでいるかどうか
    public byte onePlayer;                                                            //1人側プレイヤー
    public Dictionary<byte, bool> threePlayer = new Dictionary<byte, bool>();         //3人側プレイヤー(boolは死んだかどうか)
    public Dictionary<byte, float> lifeTime = new Dictionary<byte, float>();          //3人側プレイヤーの生きてる時間

    ////////////////////////////////////カメラ////////////////////////////////////////////

    [SerializeField] protected Vector3 rankCameraPos;
    [SerializeField] protected Vector3 rankCameraRotate;

    ////////////////////////////////////必要UI////////////////////////////////////////////

    public GameObject endText;          //終了テキスト
    public GameObject rankText;         //順位テキスト
    public List<GameObject> killCanvas; //固有のキャンバス(各ミニゲームに表示してるUI,結果発表の時に消したいキャンバス)
    public List<GameObject> okImage;    //okの画像
    public GameObject renderImage;      //描画用の画像

    ////////////////////////////////////ミニゲーム情報////////////////////////////////////////////

    public Dictionary<byte, byte> nowMiniGameRank = new Dictionary<byte, byte>(); //現在のミニゲームのランク表(key : プレイヤー番号)
    protected bool isStart;             //ミニゲーム開始しているか
    protected bool isFinish;            //ミニゲームが終了しているか
    protected bool nowRankAnnouncement; //順位発表しているかどうか
    public bool isTutorialUse;          //チュートリアルを使うかどうか

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

        //チュートリアルを使わないのなら
        if(!isTutorialUse)
            TutorialManager.isTutorialFinish = true;

        //チュートリアルが終わっているのなら
        if (TutorialManager.isTutorialFinish)
        {
            // 新しいビューポート領域を設定する
            Rect newViewportRect = new Rect(0, 0, 1, 1);
            Camera.main.rect = newViewportRect;
            Camera.main.targetTexture = null;

            //有効と無効切り替え
            for (int i = 0; i < threePlayerImage.Count; i++)
            {
                if(threePlayerImage[i]) threePlayerImage[i].gameObject.SetActive(true);
                
            }
            //有効と無効切り替え
            for (int i = 0; i < threePlayerImageTutorial.Count; i++)
            {
                if (threePlayerImageTutorial[i]) threePlayerImageTutorial[i].gameObject.SetActive(false);
            }

            //1人側の画像があるのなら
            if (onePlayerImage != null)
            {
                onePlayerImage.gameObject.SetActive(true);
                onePlayerImageTutorial.gameObject.SetActive(false);
            }

            //描画用の画像を無効に
            if(renderImage != null) renderImage.SetActive(false);
        }
        else
        {
            onePlayerImage = onePlayerImageTutorial;
            threePlayerImage = threePlayerImageTutorial;
        }

        //カウントダウンとタイマーを設定する
        if(this.GetComponent<CountDownAndTimer>())
            this.GetComponent<CountDownAndTimer>().SetCountDownAndTimer();

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
        tutorialPlayerImage[0].sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage(onePlayer));

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
            tutorialPlayerImage[lookNum + 1].sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage(num));

            if (lookNum < threePlayerParent.Count)
                threePlayerObj[lookNum].transform.parent = threePlayerParent[lookNum].transform;

            if (lookNum < threePlayerChild.Count)
                threePlayerChild[lookNum].transform.parent = threePlayerObj[lookNum].transform;

            if (threePlayerImage.Count > lookNum && threePlayerImage[lookNum] != null)
            {
                threePlayerImage[lookNum].sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage(num));
                playerImageTable[num] = threePlayerImage[lookNum];
            }
            lookNum++;
        }

        this.GetComponent<CountDownAndTimer>().CountDownStart();
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

        //生きている3人側の時間記録
        if (TutorialManager.isTutorialFinish)
        {
            foreach (byte num in threePlayer.Keys)
                if (!threePlayer[num]) lifeTime[num] += Time.deltaTime;
        }
        //チュートリアルが終わってないのなら
        else if(okImage != null && !TutorialManager.isTutorialFinish)
        {
            //時間と準備人数を表示
            TutorialManager.Update();

            //全員が準備できたのなら
            if (TutorialManager.GetReadyOKSum() >= PlayerManager.PLAYER_MAX)
                TutorialFinish();
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
        if (playerImageTable == null || !TutorialManager.isTutorialFinish) return;

        Color c = playerImageTable[player].color;
        c.r = 0.2f;
        c.g = 0.2f;
        c.b = 0.2f;
        playerImageTable[player].color = c;
    }

    public void PlayerHeal(byte player)
    {
        if (playerImageTable == null || !TutorialManager.isTutorialFinish) return;

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
        if(!TutorialManager.isTutorialFinish)
        {
            SceneManager.LoadScene(miniGameName);
            return;
        }
        
        isFinish = true; 
        isStart = false;
        endText = Instantiate(endText, new Vector3(0, 0, 0), Quaternion.identity);
        MiniGameFinish();

        //勝利プレイヤーの演出に移行
        Invoke("WinPlayerDirectingChange", 2.0f);
    }

    //勝利プレイヤーの演出に移行
    public void WinPlayerDirectingChange()
    {
        //どっちが勝利したか求める
        bool isOnePlayerWin = false;
        foreach (var rank in nowMiniGameRank)
            if (rank.Key == onePlayer && rank.Value == 1) isOnePlayerWin = true;

        //フェードイン
        this.GetComponent<MiniGameWinPlayerInfo>().FadeIn(isOnePlayerWin);
    }

    //順位発表に変更
    public void ChangeRankAnnouncement()
    {
        var sortedDictionary = nowMiniGameRank.OrderBy((pair) => pair.Value);
        Dictionary<byte,byte> rankTable = new Dictionary<byte,byte>();

        foreach (var rank in sortedDictionary)
            rankTable[rank.Key] = rank.Value;

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
        endText.SetActive(false);

        //順位のテキスト表示
        StartCoroutine(RankResultTextGeneration(2.0f));
    }

    //チュートリアル終わり
    public void TutorialFinish() 
    {
        //フェードが情報ないのなら
        if (this.GetComponent<CountDownAndTimer>().fade != null)
            this.GetComponent<CountDownAndTimer>().fade.FadeIn(1.0f);

        //フェードのレンダリングモードを変更
        this.GetComponent<CountDownAndTimer>().fade.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        //終わり
        TutorialManager.isTutorialFinish = true;

        //シーンを本番にチェンジ
        Invoke("SceneChange", 1.0f);
    }

    //シーン変更
    public void SceneChange() { SceneManager.LoadScene(miniGameName); }

    //ランク発表のテキスト生成
    IEnumerator RankResultTextGeneration(float delay)
    {
        yield return new WaitForSeconds(delay);

        nowRankAnnouncement = true;

        var sortedDictionary = nowMiniGameRank.OrderBy((pair) => pair.Value);
        Dictionary<byte, byte> rankTable = new Dictionary<byte, byte>();

        foreach (var rank in sortedDictionary)
            rankTable[rank.Key] = rank.Value;

        //順位テキスト表示
        for (byte i = 0; i < rankTable.Count; i++)
        {
            rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = rankTable.Values.ElementAt(i).ToString() + "位 "
            + ScoreManager.GetRankScore(rankTable.Keys.ElementAt(i), rankTable.Values.ElementAt(i)) + "P";
        }
        Instantiate(rankText, new Vector3(0, 0, 0), Quaternion.identity);

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
