using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private List<Vector3> playerPosition = new List<Vector3>();
    [SerializeField] private List<Vector3> playerRotation = new List<Vector3>();
    [SerializeField] private List<Vector3> playerScale = new List<Vector3>();
    [SerializeField] private Fade fade;
    [SerializeField] private float fadeTime;
    [SerializeField] private  List<GameObject> stageImageObj;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject rankText;   
    [SerializeField] private GameObject mainModeWinPlayerCanvas;

    private bool isResultFinish;

    // Start is called before the first frame update
    void Start()
    {
        isResultFinish = false;

        PlayerManager.Initializ();

        //�v���C���[����
        PlayerInstantiate();

        //���E���h�S�ďI�����Ă���̂Ȃ�
        if (StageSelectManager.isMainModeFinish)
        {
            roundText.text = "";
            mainModeWinPlayerCanvas = Instantiate(mainModeWinPlayerCanvas, new Vector3(0, 0, 0), Quaternion.identity);

            //1�ʂ̃v���C�����擾
            List<byte> a = ScoreManager.GetNominatePlayerRank(1);
            string text = "Win\n";
            for (int i = 0; i < a.Count; i++)
                text += a[i] + "P " + ScoreManager.GetScore(a[i]) + "point\n";

            //�e�L�X�g�ύX
            mainModeWinPlayerCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;

            //2�b��ɊJ�n
            StartCoroutine(ResultFinish(2.0f));

            return;
        }

        fade.FadeOut(fadeTime);
        roundText.text = StageSelectManager.GetNowRound() + "/4";

        //���ʃe�L�X�g�\��
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
        {
            rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text
                = ScoreManager.GetRank((byte)(i + 1)).ToString() + "�� " + ScoreManager.GetScore((byte)(i + 1)).ToString() + "P";
        }
        Instantiate(rankText, new Vector3(0, 0, 0), Quaternion.identity);

        //4�b��ɊJ�n
        StartCoroutine(StartSceneChange(4.0f));
    }

    // Update is called once per frame
    void Update()
    {
        //���ʔ��\���I������̂Ȃ�
        if (isResultFinish) 
            GoModeSelect();
    }

    //�v���C���[����
    private void PlayerInstantiate()
    {
        //�v���C���[����
        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
           GameObject player = ((GameObject)Resources.Load("Prefabs/" + PlayerManager.GetPlayerVisual(i)));
           player = Instantiate(player, this.transform.position, Quaternion.identity);
           player.transform.position = playerPosition[i - 1];
           player.transform.localScale = playerScale[i - 1];
           player.transform.localEulerAngles = playerRotation[i - 1];
        }
    }

    private void GoModeSelect()
    {
        if (Input.GetButtonDown("Abutton1")) 
            SceneManager.LoadScene("ModeSelect");
    }

    //���ʔ��\�I��
    IEnumerator ResultFinish(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�I���ɕύX
        isResultFinish = true;
    }

    //�V�[���J�ڊJ�n
    IEnumerator StartSceneChange(float delay)
    {
        yield return new WaitForSeconds(delay);

        StageSelectManager.ChangeMiniGameScene();
    }
}
