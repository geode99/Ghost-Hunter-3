using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.InputSystem; // Added for the New Input System

public class EnemySpawner : MonoBehaviour
{
    public int Rounds = 0;
    public NPC_Controller npc;
    public NPC_Controller npc_Ranged;
    public NPC_Controller npc_Tank;
    public bool CanStart;

    [Header("UI")]
    public TMP_Text waveText;
    private NPC_Controller newNPC;

    void Start()
    {
        UpdateWaveText();
    }

    void Update()
    {
        if (CanStart)
        {
            SpawnEnemy();
            CanStart = false;
        }

        // Listens for the X Button (Button West) using the New Input System package
        if (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        // Performance fix: Caches the generator to avoid searching the scene inside the loop
        var generator = FindAnyObjectByType<RandomWalkerGenerator>();
        if (generator == null || generator.nodeList.Count == 0) return;

        for (int i = Rounds; i >= 0; i--)
        {
            Node randNode = generator.nodeList[Random.Range(0, generator.nodeList.Count)];

            var rand = Random.Range(0, 4);
            if (rand == 2 && Rounds >= 20)
            {
                newNPC = Instantiate(npc_Ranged, randNode.transform.position, Quaternion.identity);
            }
            else if (rand == 1 && Rounds >= 10)
            {
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
            PopupManager.instance.Popup("+" + grave.reward, grave.transform);
        }
    }
}
