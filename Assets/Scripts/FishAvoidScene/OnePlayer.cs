using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class OnePlayer : MonoBehaviour
{
    public GameObject penguinP;
    public GameObject sharkP;
    public GameObject fishesP;
    public GameObject dolphinP;
    public float playerSpeed;
    public float pinguinCoolTime;
    public float dolphinCoolTime;
    public float sharkCoolTime;
    public float fishesCoolTime;
    public float AllCoolTime;
    float a = 2;
    float b = 1;


    Quaternion penguinRotate = Quaternion.Euler(0, -90, 90);
    Quaternion sharkRotate = Quaternion.Euler(0, 270, 0);
    Quaternion fishesRotate = Quaternion.Euler(0, 180, 0);
    Quaternion dolphinRotate = Quaternion.Euler(250, 180, 0);
    bool isPenguin;
    bool isShark;
    bool isFishes;
    bool isDolphin;
    bool isStop;
    bool isCoroutineStart;
    public GameObject PenguinImage;
    GameObject SharkImage;
    GameObject FishesImage;
    GameObject DolphinImage;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(penguinP, this.transform.position, penguinRotate);
        isPenguin = true;
        isShark = true;
        isFishes = true;
        isDolphin = true;
        isStop = false;
        isCoroutineStart = false;
        //PenguinImage = GameObject.Find("PenguinCoolTimeImage");
        SharkImage = GameObject.Find("SharkCoolTimeImage");
        FishesImage = GameObject.Find("FishesCoolTimeImage");
        DolphinImage = GameObject.Find("DolphinCoolTimeImage");
    }

    // Update is called once per frame
    void Update()
    {
        b -=(1 / (60 * a));
        //PenguinImage.GetComponent<Image>().fillAmount = 1 / (360 / (60 * a));
        PenguinImage.GetComponent<Image>().fillAmount = b;
        //a -= 0.01f;

        if (isStop)
        {
            if (!isCoroutineStart) 
            {
                StartCoroutine(AllCoolCorou());
            }           
        }
        else if (Input.GetKeyDown(KeyCode.A) && isPenguin)
        {
            Instantiate(penguinP, new Vector3(this.transform.position.x, -0.53f, 10), penguinRotate);
            StartCoroutine(PenguinCoolCorou());
            isStop = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) && isShark)
        {
            Instantiate(sharkP, new Vector3(this.transform.position.x, -1, 10), sharkRotate);
            StartCoroutine(SharkCoolCorou());
            isStop = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && isFishes)
        {
            Instantiate(fishesP, new Vector3(this.transform.position.x, -0.9f, 10), fishesRotate);
            StartCoroutine(FishesCoolCorou());
            isStop = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) && isDolphin)
        {
            Instantiate(dolphinP, new Vector3(this.transform.position.x, -3, 10), dolphinRotate);
            StartCoroutine(DolphinCoolCorou());
            isStop = true;
        }

        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 3.5)
        {
            transform.position += new Vector3(playerSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -3.5)
        {
            transform.position += new Vector3(-playerSpeed, 0, 0);
        }


        
    }


    //�R���[�`���֐����`
    private IEnumerator AllCoolCorou() //�R���[�`���֐��̖��O
    {
        isCoroutineStart = true;
        yield return new WaitForSeconds(AllCoolTime);
        isStop = false;
        isCoroutineStart = false;
    }

    //�؂񂬂�
    private IEnumerator PenguinCoolCorou() //�R���[�`���֐��̖��O
    {
        isPenguin = false;
        yield return new WaitForSeconds(pinguinCoolTime);
        isPenguin = true;
    }

    //����
    private IEnumerator SharkCoolCorou() //�R���[�`���֐��̖��O
    {
        isShark = false;
        yield return new WaitForSeconds(sharkCoolTime);
        isShark = true;
    }

    //���Q
    private IEnumerator FishesCoolCorou() //�R���[�`���֐��̖��O
    {
        isFishes = false;
        yield return new WaitForSeconds(fishesCoolTime);
        isFishes = true;
    }

    //���邩
    private IEnumerator DolphinCoolCorou() //�R���[�`���֐��̖��O
    {
        isDolphin = false;
        yield return new WaitForSeconds(dolphinCoolTime);
        isDolphin = true;
    }


    ////�W�����v
    //private void Jump()
    //{
    //    //�W�����v���Ă���Ȃ炱�̐揈�����Ȃ�
    //    if (isButton) return;

    //    //�y���M������
    //    if (Input.GetButtonDown("Abutton" + playerNum) && lockA <= 0)
    //    {
    //        lockA = 60;


    //        //�ʏ��ԂɕύX
    //        ChangeStateTo(SlimeAnimationState.Idle);

    //        //��ɗ͂�������
    //        rb.AddForce(Vector3.up * jumpPower);
    //        isJump = true;
    //        return;
    //    }














    //    int beforeStage = nowStageNum;

    //    //�����W�����v(�ʂ̑����)
    //    if (Input.GetAxis("L_Stick_V" + playerNum) < -0.8f)
    //    {
    //        nowStageNum--;
    //        nowStageNum = Math.Max(nowStageNum, 0);
    //        if (beforeStage == nowStageNum) return;
    //        transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
    //        ChangeStateTo(SlimeAnimationState.Idle);
    //        rb.AddForce(Vector3.up * jumpPower);
    //        isJump = true;
    //    }
    //    else if (Input.GetAxis("L_Stick_V" + playerNum) > 0.8f)
    //    {
    //        nowStageNum++;
    //        nowStageNum = Math.Min(nowStageNum, stage.Length - 1);
    //        if (beforeStage == nowStageNum) return;
    //        transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
    //        ChangeStateTo(SlimeAnimationState.Idle);
    //        rb.AddForce(Vector3.up * jumpPower);
    //        isJump = true;
    //    }

    //    //�W�����v��ԂɕύX    
    //    //ChangeStateTo(SlimeAnimationState.Jump);
    //}
}
