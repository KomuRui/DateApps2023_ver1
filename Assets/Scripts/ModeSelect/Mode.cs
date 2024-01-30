using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode : MonoBehaviour
{
    [SerializeField] private Vector3 camMovePos;
    private Vector3 initialPos;
    private Vector3 initialRotate;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = Camera.main.transform.position;
        initialRotate = Camera.main.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTalk()
    {
        Camera.main.transform.DOMove(camMovePos, 1.0f).SetEase(Ease.OutCubic);
        Camera.main.transform.DORotate(Vector3.zero, 1.0f).SetEase(Ease.OutCubic);
    }

    public void FinishTalk()
    {
        Camera.main.transform.DOMove(initialPos, 1.0f).SetEase(Ease.OutCubic);
        Camera.main.transform.DORotate(initialRotate, 1.0f).SetEase(Ease.OutCubic);
    }
}
