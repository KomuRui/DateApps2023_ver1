using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameWinPlayerInfo : MonoBehaviour
{

    //�J����
    public Vector3 winThreeCameraPos;
    public Vector3 winThreeCameraRotate;
    public Vector3 winOneCameraPos;
    public Vector3 winOneCameraRotate;

    //�v���C���[�̈ʒu�Ȃ�
    public Vector3 winOnePlayerPos;
    public Vector3 winOnePlayerRotate;
    public Vector3 winOnePlayerScale;
    public List<Vector3> winThreePlayerPos = new List<Vector3>();
    public List<Vector3> winThreePlayerRotate = new List<Vector3>();
    public List<Vector3> winThreePlayerScale = new List<Vector3>();

    //��p�̃t�F�[�h
    public Fade fade;

    //�����v���C���[�p�̃L�����o�X
    public GameObject winPlayerCanves;

    //�G�t�F�N�g
    public List<GameObject> effeOne;
    public List<GameObject> effeThree;

    //���������I�u�W�F�N�g�ۑ��p
    private GameObject winOneObj;
    private List<GameObject> winThreeObj = new List<GameObject>();

    //�ǂ�������������
    private bool isWinOne;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�t�F�[�h�C��
    public void FadeIn(bool isWinOnePlayer)
    {
        fade.FadeIn(2.0f);
        StartCoroutine(FadeOut(2.0f));
        StartCoroutine(WinPlayerDirecting(2.0f, isWinOnePlayer));

        //�s�v��UI���폜
        foreach (var obj in GameManager.nowMiniGameManager.killCanvas)
            obj.SetActive(false);
    }

    //�t�F�[�h�A�E�g
    IEnumerator FadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.nowMiniGameManager.endText.SetActive(false);
        fade.FadeOut(2.0f);
        StartCoroutine(WinPlayerCanvasGeneration(2.0f));

        if (GameManager.isSubMode)
            StartCoroutine(SubModeLoad(6.0f));
        else
            StartCoroutine(RankResult(6.0f));
    }

    //�����N���\�ڍs
    IEnumerator RankResult(float delay)
    {
        yield return new WaitForSeconds(delay);
        fade.FadeIn(2.0f);
        StartCoroutine(RankResultPlayerGeneration(2.0f));
        StartCoroutine(WinPlayerDelete(2.0f));

    }

    //���ʔ��\�̃v���C���[����
    IEnumerator RankResultPlayerGeneration(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.nowMiniGameManager.ChangeRankAnnouncement();
        fade.FadeOut(2.0f);
    }

    //����UI����
    IEnumerator WinPlayerCanvasGeneration(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.nowMiniGameManager.endText = Instantiate(winPlayerCanves, new Vector3(0, 0, 0), Quaternion.identity);

        //1�l�����������̂Ȃ�
        if (isWinOne)
        {
            for(int i = 0; i < effeOne.Count; i++)
            {
                effeOne[i].SetActive(true);
                effeOne[i].GetComponent<ParticleSystem>().Play();
            }

            winOneObj.GetComponent<MiniGameWinPlayer>().WinAnimation();
        }
        else
        {
            for (int i = 0; i < effeThree.Count; i++)
            {
                effeThree[i].SetActive(true);
                effeThree[i].GetComponent<ParticleSystem>().Play();
            }

            for (int i = 0; i < winThreeObj.Count; i++)
                winThreeObj[i].GetComponent<MiniGameWinPlayer>().WinAnimation();
        }
    }

    //�����v���C���[�̉��o
    IEnumerator WinPlayerDirecting(float delay,bool isWinOnePlayer)
    {
        yield return new WaitForSeconds(delay);
        MiniGameManager g = GameManager.nowMiniGameManager;
        isWinOne = isWinOnePlayer;

        //1�l�����������̂Ȃ�
        if (isWinOnePlayer)
        {
            //����
            GameObject obj = (GameObject)Resources.Load("Prefabs/" + PlayerManager.GetPlayerVisual(g.onePlayer));
            obj = Instantiate(obj, winOnePlayerPos, Quaternion.identity);
            obj.transform.position = winOnePlayerPos;
            obj.transform.localScale = winOnePlayerScale;
            obj.transform.localEulerAngles = winOnePlayerRotate;
            winOneObj = obj;

            //�J�����̂������ύX
            Camera.main.transform.position = winOneCameraPos;
            Camera.main.transform.localEulerAngles = winOneCameraRotate;
        }
        else
        {
            //�V���Ƀv���C���[����
            int i = 0;
            foreach (var rank in g.threePlayer)
            {
                GameObject obj = (GameObject)Resources.Load("Prefabs/" + PlayerManager.GetPlayerVisual(rank.Key));
                obj = Instantiate(obj, winThreePlayerPos[i], Quaternion.identity);
                obj.transform.position = winThreePlayerPos[i];
                obj.transform.localScale = winThreePlayerScale[i];
                obj.transform.localEulerAngles = winThreePlayerRotate[i];
                winThreeObj.Add(obj);
                i++;
            }

            //�J�����̂������ύX
            Camera.main.transform.position = winThreeCameraPos;
            Camera.main.transform.localEulerAngles = winThreeCameraRotate;
        }

        //�v���C���[�폜
        g.PlayerAllDelete();

        //finish�̕������폜
        GameManager.nowMiniGameManager.endText.SetActive(false);
    }


    //�����v���C���[�폜
    IEnumerator WinPlayerDelete(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isWinOne)
        {
            Destroy(winOneObj.gameObject);
        }
        else
        {
            for(int i = 0; i < winThreeObj.Count; i++)
            {
                Destroy(winThreeObj[i].gameObject);
            }
        }
    }

    IEnumerator SubModeLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("SubMode");
    }
}
