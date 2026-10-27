using UnityEngine;
using TMPro;
using JetBrains.Annotations; // Add this for TextMeshPro support

public class EnemySpawner : MonoBehaviour
{
    public int Rounds = 0;
    public NPC_Controller npc;   
    public NPC_Controller npc_Ranged; // Assign this in the Inspector
    public NPC_Controller npc_Tank; // Assign this in the Inspector
    public bool CanStart;

    [Header("UI")]
    public TMP_Text waveText; // Assign this in the Inspector
    private NPC_Controller newNPC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateWaveText();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanStart)
        {
            SpawnEnemy();
            CanStart = false;
        }
    }

    public void SpawnEnemy()
    {
        for (int i = Rounds; i >= 0; i--)
        {
                Node randNode = FindAnyObjectByType<RandomWalkerGenerator>().nodeList[Random.Range(0, FindAnyObjectByType<RandomWalkerGenerator>().nodeList.Count)];




            var rand = Random.Range(0, 4);
            if (rand == 2 && Rounds >= 20)
            {
                newNPC = Instantiate(npc_Ranged, randNode.transform.position, Quaternion.identity);
            }
            else if (rand == 1 && Rounds >= 10) {
                newNPC = Instantiate(npc_Tank, randNode.transform.position, Quaternion.identity);
            }
            else
            {
                newNPC = Instantiate(npc, randNode.transform.position, Quaternion.identity);
            }


            newNPC.currentNode = randNode;
        }
        Rounds++;
        UpdateWaveText();
    }

    private void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {Rounds}";
        }

        foreach (Grave grave in FindObjectsByType<Grave>())
        {
            PopupManager.instance.Popup("+5", grave.transform);
        }
    }
}
