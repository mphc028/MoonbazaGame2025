using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class TilemapCursor : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject cursorPrefab;
    private GameObject cursorInstance;
    private Vector3 targetPosition;
    private Camera mainCamera;

    private void Start()
    {
        cursorInstance = Instantiate(cursorPrefab);
        targetPosition = cursorInstance.transform.position;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, -mainCamera.transform.position.z));
        mouseWorldPosition.z = 0;

        Vector3Int tilePosition = tilemap.WorldToCell(mouseWorldPosition);
        targetPosition = tilemap.GetCellCenterWorld(tilePosition);

        float distance = Vector3.Distance(cursorInstance.transform.position, targetPosition);
        float moveSpeed = Mathf.Clamp(distance * 50f, 10f, 1000f);

        cursorInstance.transform.position = Vector3.MoveTowards(cursorInstance.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
