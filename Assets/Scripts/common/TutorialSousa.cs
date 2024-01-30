using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSousa : MonoBehaviour
{
    [SerializeField] private GameObject winKeyTag;
    [SerializeField] private GameObject sousaTag;
    [SerializeField] private GameObject winImage;
    [SerializeField] private GameObject sousaImage;

    private bool isWinKeyPrint = true;
    private float beforeInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //チュートリアルが終わっているならこの先処理しない
        if (TutorialManager.isTutorialFinish) return;

        float trigger = Input.GetAxis("L_R_Trigger1");
        Debug.Log(trigger);
        if(trigger <= -0.7f && !isWinKeyPrint && beforeInput > -0.7f)
        {
            sousaTag.GetComponent<Image>().color = winKeyTag.GetComponent<Image>().color;
            winKeyTag.GetComponent<Image>().color = Color.white;
            winImage.SetActive(true);
            sousaImage.SetActive(false);
            isWinKeyPrint = true;
        }
        else if(trigger >= 0.7f && isWinKeyPrint && beforeInput < 0.7f)
        {
            winKeyTag.GetComponent<Image>().color = sousaTag.GetComponent<Image>().color;
            sousaTag.GetComponent<Image>().color = Color.white;
            sousaImage.SetActive(true);
            winImage.SetActive(false);
            isWinKeyPrint = false;
        }
        beforeInput = trigger;
    }
}
