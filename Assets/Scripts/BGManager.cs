using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGManager : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (string line in System.IO.File.ReadLines(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData), "GachaUnity"), "CurrentStudio"), "Current.oc")))
        {
            var ValueIn = line.Substring(0, line.IndexOf(" "));
            if (ValueIn == "BG")
            {
                var ValueS = line.Substring(4, line.IndexOf(" "));
                var Value = String.Concat(ValueS.Where(c => !Char.IsWhiteSpace(c)));
                string bgname = ValueIn + Value;
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Backgrounds/" + bgname);
            }
        }
    }
}
