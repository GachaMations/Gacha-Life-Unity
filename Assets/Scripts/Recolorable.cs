using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolorable : MonoBehaviour
{
    public string ColorKey1;
    public string ColorKey2;
    public bool rawColor;
    SaveFile saves;
    void Awake() {
        saves = GameObject.Find("File").GetComponent<SaveFile>();
    }
    void Update() {
        var currentChar = saves.Load("PlayerData.dat", "CurrentCharacter");
        if (!rawColor) {
            var currentColor1 = saves.Load("Characters\\"+currentChar+".oc", ColorKey1);
            var currentColor2 = saves.Load("Characters\\"+currentChar+".oc", ColorKey2);
            Color newCol1, newCol2;
            if (ColorUtility.TryParseHtmlString("#"+currentColor1, out newCol1) && ColorUtility.TryParseHtmlString("#"+currentColor2, out newCol2)) {
                GetComponent<SpriteRenderer>().material.SetColor("_RemapColor1", newCol1);
                GetComponent<SpriteRenderer>().material.SetColor("_RemapColor2", newCol2);
                Debug.Log(newCol1);
            }
        } else {
            var currentColor = saves.Load("Characters\\"+currentChar+".oc", ColorKey1);
            Color newCol;
            if (ColorUtility.TryParseHtmlString("#"+currentColor, out newCol)) {
                GetComponent<SpriteRenderer>().material.color = newCol;
            }
        }
    }
}