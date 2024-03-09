using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneFunctions : MonoBehaviour
{
    IEnumerator FadeIn() {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        float alpha = transform.GetComponent<SpriteRenderer>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.15f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha,0,t));
            transform.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
    }
    IEnumerator Splash() {
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        float alpha = transform.GetComponent<SpriteRenderer>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.25f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha,1,t));
            transform.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("StartScreen");
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!GameObject.Find("File")) {
            GameObject newFile = new GameObject("File");
            newFile.AddComponent<SaveFile>();
            DontDestroyOnLoad(newFile);
        }
        if (!GameObject.Find("FadeBG") || GameObject.Find("FadeBG") == gameObject) DontDestroyOnLoad(gameObject); else Destroy(gameObject);
        switch (scene.name) {
            case "StartScreen":
                StartCoroutine(FadeIn());
                break;
            case "SplashScreen":
                StartCoroutine(Splash());
                break;
            case "MainMenu":
                GameObject.Find("File").GetComponent<SaveFile>().Save("TestFile.json", "TestKey", "TestValue");
                StartCoroutine(FadeIn());
                break;
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Update()
    {
        switch (SceneManager.GetActiveScene().name) {
            case "StartScreen":
                if (Input.GetMouseButtonDown(0)) {
                    if(!GameObject.Find("Canvas").GetComponent<Canvas>().enabled)
                    {
                        GetComponent<AudioSource>().Play();
                        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
                    }
                }
                //GameObject.Find("Circle").transform.Rotate(0,0,-0.05f);               // WHY IS THE GAME'S IMAGE OFFSET??? WHAT???
                break;
        }
    }

}