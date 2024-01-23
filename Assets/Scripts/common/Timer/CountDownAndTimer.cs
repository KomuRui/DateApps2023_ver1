using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CountDownAndTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;               //���Ԑ����e�L�X�g
    [SerializeField] private Image timeImage;                        //���Ԑ����e�L�X�g
    [SerializeField] private StartAnimation startText;               //�J�E���g�_�E���e�L�X�g
    [SerializeField] public Fade fade;  //�t�F�[�h
    [SerializeField] public Sprite timeImageRed;
    [SerializeField] public Sprite timeImageAlpha;

    private Tweener bounceTweenText;
    private Tweener bounceTweenImage;
    private Vector3 originalScale;
    private int beforeTime = 0;
    [SerializeField] public float time = 30.0f;
    public bool isfinish = false;

    // Start is called before the first frame update
    void Start()
    {
        if(timeText)
            originalScale = timeText.transform.localScale;

        //�`���[�g���A���Ȃ�
        if(!TutorialManager.isTutorialFinish)
        {
            GameManager.nowMiniGameManager.SetMiniGameStart();
        }
        else
            StartCoroutine(startText.StartBackGroundAnimation(1.0f));

    }

    // Update is called once per frame
    void Update()
    {
        //���Ԍv�Z
        TimeCalc();
    }

    //���Ԍv�Z
    private void TimeCalc()
    {
        if (!GameManager.nowMiniGameManager.IsStart() || isfinish || GameManager.nowMiniGameManager.IsFinish() || timeText == null) return;
        time -= Time.deltaTime;
        time = Mathf.Max(time, 0);
        timeText.text = ((int)time).ToString();
        if (time <= 1)
        {
            isfinish = true;
            timeText.text = "0";
            GameManager.nowMiniGameManager.SetMiniGameFinish();
        }
        if(((int)time) <= 5)
        {
            //�ԐF�ɕϊ�
            timeImage.sprite = timeImageRed;

            //�O��Ǝ��Ԃ��Ⴄ�̂Ȃ�
            if (beforeTime != ((int)time))
            {
                BounceAnimation();
                beforeTime = ((int)time);
            }

        }
    }

    //���Ƃ�
    IEnumerator CountDownText(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    //�J�E���g�_�E���ƃ^�C�}�[��ݒ肷��
    public void SetCountDownAndTimer()
    {
        //�����`���[�g���A�����I����Ă���̂Ȃ�
        if (TutorialManager.isTutorialFinish && fade != null)
        {
            fade.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            if (timeText != null) timeText.gameObject.SetActive(true);
            if (timeImage != null) timeImage.gameObject.SetActive(true);
        }

        //�t�F�[�h�����Ȃ��̂Ȃ�
        if (fade != null)
            fade.FadeOut(1.0f);

        StartCoroutine(CountDownText(1.0f));
    }

    private void BounceAnimation()
    {
        //�O��̃A�j���[�V�����폜
        bounceTweenText.Kill();
        bounceTweenImage.Kill();

        //�e�L�X�g�̃A�j���[�V����
        timeText.transform.localScale = transform.localScale + new Vector3(3,3,3);
        bounceTweenText = timeText.transform.DOScale(originalScale, 1)
            .SetEase(Ease.OutBounce);

        //�摜�̃A�j���[�V����
        GameObject newImageObject = Instantiate(timeImage.gameObject, timeImage.transform.position, timeImage.transform.rotation, timeImage.transform.parent);
        newImageObject.transform.localScale = timeImage.transform.localScale;
        newImageObject.GetComponent<Image>().sprite = timeImageAlpha;

        //�g��
        Vector3 afterScale = transform.localScale + new Vector3(3.5f, 3.5f, 3.5f);
        bounceTweenImage = newImageObject.transform.DOScale(afterScale, 0.8f)
            .SetEase(Ease.OutQuad);

        //������
        newImageObject.GetComponent<Image>().DOFade(0f, 0.8f)
            .SetEase(Ease.OutQuad).OnComplete(() => Destroy(newImageObject.gameObject));
    }


}
