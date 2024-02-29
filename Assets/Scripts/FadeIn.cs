using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;

    IEnumerator StartingScript()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeTo(1.0f, 0.1f));
    }

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
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        float TheAlpha = transform.GetComponent<Renderer>().material.color.a;
        Color ThenewColor = new Color(1, 1, 1 , Mathf.Lerp(0f, 0f, 0f));
        transform.GetComponent<Renderer>().material.color = ThenewColor;
        StartCoroutine(StartingScript());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
