using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Options : MonoBehaviour
{
    public bool toggle;

    public string toggleOnValue; //I tried to hide these in the inspector if toggle was false, but I think you need editor scripts for that.
    public string toggleOffValue;

    public bool buttonName = true;
    public UnityEvent ClickAction;
    void updateText()
    {
        if (toggle) {
            if (GameObject.Find("File").GetComponent<SaveFile>().Load("PlayerData.dat", transform.name) == "")
                GameObject.Find("File").GetComponent<SaveFile>().Save("PlayerData.dat", transform.name, "false");
            string textValue = GameObject.Find("File").GetComponent<SaveFile>().Load("PlayerData.dat", transform.name)=="false" ? toggleOffValue : toggleOnValue;
            Debug.Log(textValue);
            transform.Find("Text").GetComponent<Text>().text = transform.name+"\n"+textValue; //Add horizontal line
            if (GameObject.Find("File").GetComponent<SaveFile>().Load("PlayerData.dat", transform.name)=="false")
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("OptionOff");
            else gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("OptionOn");
        } else if (buttonName) transform.Find("Text").GetComponent<Text>().text = transform.name;
        else transform.Find("Text").GetComponent<Text>().text = "";
    }
    void Start() { updateText(); }

    public void Click() {
        if (toggle) {
            if (GameObject.Find("File").GetComponent<SaveFile>().Load("PlayerData.dat", transform.name) == "")
                GameObject.Find("File").GetComponent<SaveFile>().Save("PlayerData.dat", transform.name, "false");
            GameObject.Find("File").GetComponent<SaveFile>().Save("PlayerData.dat", transform.name,
                GameObject.Find("File").GetComponent<SaveFile>().Load("PlayerData.dat", transform.name)=="false"?"true":"false"
            );
            updateText();
        } else ClickAction.Invoke();
    }

    public void openTransfer() { GameObject.Find("Menus/Transfer").GetComponent<Canvas>().enabled = true; }
    public void closeTransfer() { GameObject.Find("Menus/Transfer").GetComponent<Canvas>().enabled = false; }
    public void openCredits() { GameObject.Find("Menus/Credits").GetComponent<Canvas>().enabled = true; }
    public void closeCredits() { GameObject.Find("Menus/Credits").GetComponent<Canvas>().enabled = false; }
    public void openMusic() { GameObject.Find("Menus/CreditsMusic").GetComponent<Canvas>().enabled = true; }
    public void closeMusic() { GameObject.Find("Menus/CreditsMusic").GetComponent<Canvas>().enabled = false; }
    public void openNpc() { GameObject.Find("Menus/CreditsMusic").GetComponent<Canvas>().enabled = true; }
    public void closeNpc() { GameObject.Find("Menus/CreditsMusic").GetComponent<Canvas>().enabled = false; }
}
