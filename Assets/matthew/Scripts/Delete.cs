using UnityEngine;
using UnityEngine.Tilemaps;

public class Delete : MonoBehaviour
{
    public Transform player;
    public Tilemap tilemap;
    public Tilemap tilemapFloor;
    public TileBase wallTile;
    public TileBase floorTile;
    public Sprite previewSprite;
    public GameObject previewObject;
    public Node node;

    Vector3Int tilePos;

    Vector3 worldPos;

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
        tilemapFloor.SetTile(tilePos, floorTile);
        tilemap.SetTile(tilePos, null);
        checkSurroundingsForWalls();
        FindAnyObjectByType<ScoreManager>().coinCount -= 10;

        var generator = FindAnyObjectByType<RandomWalkerGenerator>();

        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);
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
        var CreatedNode = Instantiate(node);
        CreatedNode.transform.position = worldPos;
        generator.nodeList.Add(CreatedNode);
        generator.CreateConnections();

        RemovePreviewAndDisable();
    }

    public void checkSurroundingsForWalls()
    {
        var colliders = Physics2D.OverlapPointAll(previewObject.transform.position + Vector3.right);
        colliders = null;
        if (colliders == null)
        {
            Vector3Int newTilePos = Vector3Int.RoundToInt(previewObject.transform.position);
            tilemap.SetTile(newTilePos, wallTile);
        }
        //colliders = Physics2D.OverlapPointAll(previewObject.transform.position + Vector3.left);
        //if (colliders == null)
        //{
        //    Vector3Int newTilePos = Vector3Int.RoundToInt(previewObject.transform.position + Vector3.left);
        //    tilemap.SetTile(newTilePos, wallTile);
        //}
        //colliders = Physics2D.OverlapPointAll(previewObject.transform.position + Vector3.up);
        //if (colliders == null)
        //{
        //    Vector3Int newTilePos = Vector3Int.RoundToInt(previewObject.transform.position + Vector3.up);
        //    tilemap.SetTile(newTilePos, wallTile);
        //}
        //colliders = Physics2D.OverlapPointAll(previewObject.transform.position + Vector3.down);
        //if (colliders == null)
        //{
        //    Vector3Int newTilePos = Vector3Int.RoundToInt(previewObject.transform.position + Vector3.down);
        //    tilemap.SetTile(newTilePos, wallTile);
        //}
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
