using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Initializ();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Abutton1")) SceneManager.LoadScene("ModeSelect");
    }
}
