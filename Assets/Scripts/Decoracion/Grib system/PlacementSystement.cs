using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystement : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    private MuebleDatabaseOS database;
    [SerializeField]
    private GameObject gridVisualization;
    [SerializeField]
    private AudioSource source;
    private GridData floorData, furnitureData;
   
    [SerializeField]
    private PreviewSystem preview;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    [SerializeField]
    private ObjectPlacer objectPlacer;
    IBuildingState buildingState;
    [SerializeField]
    private SoundFeedback soundFeedback;
    private Action rotateRightAction;
    private Action rotateLeftAction;
    private bool isProcessingAction = false;
    private void Start()
    {
        database = Resources.Load<MuebleDatabaseOS>("MuebleDatabases/MuebleData");
        if (DatabaseManager.Instance != null)
        {
            database = DatabaseManager.Instance.GetMuebleDatabase();
            Debug.Log($"PlacementSystement: Database obtenida = {database.name}");
        }

        if (database == null)
        {
            Debug.LogError("PlacementSystement: Database es NULL");
            return;
        }
        gridVisualization.SetActive(false);
        floorData = new();
        furnitureData = new();
        rotateRightAction = () => RotateFurniture(90);
        rotateLeftAction = () => RotateFurniture(-90);

    }
    public void StartPlacement(int ID)
    {
        if (database == null)
        {
            Debug.LogError("PlacementSystement: Database no disponible");
            return;
        }
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(ID,
                                         grid,
                                         preview,
                                         database,
                                         floorData,
                                         furnitureData,
                                         objectPlacer, soundFeedback);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        inputManager.OnRotateRight += () => RotateFurniture(90);
        inputManager.OnRotateLeft += () => RotateFurniture(-90);
        inputManager.OnRotateRight += rotateRightAction;
        inputManager.OnRotateLeft += rotateLeftAction;
    }
    public void StartRemoving()
    {
        if (database == null)
        {
            Debug.LogError("PlacementSystement: Database no disponible");
            return;
        }
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer, soundFeedback);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        isProcessingAction = true;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        buildingState.OnAction(gridPosition);
        isProcessingAction = false;
  }
    private void RotateFurniture(int rotationAmount)
    {
        if (buildingState == null) return; 

        if (buildingState is PlacementState placementState)
        {
            placementState.RotateFurniture(rotationAmount);
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        buildingState.UpdateState(gridPosition);
    }
    private void StopPlacement()
    {
        if (soundFeedback != null)
            soundFeedback.PlaySound(SoundType.Click);
        if (buildingState == null)
            return;
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        inputManager.OnRotateRight -= rotateRightAction;
        inputManager.OnRotateLeft -= rotateLeftAction;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
   
        private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }
}