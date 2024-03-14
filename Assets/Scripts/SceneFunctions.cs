using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class SceneFunctions : MonoBehaviour
{
    SaveFile Saves;

    public bool Transfer;
    public bool skipTOS;
    private string CharSlot;

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

    IEnumerator Menu() {
        float alpha = gameObject.GetComponent<SpriteRenderer>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.25f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha,1,t));
            gameObject.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        switch (scene.name) {
            case "StartScreen":
                StartCoroutine(FadeIn());
                break;
            case "SplashScreen":
                StartCoroutine(Splash());
                break;
            case "MainMenu":
                if (Saves.Load("PlayerData.dat", "CurrentCharacter") == "") Saves.Save("PlayerData.dat", "CurrentCharacter", "1");
                StartCoroutine(FadeIn());
                break;
        }
    }
    void Update()
    {
        switch (SceneManager.GetActiveScene().name) {
            case "StartScreen":
                if (Input.GetMouseButtonDown(0)) {
                    if(!GameObject.Find("Canvas").GetComponent<Canvas>().enabled && !GameObject.Find("Transfer").GetComponent<Canvas>().enabled)
                    {
                        if (EventSystem.current.IsPointerOverGameObject()) Transfer = true;
                        GetComponent<AudioSource>().Play();
                        if (!skipTOS) GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
                        else {
                            if (Transfer) {
                                Transfer = false;
                                GameObject.Find("Transfer").GetComponent<Canvas>().enabled = true;
                                GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
                            } else StartCoroutine(Menu());
                        }
                    }
                }
                //GameObject.Find("Circle").transform.Rotate(0,0,-0.05f);               // WHY IS THE GAME'S IMAGE OFFSET??? WHAT???
                break;
            case "MainMenu":
                CharSlot = Saves.Load("PlayerData.dat", "CurrentCharacter");
                if (Saves.Load("Characters\\"+CharSlot+".oc", "BG") == "") Saves.Save("Characters\\"+CharSlot+".oc", "BG", "1");
                GameObject.Find("Background").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Backgrounds/"+Saves.Load("Characters\\"+CharSlot+".oc", "BG"));
                break;
        }
    }


    void OnEnable()
    {
        if (!GameObject.Find("File")) {
            GameObject newFile = new GameObject("File");
            newFile.AddComponent<SaveFile>();
            DontDestroyOnLoad(newFile);
        }
        if (!GameObject.Find("FadeBG") || GameObject.Find("FadeBG") == gameObject) DontDestroyOnLoad(gameObject); else Destroy(gameObject);
        Saves = GameObject.Find("File").GetComponent<SaveFile>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}