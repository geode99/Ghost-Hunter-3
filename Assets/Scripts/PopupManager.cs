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
        Canvas canvas = FindAnyObjectByType<Canvas>();
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(parent.transform.position + (Vector3.up/2));
        
        GameObject newObject = Instantiate(prefab, canvas.transform);
        newObject.transform.position = screenPoint;

        if (newObject.TryGetComponent(out TMPro.TMP_Text text))
        {
            text.SetText(message);
        }

        Destroy(newObject, 1);
    }
}
