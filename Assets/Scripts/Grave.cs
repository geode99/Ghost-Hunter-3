using UnityEngine;

public class Grave : MonoBehaviour
{
    public ScoreManager SM;
    public EnemySpawner ES;
    private int wave = 0;
    public int reward = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SM = FindAnyObjectByType<ScoreManager>();
        ES = FindAnyObjectByType<EnemySpawner>();
        wave = ES.Rounds;
    }

    // Update is called once per frame
    void Update()
    {
        if(wave < ES.Rounds)
        {
            SM.coinCount += reward;
            wave = ES.Rounds;
            reward++;
        }
    }
}
