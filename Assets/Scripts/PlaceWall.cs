using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceWall : MonoBehaviour
{
    public Transform player;
    public Tilemap tilemap;
    public TileBase wallTile;
    public Sprite previewSprite;
    public GameObject previewObject;

   public placer facing;

    void Start()
    {
        facing = player.GetComponent<placer>();
        previewObject = new GameObject("TilePreview");
        var sr = previewObject.AddComponent<SpriteRenderer>();
        sr.sprite = previewSprite;
        sr.color = new Color(1, 1, 1, 0.5f);
        sr.sortingOrder = 10;
        gameObject.SetActive(false);
    }

    void Update()
    {
        ShowPreviewNextToPlayer();
    }

    public void ShowPreviewNextToPlayer()
    {
        if (previewObject == null) return;
        previewObject.SetActive(true);
        previewObject.transform.position = tilemap.GetCellCenterWorld(GetTargetTile());
    }

    public void PlaceTileNextToPlayer()
    {
        Vector3Int tilePos = GetTargetTile();
        tilemap.SetTile(tilePos, wallTile);
        FindAnyObjectByType<ScoreManager>().coinCount -= 10;

        var generator = FindAnyObjectByType<RandomWalkerGenerator>();
        if (generator != null)
        {
            Node nodeToRemove = null;
            Vector2 nodePos = new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f);
            foreach (var node in generator.nodeList)
            {
                if (Vector2.Distance(node.transform.position, nodePos) < 0.1f)
                {
                    nodeToRemove = node;
                    break;
                }
            }
            if (nodeToRemove != null)
            {
                generator.nodeList.Remove(nodeToRemove);
                generator.CreateConnections();
                foreach (var npc in FindObjectsByType<NPC_Controller>())
                    npc.path.Clear();
                Destroy(nodeToRemove.gameObject);
            }
        }

        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);
        foreach (var col in Physics2D.OverlapPointAll(worldPos))
        {
            if (col.gameObject.name == "node(Clone)")
                Destroy(col.gameObject);
        }

        RemovePreviewAndDisable();
    }

    public void RemovePreviewAndDisable()
    {
        if (previewObject != null)
            previewObject.SetActive(false);
        gameObject.SetActive(false);
    }

    Vector3Int GetTargetTile()
    {
        Vector3Int playerTile = tilemap.WorldToCell(player.position);
        Vector2Int dir = facing != null ? facing.Direction : Vector2Int.right;
        return playerTile + new Vector3Int(dir.x, dir.y, 0);
    }
}