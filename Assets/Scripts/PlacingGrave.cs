using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingGrave : MonoBehaviour
{
    [SerializeField] private GameObject prefabToPlace;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset = new Vector3(1, 0, 0); // Offset from player
    private GameObject previewInstance = null;

    // Call this to show the preview sprite next to the player
    public void ShowPreviewSprite()
    {
        Debug.Log("showing preview");
        if (previewInstance == null && prefabToPlace != null)
        {
            Debug.Log("instantiating preview");
            previewInstance = Instantiate(prefabToPlace, playerTransform.position + offset, Quaternion.identity);
            // Make the preview semi-transparent
            var sr = previewInstance.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Debug.Log("making preview semi-transparent");
                var color = sr.color;
                color.a = 0.5f;
                sr.color = color;
            }
            // Optionally disable any scripts/colliders on the preview
//            foreach (var c in previewInstance.GetComponents<Grave>())
  //               c.enabled = false;
        }
    }

    // Call this every frame to update the preview's position
    private void Update()
    {
        if (previewInstance != null && playerTransform != null)
        {
            previewInstance.transform.position = playerTransform.position + offset;
        }
    }

    // Call this to place the actual prefab next to the player
    public void PlacePrefab(InputAction.CallbackContext ctx)
    {
        if (prefabToPlace != null && playerTransform != null && ctx.started && FindAnyObjectByType<ScoreManager>().coinCount >= 50)
        {
            FindAnyObjectByType<ScoreManager>().coinCount -= 50;
            // Destroy the preview before placing the actual prefab
            HidePreviewSprite();
            var cannon = Instantiate(prefabToPlace, playerTransform.position + offset, Quaternion.identity);
            Debug.Log("placed");
            gameObject.SetActive(false); // Disable the placing script after placing
        }
    }
    // Call this to remove the preview (e.g., after placing)
    public void HidePreviewSprite()
    {
        if (previewInstance != null)
        {
            Destroy(previewInstance);
            previewInstance = null;
        }
    }
}
