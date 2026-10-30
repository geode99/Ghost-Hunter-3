using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManagement : MonoBehaviour
{
    public GameObject PC;
    public GameObject PW;
    public GameObject PG;
    public float GraveCost = 25f;
    public float CannonCost = 50f;

    // Assign the cannon prefab in the Inspector
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlaceCannon()
    {
        if (FindAnyObjectByType<ScoreManager>().coinCount >= 50)
        {
            PW.SetActive(false);
            PG.SetActive(false);
            PC.SetActive(true);
            PG.GetComponent<PlacingGrave>().HidePreviewSprite();
            PW.GetComponent<PlaceWall>().RemovePreviewAndDisable();
            PC.GetComponent<PlacingCannon>().ShowPreviewSprite();
        }
    }
    public void PlaceCannonReal(InputAction.CallbackContext ctx)
    {
        if (PC.activeSelf)
        {
            PC.GetComponent<PlacingCannon>().PlacePrefab(ctx);
        }

    }
    public void PlaceWall()
    {
        PC.SetActive(false);
        PG.SetActive(false);
        PC.GetComponent<PlacingCannon>().HidePreviewSprite();
        PG.GetComponent<PlacingGrave>().HidePreviewSprite();
        if (FindAnyObjectByType<ScoreManager>().coinCount >= 10)
        {
            PW.SetActive(true);
            PW.GetComponent<PlaceWall>().ShowPreviewNextToPlayer();
        }
    }
    public void PlaceWallReal(InputAction.CallbackContext ctx)
    {
        if (PW.activeSelf && ctx.started)
        {
            PW.GetComponent<PlaceWall>().PlaceTileNextToPlayer();
        }
    }
    public void PlaceGrave()
    {
        if (FindAnyObjectByType<ScoreManager>().coinCount >= GraveCost)
        {
            PW.SetActive(false);
            PC.SetActive(false);
            PG.SetActive(true);
            PW.GetComponent<PlaceWall>().RemovePreviewAndDisable();
            PC.GetComponent<PlacingCannon>().HidePreviewSprite();
            PG.GetComponent<PlacingGrave>().ShowPreviewSprite();
        }
    }
    public void PlaceGraveReal(InputAction.CallbackContext ctx)
    {
        if (PG.activeSelf)
        {
            PG.GetComponent<PlacingGrave>().PlacePrefab(ctx);
        }

    }
}
