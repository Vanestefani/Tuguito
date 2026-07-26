using UnityEngine;
public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialsPrefab;
    private Material previewMaterialInstance;
    private Renderer cellIndicatorRenderer;
    private int currentRotation = 0;
    private Vector2Int originalSize = Vector2Int.zero;
    private Vector3 lastCursorPosition = Vector3.zero;
    private void Start()
    {

        previewMaterialInstance = new Material(previewMaterialsPrefab);

        previewMaterialInstance.color = Color.yellow;

        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
        cellIndicator.SetActive(false);
    }
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        currentRotation = 0;
        originalSize = size;
        Collider[] colliders = previewObject.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        Rigidbody[] rigidbodies = previewObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        PreparePreavie(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreavie(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
                      Material[] oldMats = renderer.sharedMaterials;
            Material[] newMats = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < newMats.Length; i++)
            {
                newMats[i] = previewMaterialInstance;
            }
            renderer.sharedMaterials = newMats;

        }

    }
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if (previewObject != null)
            Destroy(previewObject);
        currentRotation = 0;
    }
    public void RotatePreview(int rotationAmount)
    {
        if (previewObject == null)
            return;
        currentRotation = (currentRotation + rotationAmount) % 360;
        if (currentRotation < 0) 
            currentRotation += 360;
        previewObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        UpdateCellIndicatorForRotation();
    }
    private void UpdateCellIndicatorForRotation()
    {
        if (cellIndicator == null || originalSize == Vector2Int.zero)
            return;
        Vector2Int rotatedSize = GridData.GetRotatedSize(originalSize, currentRotation);
        cellIndicator.transform.rotation = Quaternion.identity;
        cellIndicator.transform.localScale = new Vector3(rotatedSize.x, 1, rotatedSize.y);
        Vector3 adjustedPosition = new Vector3(lastCursorPosition.x, previewYOffset, lastCursorPosition.z);
        cellIndicator.transform.position = adjustedPosition;
    }
   
    public int GetCurrentRotation()
    {
        return currentRotation;
    }
    public void ResetRotation()
    {
        currentRotation = 0;
        if (previewObject != null)
        {
            previewObject.transform.rotation = Quaternion.identity;
        }
        UpdateCellIndicatorForRotation();
    }
    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }
        MoveCursorWithoutRotation(position);
        ApplyFeedbackToCursor(validity);
    }
    private void MoveCursorWithoutRotation(Vector3 position)
    {
        lastCursorPosition = position;
        Vector3 adjustedPosition = new Vector3(position.x, previewYOffset, position.z);
        cellIndicator.transform.position = adjustedPosition;
    }
    private void ApplyFeedbackToPreview(bool validity)
    {
        if (cellIndicatorRenderer == null) return;

        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;

        previewMaterialInstance.color = c;

    }
    private void ApplyFeedbackToCursor(bool validity)
    {
        if (cellIndicatorRenderer == null) return;

        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;

        if (cellIndicatorRenderer.sharedMaterial != null)
        {
            cellIndicatorRenderer.sharedMaterial.color = c;
        }

    }
    private void MoveCursor(Vector3 position)
    {

        lastCursorPosition = position;
        Vector3 adjustedPosition = new Vector3(position.x, previewYOffset, position.z);

        cellIndicator.transform.position = adjustedPosition;
   
    }
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x,
            position.y + previewYOffset,
            position.z);
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}