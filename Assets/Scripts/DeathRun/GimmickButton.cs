using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GimmickButton : MonoBehaviour
{

    [SerializeField] GimmickBase targetGimmick;
    [SerializeField] GameObject buttonObj;
    private bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Abutton" + GameManager.nowMiniGameManager.onePlayer) && isHit)
        {
            targetGimmick.SetIsInput(true);

            //nullじゃないのならアニメーション
            if(buttonObj != null) buttonObj.transform.DOMoveY(transform.localPosition.x - 2.0f, 0.5f).SetEase(Ease.OutQuad).OnComplete(ChangeMaterial);
        }
    }

    //マテリアル変更
    private void ChangeMaterial()
    {
        var children2 = buttonObj.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < children2.Length; i++)
        {
            for (int j = 0; j < children2[i].materials.Length; j++)
                children2[i].materials[j].color = Color.green;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "OnePlayer")
            isHit = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "OnePlayer")
            isHit = false;
    }
}
