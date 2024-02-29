using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInMenu : MonoBehaviour
{
  IEnumerator StartingScript()
  {
    transform.GetComponent<Renderer>().enabled = true;
    transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
    yield return new WaitForSeconds(4);
    float alpha = transform.GetComponent<Renderer>().material.color.a;
    for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.1f)
    {
        Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,0,t));
        transform.GetComponent<Renderer>().material.color = newColor;
        yield return null;
    }
  }
    void Start() { StartCoroutine(StartingScript()); }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if(!GameObject.Find("Canvas").GetComponent<Canvas>().enabled)
            {
                GetComponent<AudioSource>().Play();
                //GetComponent<SpriteRenderer>().sprite = MenuTerms;
            }
            
        }
    }

}