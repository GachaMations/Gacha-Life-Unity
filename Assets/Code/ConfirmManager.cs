using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmManager : MonoBehaviour
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
    public Sprite AgreeMenu;
    public Button ButtonObj;

    void Update()
    {
        if(m_SpriteRenderer.sprite == AgreeMenu)
        {
            ButtonObj.interactable = true;
        }
        else
        {
            StartCoroutine(WaitFrame());
            ButtonObj.interactable = false;
        }
    }
}
