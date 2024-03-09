using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
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
            //byte[] data = Encoding.UTF8.GetBytes(ZipData);
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
            resp.Close();
        }
    }

    public void StartServer()
    {
        data = File.ReadAllBytes(Application.dataPath+"/resources/TestZip.zip");
        var Port = int.Parse(gameObject.GetComponent<InputField>().text);
        listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:"+Port+"/");
        listener.Start();
        Task t = new Task(Handle);
        t.Start();

        //listener.Close();
    }
}
