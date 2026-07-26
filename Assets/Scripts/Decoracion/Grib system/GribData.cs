using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();
    private Dictionary<(Vector3Int, Vector2Int, int), List<Vector3Int>> positionCache = new();
    public void AddObjectAt(Vector3Int gridPosition,
                            Vector2Int objectSize,
                            int ID,
                            int placedObjectIndex,
                            int rotation = 0)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize, rotation);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex, rotation);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell positiojn {pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize, int rotation)
    {
        var key = (gridPosition, objectSize, rotation);
        List<Vector3Int> returnVal = new();
        // Iterar sobre el tamaño ORIGINAL, no el rotado
        // Los offsets de rotación están diseñados para este loop
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                Vector3Int cellOffset = new Vector3Int(x, 0, y);
                if (rotation == 90)
                    cellOffset = new Vector3Int(objectSize.y - 1 - y, 0, x);
                else if (rotation == 180)
                    cellOffset = new Vector3Int(objectSize.x - 1 - x, 0, objectSize.y - 1 - y);
                else if (rotation == 270)
                    cellOffset = new Vector3Int(y, 0, objectSize.x - 1 - x);

                returnVal.Add(gridPosition + cellOffset);
            }
        }
        positionCache[key] = returnVal;
        return returnVal;
    }
    public void ClearCache()
    {
        positionCache.Clear();
    }
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int rotation = 0)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize, rotation);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }


    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return -1;
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
    public static Vector2Int GetRotatedSize(Vector2Int originalSize, int rotation)
    {
        // Normalizar rotaci?n a 0-360
        rotation = rotation % 360;

        // En rotaciones de 90 y 270 grados, se intercambian ancho y alto
        if (rotation == 90 || rotation == 270)
        {
            return new Vector2Int(originalSize.y, originalSize.x);
        }

        return originalSize;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }
    public int Rotation { get; private set; }
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex, int rotation = 0)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
        Rotation = rotation;
    }

}