using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splatter : MonoBehaviour
{
    public List<Sprite> sprites;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
    }
}
