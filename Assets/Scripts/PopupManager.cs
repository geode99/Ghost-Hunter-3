using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;

    public GameObject prefab;

    private void Awake()
    {
        instance = this;
    }

    public void Popup(string message,Transform parent)
    {
        GameObject newObject = Instantiate(prefab,FindAnyObjectByType<Canvas>().transform);

        if (newObject.TryGetComponent(out TMPro.TMP_Text text))
        {
            text.SetText(message);
        }

        Destroy(newObject, 1);
    }
}
