using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TOSScreen : MonoBehaviour
{
    IEnumerator Menu()
    {
      float alpha = GameObject.Find("Fade").transform.GetComponent<Renderer>().material.color.a;
      for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.25f)
      {
          Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,0,t));
          GameObject.Find("Fade").transform.GetComponent<Renderer>().material.color = newColor;
          yield return null;
      }
      yield return new WaitForSeconds(0.5f);
      SceneManager.LoadScene("MainMenu");
    }
    public void Confirm()
    {
        GetComponent<AudioSource>().Play(0);
        StartCoroutine(Menu());
    }
    public void Agree() {
        GetComponent<Button>().interactable = true;
        GameObject.Find("ButtonsVisual").GetComponent<Image>().sprite = Resources.Load<Sprite>("TOSAgree");
    }
    public void Disagree() {
        GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonsVisual").GetComponent<Image>().sprite = Resources.Load<Sprite>("TOSDisagree");
    }
}
