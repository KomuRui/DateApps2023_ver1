using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HammerOnePlayer : MonoBehaviour
{
    [SerializeField] private GameObject HammerOb;  // ÉnÉìÉ}Å[
    private Vector3 initializeRotate;
    private Vector3 AttackRotate;
    private bool isAttack;

    // Start is called before the first frame update
    void Start()
    {
        //èâä˙
        initializeRotate = new Vector3(90, 0, 0);
        AttackRotate = new Vector3(0, 0, 0);
        isAttack = true;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum) && isAttack)
        {
            isAttack = false;
            //HammerOb.transform.DORotate(initializeRotate, 0.1f);
            //HammerOb.transform.DORotate(AttackRotate, 0.5f);
            HammerOb.transform.DORotate(AttackRotate, 0.5f).SetEase(Ease.InBack);

            //1.5ïbå„Ç…Ç†Ç∞ÇÈ
            Invoke("HammerUp", 1.5f);
            Invoke("HammerAttack", 2.5f);

        }
    }


    public void HammerUp()
    {
        HammerOb.transform.DORotate(initializeRotate, 1.0f).SetEase(Ease.InQuad);
    }

    public void HammerAttack()
    {
        isAttack = true;
    }
}
