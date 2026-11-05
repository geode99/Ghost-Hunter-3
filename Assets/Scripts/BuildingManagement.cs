using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManagement : MonoBehaviour
{
    public GameObject PC;
    public GameObject PW;
    public GameObject PG;
    public float WallCost = 10f;
    public float GraveCost = 25f;
    public float CannonCost = 50f;

    private PlacingCannon cannonPlacer;
    private PlaceWall wallPlacer;
    private PlacingGrave gravePlacer;

    void Awake()
    {
        cannonPlacer = PC.GetComponent<PlacingCannon>();
        wallPlacer = PW.GetComponent<PlaceWall>();
        gravePlacer = PG.GetComponent<PlacingGrave>();
    }

    private bool CanAfford(float cost)
    {
        return FindAnyObjectByType<ScoreManager>().coinCount >= cost;
    }

    public void PlaceCannon()
    {
        if (!CanAfford(CannonCost)) return;
        PW.SetActive(false);
        PG.SetActive(false);
        PC.SetActive(true);
        gravePlacer.HidePreviewSprite();
        wallPlacer.RemovePreviewAndDisable();
        cannonPlacer.ShowPreviewSprite();
    }

    public void PlaceCannonReal(InputAction.CallbackContext ctx)
    {
        if (PC.activeSelf)
            cannonPlacer.PlacePrefab(ctx);
    }

    public void PlaceWall()
    {
        PC.SetActive(false);
        PG.SetActive(false);
        cannonPlacer.HidePreviewSprite();
        gravePlacer.HidePreviewSprite();
        if (!CanAfford(WallCost)) return;
        PW.SetActive(true);
        wallPlacer.ShowPreviewNextToPlayer();
    }

    public void PlaceWallReal(InputAction.CallbackContext ctx)
    {
        if (PW.activeSelf && ctx.started)
            wallPlacer.PlaceTileNextToPlayer();
    }

    public void PlaceGrave()
    {
        if (!CanAfford(GraveCost)) return;
        PW.SetActive(false);
        PC.SetActive(false);
        PG.SetActive(true);
        wallPlacer.RemovePreviewAndDisable();
        cannonPlacer.HidePreviewSprite();
        gravePlacer.ShowPreviewSprite();
    }

    public void PlaceGraveReal(InputAction.CallbackContext ctx)
    {
        if (PG.activeSelf)
            gravePlacer.PlacePrefab(ctx);
    }
}