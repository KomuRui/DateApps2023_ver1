using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubModeSelectManager : MonoBehaviour
{
    //���͏��
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

    [SerializeField] private List<SubModeImageInfo> image;                       //�摜
    [SerializeField] private List<SubModeImageInfo> playerInitializSelectImafe;  //�v���C���[�̏����I���摜   
    [SerializeField] public List<Color> playerColor;                             //�v���C���[�̐F
    [SerializeField] public List<Image> playerSelectImageBig;                    //�v���C���[���I�����Ă���摜��傫���\��������   
    [SerializeField] public List<Vector3> imageBigAnimationPos;                  //�r�b�O�摜�̃A�j���[�V������̈ʒu   
    [SerializeField] public List<Image> playerImage;                             //�v���C���[�摜  
    [SerializeField] private TextMeshProUGUI text;                               //��������̕���
    [SerializeField] private TextMeshProUGUI whoText;                            //�N�̃e�L�X�g
    [SerializeField] private GameObject bigSelectEdge;                           //�傫���I���摜�̘g
    [SerializeField] private Fade fade;                                          //�t�F�[�h�p           

    //���ۊǂ��Ă���Ƃ���
    private Dictionary<byte, SubModeImageInfo> playerSelectImage = new Dictionary<byte, SubModeImageInfo>();
    private Dictionary<byte, InputInfo> inputXY = new Dictionary<byte, InputInfo>();
    private Dictionary<byte, bool> isPlayerSelect = new Dictionary<byte, bool>();

    //�l�ς��ϐ�
    private bool isAllPlayerOk;       //�S�v���C���[��OK������
    private int lookNum;              //���݌��Ă���ԍ�
    private int nowRouletteCount;     //���݂̃��[���b�g��
    private int finishRouletteCount;  //�I�����鎞�̉�

    //���̉摜�Ɉڂ鎞��
    [SerializeField] private float nextImageTime;

    void Start()
    {
        //������
        for (byte i = 0; i < PlayerManager.PLAYER_MAX; i++)
        {
            //���͒l��������
            InputInfo input = new InputInfo();
            input.nowInputX = 0;
            input.nowInputY = 0;
            input.beforeInputX = 0;
            input.beforeInputY = 0;
            inputXY[(byte)(i + 1)] = input;

            //�v���C���[�̏����I���摜��������
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

        //�t�F�[�h����񂠂�̂Ȃ�
        if (fade)
            fade.FadeOut(1.0f);


        /////////////////////////////////////Test////////////////////////////////////////////

        isPlayerSelect[2] = true;
        isPlayerSelect[3] = true;
        isPlayerSelect[4] = true;

        //OK�摜��\��
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
            //����̓��͒l���擾
            inputXY[i].nowInputX = Input.GetAxis("L_Stick_H" + i);
            inputXY[i].nowInputY = Input.GetAxis("L_Stick_V" + i);

            SelectImageChange(i);  //�I���摜�ύX
            SelectImageDecide(i);  //�I���摜����
            SelectImageUnlock(i);  //�I���摜����


            //����̓��͒l��ۑ�
            inputXY[i].beforeInputX = inputXY[i].nowInputX;
            inputXY[i].beforeInputY = inputXY[i].nowInputY;
        }

    }

    //�I���摜�ύX
    private void SelectImageChange(byte playerNum)
    {
        //�I�𒆂Ȃ炱�̐揈�����Ȃ�
        if (isPlayerSelect[playerNum]) return;

        //�ύX���ł������`�F�b�N
        SubModeImageInfo info = playerSelectImage[playerNum].SelectImageChange(playerNum, inputXY);
        if (info)
        {
            //�ύX�ł����̂Ȃ�e�摜�̏���ύX
            playerSelectImage[playerNum].playerSelectMyNum.Remove(playerNum);
            info.playerSelectMyNum.Add(playerNum);
            playerSelectImage[playerNum].ImageColorChange();
            info.ImageColorChange();
            playerSelectImage[playerNum] = info;

            //�傫���摜���ύX
            playerSelectImageBig[playerNum - 1].sprite = playerSelectImage[playerNum].GetComponent<Image>().sprite;
        }
    }

    //�I���摜����
    private void SelectImageDecide(byte playerNum)
    {
        //����
        if (Input.GetButtonDown("Abutton" + playerNum) && !isPlayerSelect[playerNum])
        {
            isPlayerSelect[playerNum] = true;

            //OK�摜��\��
            playerSelectImageBig[playerNum - 1].transform.GetChild((int)BigSelectImageChild.OK_IMAGE).gameObject.SetActive(true);
            playerSelectImageBig[playerNum - 1].transform.GetChild((int)BigSelectImageChild.OK_TEXT).gameObject.SetActive(true);

            //�S��OK������
            if (isAllPlayerOK() && !isAllPlayerOk)
            {
                isAllPlayerOk = true;

                //�e���A�j���[�V��������
                ImageAnimation();
            }
        }

    }

    //�I���摜����
    private void SelectImageUnlock(byte playerNum)
    {
        //����
        if (Input.GetButtonDown("Bbutton" + playerNum) && isPlayerSelect[playerNum] && !isAllPlayerOk)
        {
            isPlayerSelect[playerNum] = false;

            //OK�摜���\��
            playerSelectImageBig[playerNum - 1].transform.GetChild((int)BigSelectImageChild.OK_IMAGE).gameObject.SetActive(false);
            playerSelectImageBig[playerNum - 1].transform.GetChild((int)BigSelectImageChild.OK_TEXT).gameObject.SetActive(false);
        }
    }

    //�S�������ł������m�F
    private bool isAllPlayerOK()
    {
        for (byte i = 1; i < PlayerManager.PLAYER_MAX; i++)
        {
            //�I�����Ă��Ȃ����A�j���[�V�����̍Œ��Ȃ�false��Ԃ�
            if (!isPlayerSelect[i]) return false;
        }

        return true;
    }

    //�摜�A�j���[�V����
    private void ImageAnimation()
    {
        foreach (var i in image)
            i.ImageMoveScreenOut();

        for (int i = 0; i < playerSelectImageBig.Count; i++)
        {
            //�v���C���[���I�������傫���摜
            playerSelectImageBig[i].transform.GetChild((int)BigSelectImageChild.FIRST_EDGE).gameObject.SetActive(false);
            playerSelectImageBig[i].transform.GetChild((int)BigSelectImageChild.PLAYER_NUMBER_TEXT).gameObject.SetActive(false);
            playerSelectImageBig[i].transform.DOScale(playerSelectImageBig[i].transform.localScale * 1.0f, 1.0f);
            playerSelectImageBig[i].transform.DOLocalMove(imageBigAnimationPos[i], 1.0f);

            //�v���C���[�摜
            playerImage[i].transform.DOLocalMoveY(-435, 1.0f);
        }

        //�e�L�X�g
        text.gameObject.SetActive(false);
        whoText.transform.DOLocalMoveY(300, 1.0f);

        //1�b���OK�摜���\����
        Invoke("OKImageNotActive", 1.0f);
    }

    //OK�摜���A�N�e�B�u��
    private void OKImageNotActive()
    {
        for (int i = 0; i < playerSelectImageBig.Count; i++)
        {
            //OK�摜���\��
            playerSelectImageBig[i].transform.GetChild((int)BigSelectImageChild.OK_TEXT).gameObject.SetActive(false);
        }

        //0.5�b��Ƀ��[���b�g�J�n
        StartCoroutine(RandomRouletteStart(0.5f));
    }

    //�����_�����[���b�g�J�n
    IEnumerator RandomRouletteStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�g��\��
        playerSelectImageBig[0].transform.GetChild((int)BigSelectImageChild.OK_IMAGE).gameObject.SetActive(false);
        bigSelectEdge.SetActive(true);
        bigSelectEdge.transform.localPosition = playerSelectImageBig[0].transform.localPosition;

        //0.5�b��Ƀ��[���b�g
        StartCoroutine(Roulette(nextImageTime));
    }

    //�����_�����[���b�g�J�n
    IEnumerator Roulette(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�����摜��\��
        playerSelectImageBig[lookNum].transform.GetChild((int)BigSelectImageChild.OK_IMAGE).gameObject.SetActive(true);

        //���Ă���ꏊ��ύX
        lookNum += 1;
        if (lookNum >= playerSelectImageBig.Count) lookNum = 0;

        //�g�ʒu�ύX,�����摜��\��
        bigSelectEdge.transform.localPosition = imageBigAnimationPos[lookNum];
        playerSelectImageBig[lookNum].transform.GetChild((int)BigSelectImageChild.OK_IMAGE).gameObject.SetActive(false);

        //�������[���b�g�񐔂�0�Ȃ�
        if (nowRouletteCount == 0)
        {
            nowRouletteCount = Random.Range(6, 12);
            nextImageTime += Random.Range(0.15f, 0.25f);

            //���x�������Ă�����I���̉񐔂����߂�
            if (nextImageTime >= 0.65f)
                finishRouletteCount = Random.Range(3, nowRouletteCount - 5);
        }
        else
            nowRouletteCount--;

        //���[���b�g�I���Ȃ�
        if (finishRouletteCount != 0 && finishRouletteCount == nowRouletteCount)
        {
            StartCoroutine(fadeIn(0.5f));
        }
        else
            StartCoroutine(Roulette(nextImageTime));
    }

    //�t�F�[�h
    IEnumerator fadeIn(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�t�F�[�h����񂠂�̂Ȃ�
        if (fade) fade.FadeIn(1.0f);

        //�V�[���ύX
        StartCoroutine(SceneChange(1.0f));
    }

    //�V�[���ύX
    IEnumerator SceneChange(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(playerSelectImage[(byte)(lookNum + 1)].miniGameName);
    }
}
