using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Unity.SharpZipLib.Utils;
public class DataTransfer : MonoBehaviour
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
            Debug.Log("user");
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
            resp.Close();
        }
    }
    string SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GachaUnity";
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
        var Port = int.Parse(gameObject.GetComponent<InputField>().text);
        listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:"+Port+"/");
        listener.Start();
        Task t = new Task(Handle);
        t.Start();
        var host = Dns.GetHostEntry(Dns.GetHostName());
        var privIp = "IP NOT FOUND";
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork) privIp = ip.ToString();
        }
        GameObject.Find("Status").GetComponent<Text>().text = "IP: "+privIp+"\nPort: "+Port;
    }



}
