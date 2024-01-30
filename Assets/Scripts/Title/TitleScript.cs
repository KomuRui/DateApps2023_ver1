
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    [SerializeField] private Fade fade;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Abutton1"))
        {
            fade.FadeIn(2.0f);
            StartCoroutine(ChangeScene(2.0f));
        }
    }

    //ÉVÅ[ÉìïœçX
    IEnumerator ChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("CharaSelect");

    }
}
