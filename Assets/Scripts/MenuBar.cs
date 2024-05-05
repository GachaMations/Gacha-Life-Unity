using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBar : MonoBehaviour
{
    public void Click(string toScene) {
        SceneManager.LoadScene(toScene);
    }
}
