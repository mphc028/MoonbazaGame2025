using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    private Vector3 lookAtPosition;

    [SerializeField]
    private Texture cursorTexture, pointerTexture;
    private Texture currentPointer;


    private RotableProp rot;

    private void Start()
    {
        Cursor.visible = false;
        rot = transform.GetComponentInParent<RotableProp>();
        SetCursor(1);
    }



    void Update()
    {
        Cursor.visible = false;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z)
        );

        Vector3 direction = worldPos - transform.position;
        lookAtPosition = mousePos;

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
        rot.SetSpriteByRotation(angle);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnGUI()
    {
        int scaleValue = (int)(Screen.width / 640);

        float cursorX = ((int)((lookAtPosition.x - 8 * scaleValue) / scaleValue)) * scaleValue;
        float cursorY = ((int)((lookAtPosition.y - 8 * scaleValue) / scaleValue)) * scaleValue;

        GUI.DrawTexture(
            new Rect(cursorX, Screen.height - cursorY, (Screen.width * 16) / 640, -(Screen.width * 16) / 640),
            currentPointer,
            ScaleMode.ScaleAndCrop,
            true,
            1
        );
    }

    public void SetCursor(int index)
    {
        currentPointer = index == 1 ? cursorTexture : pointerTexture;
    }
}
