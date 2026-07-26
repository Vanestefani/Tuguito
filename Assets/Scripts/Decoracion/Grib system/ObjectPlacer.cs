using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();
    [SerializeField]
    private List<int> placedObjectRotations = new();
    [SerializeField] private List<int> freeIndices = new();

    public int PlaceObject(GameObject prefab, Vector3 position, int rotation = 0)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        newObject.transform.rotation = Quaternion.Euler(0, rotation, 0);

        int index;
        if (freeIndices.Count > 0)
        {
            // Reutilizar índice reciclado
            index = freeIndices[freeIndices.Count - 1];
            freeIndices.RemoveAt(freeIndices.Count - 1);
            placedGameObjects[index] = newObject;
            placedObjectRotations[index] = rotation;
        }
        else
        {
            // Nuevo índice
            placedGameObjects.Add(newObject);
            placedObjectRotations.Add(rotation);
            index = placedGameObjects.Count - 1;
        }

        return index;
      
    }
    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex
            || placedGameObjects[gameObjectIndex] == null)
            return;
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
        freeIndices.Add(gameObjectIndex);

    }
}