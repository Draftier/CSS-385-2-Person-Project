using UnityEngine;
using System.Collections;

public class FrenchFryCreator : MonoBehaviour
{
    [Header("Sprite Settings")]
    public Sprite frenchFrySprite;
    public string fallbackSpritePath = "Assets/french_fry";

    [Header("Enemy Configuration")]
    public GameObject projectilePrefab;
    public Transform planet;
    public Transform player;
    public float moveSpeed = 1.5f;
    public float detectionRange = 7f;
    public float fireRate = 2.5f;
    public float projectileSpeed = 8f;
    public Vector2 spawnPosition = Vector2.zero;

    void Start()
    {
        CreateFrenchFryEnemy();
    }

    void CreateFrenchFryEnemy()
    {
        GameObject fry = new GameObject("FrenchFryEnemy");
        fry.transform.position = spawnPosition;
        fry.tag = "Enemy";
        
        SpriteRenderer renderer = fry.AddComponent<SpriteRenderer>();
        TryAssignSprite(renderer);
        
        Rigidbody2D rb = fry.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        
        BoxCollider2D collider = fry.AddComponent<BoxCollider2D>();
        
        FrenchFryEnemy enemy = fry.AddComponent<FrenchFryEnemy>();
        ConfigureEnemy(enemy);
        
        if (renderer.sprite != null) {
            collider.size = renderer.sprite.bounds.size;
        }
    }

    void TryAssignSprite(SpriteRenderer renderer)
    {
        if (frenchFrySprite != null) {
            renderer.sprite = frenchFrySprite;
            return;
        }
        
        if (!string.IsNullOrEmpty(fallbackSpritePath)) {
            Sprite loadedSprite = Resources.Load<Sprite>(fallbackSpritePath);
            if (loadedSprite != null) {
                renderer.sprite = loadedSprite;
                return;
            }
        }
        
        Debug.LogError("French Fry sprite assignment failed. Using default placeholder.");
        renderer.color = Color.yellow;
        renderer.sprite = Sprite.Create(
            new Texture2D(16, 64), 
            new Rect(0, 0, 16, 64), 
            Vector2.one * 0.5f
        );
    }

    void ConfigureEnemy(FrenchFryEnemy enemy)
    {
        enemy.planet = planet ? planet : GameObject.FindWithTag("Planet").transform;
        enemy.player = player ? player : GameObject.FindWithTag("Player").transform;
        enemy.projectilePrefab = projectilePrefab;
        enemy.moveSpeed = moveSpeed;
        enemy.detectionRange = detectionRange;
        enemy.fireRate = fireRate;
        enemy.projectileSpeed = projectileSpeed;
    }
}