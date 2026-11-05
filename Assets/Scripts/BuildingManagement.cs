using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManagement : MonoBehaviour
{
    public GameObject PC;
    public GameObject PW;
    public GameObject PG;
    public GameObject PD;
    public float WallCost = 10f;
    public float GraveCost = 25f;
    public float CannonCost = 50f;

    private PlacingCannon cannonPlacer;
    private PlaceWall wallPlacer;
    private PlacingGrave gravePlacer;
    private Delete Delete;

    void Awake()
    {
        cannonPlacer = PC.GetComponent<PlacingCannon>();
        wallPlacer = PW.GetComponent<PlaceWall>();
        gravePlacer = PG.GetComponent<PlacingGrave>();
        Delete = PD.GetComponent<Delete>();
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
        PD.SetActive(false);
        PC.SetActive(true);
        gravePlacer.HidePreviewSprite();
        wallPlacer.RemovePreviewAndDisable();
        Delete.RemovePreviewAndDisable();
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
        PD.SetActive(false);
        cannonPlacer.HidePreviewSprite();
        gravePlacer.HidePreviewSprite();
        Delete.RemovePreviewAndDisable();
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
        PD.SetActive(false);
        PG.SetActive(true);
        wallPlacer.RemovePreviewAndDisable();
        cannonPlacer.HidePreviewSprite();
        gravePlacer.ShowPreviewSprite();
        Delete.RemovePreviewAndDisable();
    }

    public void PlaceGraveReal(InputAction.CallbackContext ctx)
    {
        if (PG.activeSelf)
            gravePlacer.PlacePrefab(ctx);
    }

    public void PlaceDelete()
    {
        PW.SetActive(false);
        PC.SetActive(false);
        PG.SetActive(false);
        cannonPlacer.HidePreviewSprite();
        gravePlacer.HidePreviewSprite();
        wallPlacer.RemovePreviewAndDisable();
        if (!CanAfford(WallCost)) return;
        PD.SetActive(true);
        Delete.ShowPreviewNextToPlayer();
    }

    public void PlaceDeleteReal(InputAction.CallbackContext ctx)
    {
        if (PD.activeSelf && ctx.started)
            Delete.PlaceTileNextToPlayer();
    }
}