using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManagement : MonoBehaviour
{
    public GameObject PC;
    public GameObject PW;
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
            PC.SetActive(true);
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
        PC.GetComponent<PlacingCannon>().HidePreviewSprite();
        if (FindAnyObjectByType<ScoreManager>().coinCount >= 10) { 
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
}
