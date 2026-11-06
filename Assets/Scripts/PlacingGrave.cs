using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlacingGrave : MonoBehaviour
{
    [SerializeField] private GameObject prefabToPlace;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float distance = 1f;

    private GameObject previewInstance;
    private Collider2D[] colliders = new Collider2D[0];
    
    private placer facing;

    private Vector3 TargetPosition
    {
        get
        {
            if (facing == null)
                facing = playerTransform.GetComponent<placer>();
            Vector2Int dir = facing != null ? facing.Direction : Vector2Int.right;
            return playerTransform.position + new Vector3(dir.x, dir.y, 0) * distance;
        }
    }

    public void ShowPreviewSprite()
    {
        if (previewInstance != null || prefabToPlace == null) return;

        previewInstance = Instantiate(prefabToPlace, TargetPosition, Quaternion.identity);
        var sr = previewInstance.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            var color = sr.color;
            color.a = 0.5f;
            sr.color = color;
        }
        foreach (var c in previewInstance.GetComponents<Grave>())
            c.enabled = false;
    }

    private void Update()
    {
        if (previewInstance == null || playerTransform == null) return;
        previewInstance.transform.position = TargetPosition;
        colliders = Physics2D.OverlapPointAll(previewInstance.transform.position);
    }

    public void PlacePrefab(InputAction.CallbackContext ctx)
    {
        if (prefabToPlace == null || playerTransform == null || !ctx.started) return;
        var score = FindAnyObjectByType<ScoreManager>();
        if (score.coinCount < 25 || !CanPlace()) return;

        score.coinCount -= 25;
        Vector3 pos = TargetPosition;
        HidePreviewSprite();
        Instantiate(prefabToPlace, pos, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private bool CanPlace()
    {
        foreach (var col in colliders)
        {
            if (col.gameObject.CompareTag("placeable") && !col.gameObject.CompareTag("Wall") && col.gameObject != previewInstance)
                return false;
        }
        return true;
    }

    public void HidePreviewSprite()
    {
        if (previewInstance != null)
        {
            Destroy(previewInstance);
            previewInstance = null;
        }
    }
}