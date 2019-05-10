using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ListSerialPorts : MonoBehaviour
{

    [SerializeField]
    private SinglePortScript portPrefab;

    private Dictionary<string, SinglePortScript> PortnameToGUIMap = new Dictionary<string, SinglePortScript>();

    void Start()
    {
        StartCoroutine(UpdateSerialPorts());
    }

    private IEnumerator UpdateSerialPorts()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            Dictionary<string, SinglePortScript> newPortsMap = new Dictionary<string, SinglePortScript>();
            foreach (string portname in SerialPort.GetPortNames())
            {
                if (PortnameToGUIMap.ContainsKey(portname))
                {
                    //no need to create GUI element; it was created in a previous iteration
                    newPortsMap[portname] = PortnameToGUIMap[portname];
                    PortnameToGUIMap[portname].CheckStatus();
                }
                else
                {
                    SinglePortScript newPort = Instantiate<SinglePortScript>(portPrefab, this.transform);
                    newPort.gameObject.SetActive(true);
                    newPort.Name = portname;
                    newPortsMap[portname] = newPort;
                    newPort.CheckStatus();
                }
            }

            //remove any GUI elements for ports that were no longer found:
            foreach (string oldport in new List<string>(PortnameToGUIMap.Keys))
            {
                if (!newPortsMap.ContainsKey(oldport))
                {
                    GameObject GUI = PortnameToGUIMap[oldport].gameObject;
                    Destroy(GUI);
                }
            }

            PortnameToGUIMap = newPortsMap;
        }
    }

}
