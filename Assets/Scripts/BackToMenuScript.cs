using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/*
 * This class handles the input and return to the main menu
 * Author: Steven Ho
 * Date: 23-4-2021
 * Code version: 1.0
 */
public class BackToMenuScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("IntroMenu", LoadSceneMode.Single);
        }
    }
}
