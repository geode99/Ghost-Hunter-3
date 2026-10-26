using UnityEngine;
using TMPro; // Add this for TextMeshPro support

public class EnemySpawner : MonoBehaviour
{
    public int Rounds = 0;
    public NPC_Controller npc;   
    public bool CanStart;

    [Header("UI")]
    public TMP_Text waveText; // Assign this in the Inspector

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

                NPC_Controller newNPC = Instantiate(npc, randNode.transform.position, Quaternion.identity);

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
