using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    MuebleDatabaseOS database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;
    SoundFeedback soundFeedback;
    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          MuebleDatabaseOS database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer,
           SoundFeedback soundFeedback)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.soundFeedback = soundFeedback;
        selectedObjectIndex = database.muebleData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.muebleData[selectedObjectIndex].Prefab,
                database.muebleData[selectedObjectIndex].Size);
        }
        else
            throw new System.Exception($"No object with ID {iD}");

    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        if (soundFeedback == null || objectPlacer == null || grid == null || previewSystem == null || database == null)
        {
            Debug.LogError("PlacementState is missing required dependencies!");
            return;
        }
        int currentRotation = previewSystem.GetCurrentRotation();
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex, currentRotation);
        if (placementValidity == false)
        {
            soundFeedback.PlaySound(SoundType.wrongPlacement);
            return;
        }
        soundFeedback.PlaySound(SoundType.Place);
        int index = objectPlacer.PlaceObject(
            database.muebleData[selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPosition),
            currentRotation);

        GridData selectedData = database.muebleData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;
        Vector2Int rotatedSize = GridData.GetRotatedSize(
       database.muebleData[selectedObjectIndex].Size,
       currentRotation);
        selectedData.AddObjectAt(gridPosition,
     rotatedSize, 
     database.muebleData[selectedObjectIndex].ID,
     index,
     currentRotation);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition),
                CheckPlacementValidity(gridPosition, selectedObjectIndex, currentRotation));
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex, int rotation)
    {
        GridData selectedData = database.muebleData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.muebleData[selectedObjectIndex].Size, rotation);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        int currentRotation = previewSystem.GetCurrentRotation();
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex, currentRotation);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
    public void RotateFurniture(int rotationAmount)
    {
        previewSystem.RotatePreview(rotationAmount);
        
    }
}