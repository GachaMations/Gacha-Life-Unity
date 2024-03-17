/*

    --WARNING-- This code is AWFUL.
            
    This script will NOT be used for the final release. This
    is a testing script to get the basic systems of Data Transfer
    working.

    The main parts of this will be used for the actual data transfer page,
    But the entire script will be remade cleaner.

    (half this code is copy and pasted from stackoverflow lmao)


*/


using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Unity.SharpZipLib.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class DataTransferSample : MonoBehaviour
{
    private static HttpListener listener;
    private static bool HostFiles = true;
    //private static string ZipData;
    private static byte[] data;
    private static async void Handle()
    {
        while (HostFiles) {
            HttpListenerContext ctx = await listener.GetContextAsync();
            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
            resp.Close();
        }
        HostFiles = true;
    }
    string SaveDirectory = "";
    public void StartServer()
    {
        GameObject.Find("Status").GetComponent<Text>().text = "Zipping Save Files";
        ZipUtility.CompressFolderToZip(SaveDirectory+"\\Life.zip",null,SaveDirectory+"\\Life");
        GameObject.Find("Status").GetComponent<Text>().text = "Reading zip's bytes";
        try {
            data = File.ReadAllBytes(SaveDirectory+"\\Life.zip");
        }
        catch (IOException) {
            GameObject.Find("Status").GetComponent<Text>().text = "File is bigger than 2gb (somehow)";
            return;
        }
        GameObject.Find("Status").GetComponent<Text>().text = "Starting Server";
        var host = Dns.GetHostEntry(Dns.GetHostName());
        var privIp = "IP NOT FOUND";
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork) privIp = ip.ToString();
        }
        var Port = int.Parse(gameObject.GetComponent<InputField>().text);
        listener = new HttpListener();
        listener.Prefixes.Add("http://*:"+Port+"/");
        listener.Prefixes.Add("http://"+privIp+":"+Port+"/");
        listener.Prefixes.Add("http://127.0.0.1:"+Port+"/");
        listener.Prefixes.Add("http://localhost:"+Port+"/");
        listener.Start();
        Task t = new Task(Handle);
        t.Start();
        GameObject.Find("Status").GetComponent<Text>().text = "IP: "+privIp+"\nPort: "+Port;
    }
    IEnumerator StartClientI() {
        UnityWebRequest www = new UnityWebRequest("http://"+GameObject.Find("IP").GetComponent<InputField>().text+":"+GameObject.Find("CLPort").GetComponent<InputField>().text);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            UnityEngine.Debug.Log(www.error);
        }
        else {
            byte[] results = www.downloadHandler.data;
            FileStream fs = new FileStream(SaveDirectory+"\\RestoredSaves.zip", FileMode.Create, FileAccess.Write);
            fs.Write(results);
            fs.Close();
            if (Directory.Exists(SaveDirectory+"\\RestoreBackup")) Directory.Delete(SaveDirectory+"\\RestoreBackup", true);
            Directory.Move(SaveDirectory+"\\Life", SaveDirectory+"\\RestoreBackup");
            ZipUtility.UncompressFromZip(SaveDirectory+"\\RestoredSaves.zip", null, SaveDirectory+"\\Life");
            File.Delete(SaveDirectory+"\\RestoredSaves.zip");
        }
    }
    public void StartClient() { StartCoroutine(StartClientI()); }

    public void StopServer() {
        listener.Stop();
        HostFiles = false;
        GameObject.Find("Status").GetComponent<Text>().text = "Server stopped.";
    }

    public void OpenClient() {
        SceneManager.LoadScene("DataTransferTest");
    }

    public void closeScreen() {
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    void Start() {
        SaveDirectory = GameObject.Find("File").GetComponent<SaveFile>().GUPath;
    }
}
