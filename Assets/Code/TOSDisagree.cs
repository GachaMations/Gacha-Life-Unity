using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOSDisagree : MonoBehaviour
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
    public Sprite DisagreeMenu;
    public void change(Sprite DisagreeMenu)
    {
        audioData = GetComponent<AudioSource>();
        spriteRenderer.sprite = DisagreeMenu;
        audioData.Play(0);
    }
}
