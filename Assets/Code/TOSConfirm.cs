using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TOSConfirm : MonoBehaviour
{
    IEnumerator FadeTo(float aValue, float aTime)
    {
      float alpha = transform.GetComponent<Renderer>().material.color.a;
      for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
      {
          Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
          transform.GetComponent<Renderer>().material.color = newColor;
          yield return null;
      }
    }

    IEnumerator MainMenuThing()
    {
        StartCoroutine(FadeTo(0.0f, 0.5f));
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainMenu");
    }
    AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void MainMenu()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
        StartCoroutine(MainMenuThing());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
