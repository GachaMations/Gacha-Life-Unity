using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgreeDisagreeManager : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;

    IEnumerator WaitFrame()
    {
        yield return 0;
    }
    void Start()
    {
        m_SpriteRenderer = GameObject.Find("Square").GetComponent<SpriteRenderer>();
    }
    public Sprite Menu;
    public Button ButtonObj;

    void Update()
    {
        if(m_SpriteRenderer.sprite == Menu)
        {
            ButtonObj.interactable = false;
        }
        else
        {
            StartCoroutine(WaitFrame());
            ButtonObj.interactable = true;
        }
    }
}
