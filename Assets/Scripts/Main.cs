using UnityEngine;
using Unity.DotsNetKit.NetCode;
using UnityEngine.Ucg.Matchmaking;

public class Main : MonoBehaviour
{
    private bool showButton = true;
    private string endpoint;
    private Matchmaker matchmaker;    
    
    void Start()
    {
        Screen.SetResolution(640, 480, false);
        SimpleConsole.Create();
        // endpoint = "172.17.129.6:30593";
        // matchmaker = new Matchmaker(endpoint);
        // Exec();
    }

    void OnGUI()
    {
        // om_client.Update();
        if (!showButton)
            return;

        if (GUI.Button(new Rect(100, 100, 100, 50), "Start sever"))
        {
            Debug.Log("Click Btn");
            Application.targetFrameRate = 20;
            SimpleServer.Start("myAppId");
            showButton = false;
        }

        if (GUI.Button(new Rect(100, 200, 100, 50), "Start client"))
        {
            Debug.Log("Click Btn");
            Application.targetFrameRate = 60;
            SimpleClient.Connect("myAppId");
            showButton = false;
        }
    }
}
