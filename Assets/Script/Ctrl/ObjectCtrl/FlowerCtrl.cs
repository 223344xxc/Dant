using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCtrl : MonoBehaviour
{
    [SerializeField] private Sprite activeFlower;
    [SerializeField] private Sprite unActiveFlower;
    private SpriteRenderer spriteRenderer;

    private bool isFlower = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteRenderer.sprite = unActiveFlower;
            PlayerCtrl.Instance.flowerCount += isFlower ? 1 : 0;
            isFlower = false;
        }
    }
}
