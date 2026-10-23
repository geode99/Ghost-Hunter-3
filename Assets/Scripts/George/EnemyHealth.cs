using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float Health = 3;
    public ScoreManager SC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0) {
            SC.Score += 5;
            Destroy(gameObject);
        }
    }
}
