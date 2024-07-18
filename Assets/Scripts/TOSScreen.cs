using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TOSScreen : MonoBehaviour
{
    GameObject Fade;
    SceneFunctions sFuncs;
    public IEnumerator Menu()
    {
        float alpha = Fade.GetComponent<SpriteRenderer>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.25f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha,1,t));
            Fade.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }
    public void Confirm()
    {
        Fade.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/click"));
        sFuncs.skipTOS = true;
        if (!sFuncs.Transfer) StartCoroutine(Menu());
        else {
            sFuncs.Transfer = false;
            GameObject.Find("Transfer").GetComponent<Canvas>().enabled = true;
            GetComponent<Canvas>().enabled = false;
        }
    }
    public void Agree() {
        GameObject.Find("Continue").GetComponent<Button>().interactable = true;
        Fade.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/click"));
        GameObject.Find("ButtonsVisual").GetComponent<Image>().sprite = Resources.Load<Sprite>("TOSAgree");
    }
    public void Disagree() {
        GameObject.Find("Continue").GetComponent<Button>().interactable = false;
        Fade.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/click"));
        GameObject.Find("ButtonsVisual").GetComponent<Image>().sprite = Resources.Load<Sprite>("TOSDisagree");
    }
    void Awake() {
        Fade = GameObject.Find("FadeBG");
        sFuncs = Fade.GetComponent<SceneFunctions>();
    }
}
