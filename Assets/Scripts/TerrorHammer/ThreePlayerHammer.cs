using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePlayerHammer : MonoBehaviour
{

    public bool isAttack = false;
    public int playerNum;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        isAttack = true;
        this.transform.DOLocalRotate(new Vector3(0,transform.localEulerAngles.y, transform.localEulerAngles.z), 0.5f).SetEase(Ease.InBack);

    }

    public void Return()
    {
        this.transform.DOLocalRotate(new Vector3(90, transform.localEulerAngles.y, transform.localEulerAngles.z), 0.5f).SetEase(Ease.InBack).OnComplete(()=> isAttack = false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && other.transform.GetComponent<PlayerNum>().playerNum != playerNum && isAttack)
        {
            other.transform.GetComponent<TerrorHammerThreePlayer>().HitPlayerHammer();
        }
    }

}
