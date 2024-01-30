using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mode : MonoBehaviour
{
    [SerializeField] private Vector3 camMovePos;
    [SerializeField] private Fade fade;
    [SerializeField] private string changeSceneName;
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
        Camera.main.transform.DOMove(camMovePos, 1.0f).SetEase(Ease.OutCubic).OnComplete(StartFade); 
        Camera.main.transform.DORotate(Vector3.zero, 1.0f).SetEase(Ease.OutCubic);
    }

    public void FinishTalk()
    {
        Camera.main.transform.DOMove(initialPos, 1.0f).SetEase(Ease.OutCubic).OnComplete(StartFade);
        Camera.main.transform.DORotate(initialRotate, 1.0f).SetEase(Ease.OutCubic);
    }

    private void StartFade() { fade.FadeIn(2.0f); StartCoroutine(ChangeScene(2.0f)); }

    //ÉVÅ[ÉìïœçX
    IEnumerator ChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(changeSceneName);
       
    }
}
