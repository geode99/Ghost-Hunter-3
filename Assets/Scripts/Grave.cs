using UnityEngine;

public class Grave : MonoBehaviour
{
    public ScoreManager SM;
    public EnemySpawner ES;
    private int wave = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SM = FindAnyObjectByType<ScoreManager>();
        ES = FindAnyObjectByType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(wave < ES.Rounds)
        {
            SM.coinCount += 5;
            wave = ES.Rounds;
        }
    }
}
