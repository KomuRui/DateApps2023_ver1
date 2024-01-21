using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubModeSelectManager : MonoBehaviour
{
    //入力情報
    public class InputInfo
    {
        public float nowInputX;
        public float nowInputY;
        public float beforeInputX;
        public float beforeInputY;
    }

    enum BigSelectImageChild
    {
        OK_IMAGE = 0,
        OK_TEXT = 1,
        FIRST_EDGE = 2,
        PLAYER_NUMBER_TEXT = 3,
        SELECT_BIG_EDGE = 4
    }

    [SerializeField] private List<SubModeImageInfo> image;                       //画像
    [SerializeField] private List<SubModeImageInfo> playerInitializSelectImafe;  //プレイヤーの初期選択画像   
    [SerializeField] public List<Color> playerColor;                             //プレイヤーの色
    [SerializeField] public List<Image> playerSelectImageBig;                    //プレイヤーが選択している画像を大きく表示するやつ   
    [SerializeField] public List<Vector3> imageBigAnimationPos;                  //ビッグ画像のアニメーション先の位置   
    [SerializeField] public List<Image> playerImage;                             //プレイヤー画像  
    [SerializeField] private TextMeshProUGUI text;                               //操作説明の文字
    [SerializeField] private TextMeshProUGUI whoText;                            //誰のテキスト
    [SerializeField] private GameObject bigSelectEdge;                           //大きい選択画像の枠
    [SerializeField] private Fade fade;                                          //フェード用           

    //情報保管しているところ
    private Dictionary<byte, SubModeImageInfo> playerSelectImage = new Dictionary<byte, SubModeImageInfo>();
    private Dictionary<byte, InputInfo> inputXY = new Dictionary<byte, InputInfo>();
    private Dictionary<byte, bool> isPlayerSelect = new Dictionary<byte, bool>();

    //値変わる変数
    private bool isAllPlayerOk;       //全プレイヤーがOKしたか
    private int lookNum;              //現在見ている番号
    private int nowRouletteCount;     //現在のルーレット回数
    private int finishRouletteCount;  //終了する時の回数

    //次の画像に移る時間
    [SerializeField] private float nextImageTime;

    void Start()
    {
        //初期化
        for (byte i = 0; i < PlayerManager.PLAYER_MAX; i++)
        {
            //入力値を初期化
            InputInfo input = new InputInfo();
            input.nowInputX = 0;
            input.nowInputY = 0;
            input.beforeInputX = 0;
            input.beforeInputY = 0;
            inputXY[(byte)(i + 1)] = input;

            //プレイヤーの初期選択画像を初期化
            playerSelectImage[(byte)(i + 1)] = playerInitializSelectImafe[i];
            playerSelectImage[(byte)(i + 1)].playerSelectMyNum.Add((byte)(i + 1));
            playerSelectImage[(byte)(i + 1)].ImageColorChange();
            playerSelectImageBig[i].sprite = playerInitializSelectImafe[i].GetComponent<Image>().sprite;
            isPlayerSelect[(byte)(i + 1)] = false;
        }

        isAllPlayerOk = false;
        lookNum = 0;
        nowRouletteCount = Random.Range(6, 12);
        finishRouletteCount = 0;

        //フェードが情報あるのなら
        if (fade)
            fade.FadeOut(1.0f);


        /////////////////////////////////////Test////////////////////////////////////////////

        isPlayerSelect[2] = true;
        isPlayerSelect[3] = true;
        isPlayerSelect[4] = true;

        //OK画像を表示
        playerSelectImageBig[1].transform.GetChild(0).gameObject.SetActive(true);
        playerSelectImageBig[1].transform.GetChild(1).gameObject.SetActive(true);
        playerSelectImageBig[2].transform.GetChild(0).gameObject.SetActive(true);
        playerSelectImageBig[2].transform.GetChild(1).gameObject.SetActive(true);
        playerSelectImageBig[3].transform.GetChild(0).gameObject.SetActive(true);
        playerSelectImageBig[3].transform.GetChild(1).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
            //今回の入力値を取得
            inputXY[i].nowInputX = Input.GetAxis("L_Stick_H" + i);
            inputXY[i].nowInputY = Input.GetAxis("L_Stick_V" + i);

            SelectImageChange(i);  //選択画像変更
            SelectImageDecide(i);  //選択画像決定
            SelectImageUnlock(i);  //選択画像解除


            //今回の入力値を保存
            inputXY[i].beforeInputX = inputXY[i].nowInputX;
            inputXY[i].beforeInputY = inputXY[i].nowInputY;
        }

    }

    //選択画像変更
    private void SelectImageChange(byte playerNum)
    {
        //選択中ならこの先処理しない
        if (isPlayerSelect[playerNum]) return;

        //変更ができたかチェック
        SubModeImageInfo info = playerSelectImage[playerNum].SelectImageChange(playerNum, inputXY);
        if (info)
        {
            //変更できたのなら各画像の情報を変更
            playerSelectImage[playerNum].playerSelectMyNum.Remove(playerNum);
            info.playerSelectMyNum.Add(playerNum);
            playerSelectImage[playerNum].ImageColorChange();
            info.ImageColorChange();
            playerSelectImage[playerNum] = info;

            //大きい画像も変更
            playerSelectImageBig[playerNum - 1].sprite = playerSelectImage[playerNum].GetComponent<Image>().sprite;
        }
    }

    //選択画像決定
    private void SelectImageDecide(byte playerNum)
    {
        //決定
        if (Input.GetButtonDown("Abutton" + playerNum) && !isPlayerSelect[playerNum])
        {
            isPlayerSelect[playerNum] = true;

            //OK画像を表示
            playerSelectImageBig[playerNum - 1].transform.GetChild((int)BigSelectImageChild.OK_IMAGE).gameObject.SetActive(true);
            playerSelectImageBig[playerNum - 1].transform.GetChild((int)BigSelectImageChild.OK_TEXT).gameObject.SetActive(true);

            //全員OKしたら
            if (isAllPlayerOK() && !isAllPlayerOk)
            {
                isAllPlayerOk = true;

                //各自アニメーションする
                ImageAnimation();
            }
        }

    }

    //選択画像解除
    private void SelectImageUnlock(byte playerNum)
    {
        //解除
        if (Input.GetButtonDown("Bbutton" + playerNum) && isPlayerSelect[playerNum] && !isAllPlayerOk)
        {
            isPlayerSelect[playerNum] = false;

            //OK画像を非表示
            playerSelectImageBig[playerNum - 1].transform.GetChild((int)BigSelectImageChild.OK_IMAGE).gameObject.SetActive(false);
            playerSelectImageBig[playerNum - 1].transform.GetChild((int)BigSelectImageChild.OK_TEXT).gameObject.SetActive(false);
        }
    }

    //全員準備できたか確認
    private bool isAllPlayerOK()
    {
        for (byte i = 1; i < PlayerManager.PLAYER_MAX; i++)
        {
            //選択していないかアニメーションの最中ならfalseを返す
            if (!isPlayerSelect[i]) return false;
        }

        return true;
    }

    //画像アニメーション
    private void ImageAnimation()
    {
        foreach (var i in image)
            i.ImageMoveScreenOut();

        for (int i = 0; i < playerSelectImageBig.Count; i++)
        {
            //プレイヤーが選択した大きい画像
            playerSelectImageBig[i].transform.GetChild((int)BigSelectImageChild.FIRST_EDGE).gameObject.SetActive(false);
            playerSelectImageBig[i].transform.GetChild((int)BigSelectImageChild.PLAYER_NUMBER_TEXT).gameObject.SetActive(false);
            playerSelectImageBig[i].transform.DOScale(playerSelectImageBig[i].transform.localScale * 1.0f, 1.0f);
            playerSelectImageBig[i].transform.DOLocalMove(imageBigAnimationPos[i], 1.0f);

            //プレイヤー画像
            playerImage[i].transform.DOLocalMoveY(-435, 1.0f);
        }

        //テキスト
        text.gameObject.SetActive(false);
        whoText.transform.DOLocalMoveY(300, 1.0f);

        //1秒後にOK画像を非表示に
        Invoke("OKImageNotActive", 1.0f);
    }

    //OK画像を非アクティブに
    private void OKImageNotActive()
    {
        for (int i = 0; i < playerSelectImageBig.Count; i++)
        {
            //OK画像を非表示
            playerSelectImageBig[i].transform.GetChild((int)BigSelectImageChild.OK_TEXT).gameObject.SetActive(false);
        }

        //0.5秒後にルーレット開始
        StartCoroutine(RandomRouletteStart(0.5f));
    }

    //ランダムルーレット開始
    IEnumerator RandomRouletteStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //枠を表示
        playerSelectImageBig[0].transform.GetChild((int)BigSelectImageChild.OK_IMAGE).gameObject.SetActive(false);
        bigSelectEdge.SetActive(true);
        bigSelectEdge.transform.localPosition = playerSelectImageBig[0].transform.localPosition;

        //0.5秒後にルーレット
        StartCoroutine(Roulette(nextImageTime));
    }

    //ランダムルーレット開始
    IEnumerator Roulette(float delay)
    {
        yield return new WaitForSeconds(delay);

        //黒い画像を表示
        playerSelectImageBig[lookNum].transform.GetChild((int)BigSelectImageChild.OK_IMAGE).gameObject.SetActive(true);

        //見ている場所を変更
        lookNum += 1;
        if (lookNum >= playerSelectImageBig.Count) lookNum = 0;

        //枠位置変更,黒い画像非表示
        bigSelectEdge.transform.localPosition = imageBigAnimationPos[lookNum];
        playerSelectImageBig[lookNum].transform.GetChild((int)BigSelectImageChild.OK_IMAGE).gameObject.SetActive(false);

        //もしルーレット回数が0なら
        if (nowRouletteCount == 0)
        {
            nowRouletteCount = Random.Range(6, 12);
            nextImageTime += Random.Range(0.15f, 0.25f);

            //速度が落ちてきたら終わりの回数を決める
            if (nextImageTime >= 0.65f)
                finishRouletteCount = Random.Range(3, nowRouletteCount - 5);
        }
        else
            nowRouletteCount--;

        //ルーレット終了なら
        if (finishRouletteCount != 0 && finishRouletteCount == nowRouletteCount)
        {
            StartCoroutine(fadeIn(0.5f));
        }
        else
            StartCoroutine(Roulette(nextImageTime));
    }

    //フェード
    IEnumerator fadeIn(float delay)
    {
        yield return new WaitForSeconds(delay);

        //フェードが情報あるのなら
        if (fade) fade.FadeIn(1.0f);

        //シーン変更
        StartCoroutine(SceneChange(1.0f));
    }

    //シーン変更
    IEnumerator SceneChange(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(playerSelectImage[(byte)(lookNum + 1)].miniGameName);
    }
}
