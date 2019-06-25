using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System.IO;

public class SinglePortScript : MonoBehaviour
{

    [SerializeField]
    private Text portName;

    [SerializeField]
    private Text portStatus;

    [SerializeField]
    private Button portButton;

    private SerialPort port;

    public string Name
    {
        set {
            portName.text = value;
            if (port != null) port.Close();
            port = new SerialPort(value, 115200, Parity.None, 8, StopBits.One);
        }

        get {
            return portName.text;
        }
    }
    public void CheckStatus()
    {
        try
        {
            if (!port.IsOpen) port.Open();
            port.WriteLine("check");
            if (port.BytesToRead > 0)
            {
                string resp = port.ReadExisting().Trim();
                portStatus.text = resp;
                Debug.Log("Received from MicroBit: " + resp);
            }
        }
        catch (System.UnauthorizedAccessException uaae)
        {
            portStatus.text = "Unauthorized";
        }
        catch (System.ArgumentOutOfRangeException aoore)
        {
            portStatus.text = "Check params";
        }
        catch (IOException ioe)
        {
            portStatus.text = "IO error";
        }
    }

    private void OnDestroy()
    {
        if (port != null && port.IsOpen) port.Close();
    }
}
