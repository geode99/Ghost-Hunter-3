using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Thank you Annoying Orange for being more present on Kellen's screen than any amount of work, thank you AI code for not working outside of Giovanni's computer, and thank 
    public TMP_Text coinText;
    private int coinCount = 0;

    void Update()
    {
        coinText.text = "Ectoplasm " + coinCount.ToString();
    }
}
