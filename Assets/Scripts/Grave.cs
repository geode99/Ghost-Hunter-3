using UnityEngine;

public class Grave : MonoBehaviour
{
    public ScoreManager SM;
    public EnemySpawner ES;
    private int wave = 0;
    public int reward = 5;
    private SpriteRenderer sr;
    public Sprite opt1;
    public Sprite opt2;
    public Sprite opt3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SM = FindAnyObjectByType<ScoreManager>();
        ES = FindAnyObjectByType<EnemySpawner>();
        sr = GetComponent<SpriteRenderer>();
        wave = ES.Rounds;
        switch (Random.Range(0, 3))
        {
            case 0: sr.sprite = opt1; break;
            case 1: sr.sprite = opt2; break;
            case 2: sr.sprite = opt3; break;
        }

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
