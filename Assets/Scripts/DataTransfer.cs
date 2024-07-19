using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Unity.SharpZipLib.Utils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class DataTransfer : MonoBehaviour
{
    private static HttpListener listener;
    private static bool hostFiles = true;
    private byte[] data;
    private string saveDirectory = "";

    private async void HandleRequests()
    {
        while (hostFiles)
        {
            try
            {
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerResponse response = context.Response;
                response.ContentEncoding = Encoding.UTF8;
                response.ContentLength64 = data.LongLength;
                await response.OutputStream.WriteAsync(data, 0, data.Length);
                response.Close();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error handling request: " + ex.Message);
            }
        }
    }

    private IEnumerator StartClientCoroutine(string ipAddress, int port)
    {
        string url = "http://" + ipAddress + ":" + port;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error connecting to server: " + www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            string restoredSavesPath = Path.Combine(saveDirectory, "RestoredSaves.zip");
            File.WriteAllBytes(restoredSavesPath, results);
            if (Directory.Exists(Path.Combine(saveDirectory, "RestoreBackup")))
            {
                Directory.Delete(Path.Combine(saveDirectory, "RestoreBackup"), true);
            }
            Directory.Move(Path.Combine(saveDirectory, "Life"), Path.Combine(saveDirectory, "RestoreBackup"));
            ZipUtility.UncompressFromZip(Path.Combine(saveDirectory, "RestoredSaves.zip"), null, Path.Combine(saveDirectory, "Life"));
            File.Delete(Path.Combine(saveDirectory, "RestoredSaves.zip"));
            transform.Find("Status").GetComponent<Text>().text = "Successfully downloaded Save File!";
        }
    }

    public void StartServer()
    {
        int port;
        try
        {
            port = int.Parse(GameObject.Find("Port").GetComponent<InputField>().text);
            transform.Find("Start/Text").GetComponent<Text>().text = "Zipping Save Files";
            ZipUtility.CompressFolderToZip(Path.Combine(saveDirectory, "Life.zip"), null, Path.Combine(saveDirectory, "Life"));
            transform.Find("Start/Text").GetComponent<Text>().text = "Reading zip's bytes";
            data = File.ReadAllBytes(Path.Combine(saveDirectory, "Life.zip"));
        }
        catch (IOException ex)
        {
            transform.Find("Start/Text").GetComponent<Text>().text = "Error: " + ex.Message;
            return;
        }

        transform.Find("Start/Text").GetComponent<Text>().text = "Starting Server";
        string privateIpAddress = "IP NOT FOUND";
        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                privateIpAddress = ip.ToString();
                break;
            }
        }

        listener = new HttpListener();
        listener.Prefixes.Add("http://*:" + port + "/");
        listener.Prefixes.Add("http://" + privateIpAddress + ":" + port + "/");
        listener.Prefixes.Add("http://127.0.0.1:" + port + "/");
        listener.Prefixes.Add("http://localhost:" + port + "/");
        listener.Start();

        Task.Run(HandleRequests);

        transform.Find("Start/Text").GetComponent<Text>().text = "Transfer Data (s)";
    }

    public void StartClient()
    {
        string ipAddress = transform.Find("IP").GetComponent<InputField>().text;
        string port = transform.Find("Port").GetComponent<InputField>().text;
        int portNumber;
        if (!int.TryParse(port, out portNumber))
        {
            Debug.LogError("Invalid port number!");
            return;
        }

        StartCoroutine(StartClientCoroutine(ipAddress, portNumber));
    }

    public void StopServer()
    {
        if (listener != null && listener.IsListening)
        {
            listener.Stop();
            hostFiles = false;
            transform.Find("Start/Text").GetComponent<Text>().text = "Server stopped.";
        }
    }


    private void Start()
    {
        saveDirectory = GameObject.Find("File").GetComponent<SaveFile>().GUPath;
        var privateIpAddress = "ERROR 2!";
        if (SceneManager.GetActiveScene().name == "Options") {
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    privateIpAddress = ip.ToString();
                    break;
                }
            }
            transform.Find("BG/IP/BG/Text").GetComponent<Text>().text = privateIpAddress;
        }
    }
}
