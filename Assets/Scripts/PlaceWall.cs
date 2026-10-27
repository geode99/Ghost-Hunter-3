using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceWall : MonoBehaviour
{
    public Transform player; // Assign in Inspector
    public Tilemap tilemap; // Assign in Inspector
    public TileBase wallTile; // Assign in Inspector
    public Sprite previewSprite; // Assign in Inspector

    public GameObject previewObject;

    void Start()
    {
        // Create the preview object
        previewObject = new GameObject("TilePreview");
        var sr = previewObject.AddComponent<SpriteRenderer>();
        sr.sprite = previewSprite;
        sr.color = new Color(1, 1, 1, 0.5f); // semi-transparent
        sr.sortingOrder = 10; // Ensure it's on top of other sprites
        gameObject.SetActive(false); // Hide the preview initially
    }

    void Update()
    {
        ShowPreviewNextToPlayer();
    }

    // Shows the preview sprite next to the player
    public void ShowPreviewNextToPlayer()
    {
        previewObject.SetActive(true);
        Vector3Int tilePos = GetNextToPlayerTilePosition();
        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);
        previewObject.transform.position = worldPos;
    }

    // Places the tile on the tilemap at the preview position
    public void PlaceTileNextToPlayer()
    {
        Vector3Int tilePos = GetNextToPlayerTilePosition();
        tilemap.SetTile(tilePos, wallTile);
        FindAnyObjectByType<ScoreManager>().coinCount -= 10;
        // Find the RandomWalkerGenerator in the scene
        var generator = FindAnyObjectByType<RandomWalkerGenerator>();
        if (generator != null)
        {
            // Find the node at this position (nodes are placed at x+0.5, y+0.5)
            Node nodeToRemove = null;
            foreach (var node in generator.nodeList)
            {
                if (Vector2.Distance(node.transform.position, new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f)) < 0.1f)
                {
                    nodeToRemove = node;
                    break;
                }
            }
            if (nodeToRemove != null)
            {
                generator.nodeList.Remove(nodeToRemove);

                // Remove all connections to the deleted node
                generator.CreateConnections(); // Recreate connections after removing the node

                foreach (var npc in FindObjectsByType<NPC_Controller>())
                {
                    npc.path.Clear(); // Clear paths for all NPCs
                }
                Destroy(nodeToRemove.gameObject);
            }
        }

        // Also destroy any "node(Clone)" with a collider at this position (optional, for safety)
        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);
        Collider2D[] colliders = Physics2D.OverlapPointAll(worldPos);
        foreach (var col in colliders)
        {
            if (col.gameObject.name == "node(Clone)")
            {
                Destroy(col.gameObject);
            }
        }

        // Remove preview and disable this GameObject
        RemovePreviewAndDisable();
    }

    // Removes the preview object and disables this GameObject
    public void RemovePreviewAndDisable()
    {
        if (previewObject != null)
        {
            previewObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    // Helper: Gets the tile position next to the player (e.g., to the right)
    Vector3Int GetNextToPlayerTilePosition()
    {
        Vector3 playerPos = player.position;
        Vector3Int playerTile = tilemap.WorldToCell(playerPos);

        // Example: always to the right of the player
        Vector3Int nextTile = playerTile + Vector3Int.right;
        return nextTile;
    }
}
