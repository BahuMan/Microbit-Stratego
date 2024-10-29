using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class MicroBitControl : MonoBehaviour
{
    private UnityEngine.UI.Image mbitGUI;
    private PlayerControl[] players;
    private Dictionary<string, PlayerControl> nameToPlayer = new Dictionary<string, PlayerControl>();

    private SerialPort mbitport = null;
    private Dictionary<string, PlayerControl> NameToController = new Dictionary<string, PlayerControl>();

    void Start()
    {
        mbitGUI = this.GetComponent<UnityEngine.UI.Image>();
        players = GameObject.FindObjectsByType<PlayerControl>(FindObjectsSortMode.None);
        foreach (PlayerControl player in players)
        {
            player.gameObject.SetActive(false);
        }
        StartCoroutine(FindMicroBit());
    }

    Color[] blink = {new(0.0f, 0.0f, 0.0f, 0.0f), new(1.0f, 1.0f, 1.0f, 1.0f) }; //alternate between opaque white and transparent black
    private IEnumerator FindMicroBit()
    {
        while (mbitport == null)
        {
            yield return new WaitForSeconds(.5f);

            Debug.Log("looking for microbit...");
            mbitGUI.color = blink[(int)Time.time % 2];
            string[] portnames = SerialPort.GetPortNames();
            foreach (string portname in portnames)
            {
                Debug.Log("trying " + portname);
                SerialPort p = new SerialPort(portname, 115200);
                p.Open();
                if (p.IsOpen)
                {
                    p.Write("horserace");
                    yield return new WaitForSeconds(0.1f);
                    if (p.IsOpen && p.BytesToRead > 0)
                    {
                        string reply = p.ReadLine();
                        Debug.Log(portname + " responded: " + reply);
                        if ("letsgo".Equals(reply))
                        {
                            mbitport = p;
                            break;
                        }
                    }
                    p.Close();
                }
            }
        }
        mbitGUI.color = blink[1];

    }

    StringBuilder naam = new StringBuilder();
    StringBuilder instructie = new StringBuilder();
    enum bufferstatus { GARBAGE, NAAM, INSTRUCTIE}
    bufferstatus mbitstatus = bufferstatus.GARBAGE;
    private void ReadMicroBit()
    {
        if (mbitport == null) return;   //ignore
        if (!mbitport.IsOpen)           //force re-init
        {
            mbitstatus = bufferstatus.GARBAGE;
            Invoke("Start", 0.0f);
        }

        while (mbitport.BytesToRead > 0)
        {
            char c = (char)mbitport.ReadChar();

            if (mbitstatus == bufferstatus.GARBAGE && c == '#')
            {
                mbitstatus = bufferstatus.NAAM; //start met naam lezen
            }
            else if (mbitstatus == bufferstatus.NAAM)
            {
                if (c == ' ') mbitstatus = bufferstatus.INSTRUCTIE;
                else naam.Append(c);
            }
            else if (mbitstatus == bufferstatus.INSTRUCTIE)
            {
                if (c == '#')
                {
                    ProcessPlayer(naam.ToString(), instructie.ToString());
                    naam.Clear();
                    instructie.Clear();
                    mbitstatus = bufferstatus.GARBAGE;
                }
                else instructie.Append(c);
            }

        }


    }

    private void ProcessPlayer(string naam, string instructie)
    {
        //TODO: find gameobject for player
        PlayerControl thisplayer = null;
        if (nameToPlayer.ContainsKey(naam))
        {
            thisplayer = nameToPlayer[naam];
        }
        else
        {
            foreach (var p in players)
            {
                if (!p.gameObject.activeSelf)
                {
                    p.gameObject.SetActive(true);
                    nameToPlayer[naam] = p;
                    thisplayer = p;
                    p.VolgendeUiterlijk(naam); //choose first random look
                    break;
                }
            }
        }

        if (thisplayer != null)
        {
            //process instructie
            if ("hop".Equals(instructie)) thisplayer.Hop(instructie);
            else if ("los".Equals(instructie)) thisplayer.Hop(instructie);
            else if ("looks".Equals(instructie)) thisplayer.VolgendeUiterlijk(naam);
            else Debug.LogError("onbekend, naam=" + naam + ", instructie=" + instructie);
        }
        else Debug.LogError("kan niet meer toevoegen, naam=" + naam);
    }

    void Update()
    {
        ReadMicroBit();
    }

}
