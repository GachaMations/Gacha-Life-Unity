using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BodyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/Code/BodyOffset.txt";
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
