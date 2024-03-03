using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TOSScreen : MonoBehaviour
{
    IEnumerator Menu()
    {
        //GameObject.Find("FadeBG").GetComponent<Renderer>().enabled = true;
      float alpha = GameObject.Find("FadeBG").GetComponent<SpriteRenderer>().color.a;
      for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.25f)
      {
          Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha,1,t));
          GameObject.Find("FadeBG").GetComponent<SpriteRenderer>().color = newColor;
          yield return null;
      }
      yield return new WaitForSeconds(0.5f);
      SceneManager.LoadScene("MainMenu");
    }
    public void Confirm()
    {
        GameObject.Find("FadeBG").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("click_shorter"));
        StartCoroutine(Menu());
    }
    public void Agree() {
        GetComponent<Button>().interactable = true;
        GameObject.Find("FadeBG").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("click_shorter"));
        GameObject.Find("ButtonsVisual").GetComponent<Image>().sprite = Resources.Load<Sprite>("TOSAgree");
    }
    public void Disagree() {
        GetComponent<Button>().interactable = false;
        GameObject.Find("FadeBG").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("click_shorter"));
        GameObject.Find("ButtonsVisual").GetComponent<Image>().sprite = Resources.Load<Sprite>("TOSDisagree");
    }
}
