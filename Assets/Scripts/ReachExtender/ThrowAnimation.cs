using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAnimation : MonoBehaviour
{
    [SerializeField] Arm arm;
    [SerializeField] ArmChild armChild;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Throw()
    {
        //transform.DOLocalRotate(new Vector3( 30, 180, 0), 0.5f).SetEase(Ease.OutQuad).OnComplete(TurnBack);
    }

    public void TurnBack()
    {
        //transform.DOLocalRotate(new Vector3(0, 180, 0), 0.5f).SetEase(Ease.OutQuad);
        //armChild.PlayerMove();
        //arm.hitPlayer = null;
    }
}
