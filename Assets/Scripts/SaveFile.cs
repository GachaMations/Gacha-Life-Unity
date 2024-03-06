using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class SaveFile : MonoBehaviour
{
    string SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GachaUnity\\Life";
    private bool CheckExistance(string path) {
        if (!File.Exists(path)) {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.CreateText(path);
            return false;
        }
        else return true;
    }
    public void Save(string path, string varName, string varData) {
        string purePath = SaveDirectory+"\\"+path;
        if (!File.Exists(purePath)) {
            Directory.CreateDirectory(Path.GetDirectoryName(purePath));
            using (StreamWriter file = File.CreateText(purePath))
            {
                file.WriteLine("{");
                file.WriteLine("\t\""+varName+"\": \""+varData+"\",");
                file.WriteLine("}");
            }
        }
        else
        {
            string rewritten = "";
            bool foundKey = false;
            foreach (var line in File.ReadAllLines(purePath)) {
                if (line.StartsWith("\t\""+varName+"\": \"")) {
                    rewritten+="\t\""+varName+"\": \""+varData+"\",\n";
                    foundKey = true;
                } else {
                    if (line == "}" && !foundKey) {
                        rewritten+="\t\""+varName+"\": \""+varData+"\",\n}";
                    } else rewritten+=line+"\n";
                }
            }
            File.WriteAllText(purePath, rewritten);
        }
    }
    public string Load(string path, string varName) {
        string purePath = SaveDirectory+"\\"+path;
        if (File.Exists(purePath)) return JObject.Parse(File.ReadAllText(purePath))[varName].ToString();
        else return "";
    }

    void OnEnable() { DontDestroyOnLoad(gameObject); }
}
