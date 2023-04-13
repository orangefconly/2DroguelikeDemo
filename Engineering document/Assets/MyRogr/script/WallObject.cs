using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour
{
    public int hp = 3;
    public Sprite dmgWall;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void DamageWall(int _loss)
    {
        spriteRenderer.sprite = dmgWall;
        hp -= _loss;
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
