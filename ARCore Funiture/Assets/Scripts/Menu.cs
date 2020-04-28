using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Single()
    {
        SceneManager.LoadScene("ObjectManipulation");
    }
    public void Multi()
    {
        SceneManager.LoadScene("CloudAnchors");
    }
}
