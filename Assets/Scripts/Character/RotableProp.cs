using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotableProp : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private bool idleRotate;
    [SerializeField]
    private float innerRotationSpeed = 180;

    private float innerRotation = 0;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetSpriteByRotation(0);
    }

    private void LateUpdate()
    {
        if (idleRotate)
        {
            innerRotation += Time.deltaTime*innerRotationSpeed;
            SetSpriteByRotation(innerRotation);
        }
    }

    public void SetIdleRotate(bool rotate)
    {
        idleRotate = rotate;
    }

    public void SetSpriteByRotation(float rotation)
    {
        rotation = rotation % 360;
        if (rotation < 0) rotation += 360;

        int index = Mathf.RoundToInt(rotation / 45) % sprites.Length;

        if (index >= 0 && index < sprites.Length)
        {
            spriteRenderer.sprite = sprites[index];
        }
        else
        {
            Debug.LogWarning("Sprite index out of bounds!");
        }
    }
}
