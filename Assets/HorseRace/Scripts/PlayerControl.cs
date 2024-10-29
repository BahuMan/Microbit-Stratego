using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private GameController ctrl;
    private List<GameObject> uiterlijk = new List<GameObject>();
    private List<MeshRenderer> mijlpaal = new List<MeshRenderer>();
    private List<MeshRenderer> finish = new List<MeshRenderer>();
    private Transform canvasTrans;
    private TextMeshProUGUI PlayerNameTMP;
    private int actiefUiterlijk = 0;
    private bool hop = true;
    private Color nieuweKleur = Color.white;
    private bool running=false;

    // Start is called before the first frame update
    void Start()
    {
        ctrl = GameObject.FindObjectOfType<GameController>();
        ctrl.GameStarted.AddListener(OnGameStarted);
        ctrl.GameEnded.AddListener(OnGameEnded);
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Mijl")) mijlpaal.Add(child.GetComponent<MeshRenderer>());
            else if (child.name.StartsWith("Finish")) finish.Add(child.GetComponent<MeshRenderer>());
            else if (child.name.StartsWith("Canvas"))
            {
                canvasTrans = child.transform;
                PlayerNameTMP = child.GetComponentInChildren<TextMeshProUGUI>();
            }
            else uiterlijk.Add(child.gameObject);
        }
    }

    private void OnGameEnded()
    {
        running = false;
    }

    private void OnGameStarted()
    {
        running = true;
        foreach (GameObject pion in uiterlijk)
        {
            pion.transform.localPosition = Vector3.up * pion.transform.localPosition.y;
        }
    }

    public void VolgendeUiterlijk(string naam)
    {
        int nieuwUiterlijk = (actiefUiterlijk+1) % uiterlijk.Count;

        for (int i = 0; i < uiterlijk.Count; i++)
        {
            uiterlijk[i].SetActive(i == nieuwUiterlijk);
        }

        canvasTrans.SetParent(uiterlijk[nieuwUiterlijk].transform, false);
        uiterlijk[nieuwUiterlijk].transform.position = uiterlijk[actiefUiterlijk].transform.position;
        nieuweKleur = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1.0f, 0.8f);
        uiterlijk[nieuwUiterlijk].GetComponent<SpriteRenderer>().color = nieuweKleur;
        PlayerNameTMP.color = nieuweKleur;
        PlayerNameTMP.text = naam;
            
        actiefUiterlijk = nieuwUiterlijk;
    }

    public void Hop(string instructie)
    {
        if (!running) return;
        if (hop && "hop".Equals(instructie)) uiterlijk[actiefUiterlijk].transform.Translate(Vector3.right, Space.Self);
        hop = !"hop".Equals(instructie);

        UpdateMijlpalen();
    }

    private void UpdateMijlpalen()
    {
        foreach (MeshRenderer mp in mijlpaal)
        {
            if (mp.transform.position.x < uiterlijk[actiefUiterlijk].transform .position.x)
            {
                mp.material.color = nieuweKleur;
            }
        }

        foreach (var f in finish)
        {
            if (f.transform.position.x < uiterlijk[actiefUiterlijk].transform .position.x)
            {
                ctrl.Finished(PlayerNameTMP.text, nieuweKleur);
                break;
            }
        }
    }
}
