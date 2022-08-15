using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBG : MonoBehaviour
{
    public string SwitchTo;
    string NLine;
   public void ChangeBackground()
    {
        foreach (string line in System.IO.File.ReadLines(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData), "GachaUnity"), "CurrentStudio"), "Current.oc")))
        {
            var ValueIn = line.Substring(0, line.IndexOf(" "));
            if (ValueIn == "BG")
            {
                NLine = line;
            }
        }
        string fullfile = File.ReadAllText(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData), "GachaUnity"), "CurrentStudio"), "Current.oc"));
        string replacement = fullfile.Replace(NLine, "BG = " + SwitchTo);
        File.WriteAllText(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData), "GachaUnity"), "CurrentStudio"), "Current.oc"), replacement);
    }
}
