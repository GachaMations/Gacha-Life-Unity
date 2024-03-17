using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeBackground : MonoBehaviour
{
    int CurrentPage = 1;
    int Backgrounds = 600;
    int MaxPages = 24;
    SaveFile saves;
    GameObject funcs;
    public void switchBG() {
        var bg = (CurrentPage-1)*25+int.Parse(EventSystem.current.currentSelectedGameObject.name.Remove(0, 2));
        saves.Save("Characters\\"+saves.Load("PlayerData.dat", "CurrentCharacter")+".oc", "BG", bg.ToString());
        funcs.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("click"));
        this.transform.parent.GetComponent<Canvas>().enabled = false;
    }
    void RefreshPage() {
        foreach (Transform childTransform in this.transform)
        {
            var switchBG = (CurrentPage-1)*25+int.Parse(childTransform.name.Remove(0, 2));
            var spr = Resources.Load<Sprite>("Assets/Backgrounds/"+switchBG);
            childTransform.GetChild(0).GetComponent<Image>().sprite = spr;
        }
        this.transform.parent.Find("PageNumber").GetComponent<Text>().text = "Page\n"+CurrentPage+"/"+MaxPages;
    }
    public void Open() {
        this.transform.parent.GetComponent<Canvas>().enabled = true;
        funcs.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("click"));
    }
    public void Close() {
        this.transform.parent.GetComponent<Canvas>().enabled = false;
        funcs.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("cancel"));
    }
    public void MovePage(bool Right) {
        if (Right) {
            if (CurrentPage == MaxPages) CurrentPage = 1;
            else CurrentPage++;
        } else {
            if (CurrentPage == 1) CurrentPage = MaxPages;
            else CurrentPage--;
        }
        RefreshPage();
    }
    void Start() {
        saves = GameObject.Find("File").GetComponent<SaveFile>();
        funcs = GameObject.Find("FadeBG");
        // Backgrounds = Resources.LoadAll<Texture2D>("Assets/Backgrounds/").Length;  //Takes a while to load all images. Setting manually is more performant
        Backgrounds = 600;
        Debug.Log(Backgrounds);
        MaxPages = (int)Mathf.Ceil(Backgrounds / 25f);
        RefreshPage();
    }
}
