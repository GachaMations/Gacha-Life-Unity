using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOSAgree : MonoBehaviour
{
    AudioSource audioData;
    SpriteRenderer spriteRenderer;
    public void OnPressed()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
    }
    public Sprite AgreeMenu;
    public void change(Sprite AgreeMenu)
    {
        audioData = GetComponent<AudioSource>();
        spriteRenderer.sprite = AgreeMenu;
        audioData.Play(0);
    }
}