using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingCannon : MonoBehaviour
{
    [SerializeField] private GameObject prefabToPlace;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset = new Vector3(1, 0, 0); // Offset from player
    private GameObject previewInstance;
    private Collider2D[] colliders;

    // Call this to show the preview sprite next to the player
    public void ShowPreviewSprite()
    {
        if (previewInstance == null && prefabToPlace != null)
        {
            previewInstance = Instantiate(prefabToPlace, playerTransform.position + offset, Quaternion.identity);
            // Make the preview semi-transparent
            var sr = previewInstance.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                var color = sr.color;
                color.a = 0.5f;
                sr.color = color;
            }
            // Optionally disable any scripts/colliders on the preview
            foreach (var c in previewInstance.transform.GetChild(0).GetComponents<CannonShoot>())
                c.NotPlacing = false;
        }
    }

    // Call this every frame to update the preview's position
    private void Update()
    {
        if (previewInstance != null && playerTransform != null)
        {
            previewInstance.transform.position = playerTransform.position + offset;
            colliders = null;
            colliders = Physics2D.OverlapPointAll(previewInstance.transform.position);
        }
    }

    // Call this to place the actual prefab next to the player
    public void PlacePrefab(InputAction.CallbackContext ctx)
    {
        if (prefabToPlace != null && playerTransform != null && ctx.started && FindAnyObjectByType<ScoreManager>().coinCount >= 50 && CanPlace())
        {
            FindAnyObjectByType<ScoreManager>().coinCount -= 50;
            var previewRot = previewInstance.transform.GetChild(0).transform.rotation;
            // Destroy the preview before placing the actual prefab
            HidePreviewSprite();

            var cannon = Instantiate(prefabToPlace, playerTransform.position + offset, Quaternion.identity);
            cannon.transform.GetChild(0).transform.rotation = previewRot; // Set the rotation to match the preview
            Debug.Log("placed");
            gameObject.SetActive(false); // Disable the placing script after placing
        }
    }
    public void Rotate(InputAction.CallbackContext ctx) { 
        if(ctx.started && previewInstance != null) {
            previewInstance.transform.GetChild(0).Rotate(0, 0, 90); // Rotate 90 degrees around Z-axis
        }
    }
    private bool CanPlace()
    {
        foreach (var col in colliders)
        {
            if (col.gameObject.CompareTag("placeable") && col.gameObject != previewInstance)
            {
                return false;
            }
        }

        return true;
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
