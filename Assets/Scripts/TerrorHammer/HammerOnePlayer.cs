using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HammerOnePlayer : MonoBehaviour
{
    [SerializeField] private GameObject HammerOb;  // ÉnÉìÉ}Å[
    private Vector3 initializeRotate;
    private Vector3 AttackRotate;

    // Start is called before the first frame update
    void Start()
    {
        //èâä˙
        initializeRotate = new Vector3(90, 0, 0);
        AttackRotate = new Vector3(0, 0, 0);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum))
        {

            //HammerOb.transform.DORotate(initializeRotate, 0.1f);
            HammerOb.transform.DORotate(AttackRotate, 0.5f);
        }
    }
}
