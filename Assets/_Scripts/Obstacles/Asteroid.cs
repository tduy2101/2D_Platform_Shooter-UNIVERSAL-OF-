using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private FlashWhite flashWhite;

    private ObjectPooler destroyEffectPool;
    
    private int lives;
    [SerializeField] public int maxlives;
    [SerializeField] public int damage;
    [SerializeField] public int experienceToGive;
    float pushX;
    float pushY;

    [SerializeField] private Sprite[] sprites;

    private void OnEnable()
    {
        lives = maxlives;
        transform.rotation = Quaternion.identity;
        pushX = Random.Range(-1f, 0);
        pushY = Random.Range(-1f, 1f);
        if(rb) rb.linearVelocity = new Vector2(pushX, pushY);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        rb = GetComponent<Rigidbody2D>();
        flashWhite = GetComponent<FlashWhite>();
        destroyEffectPool = GameObject.Find("Boom2Pool").GetComponent<ObjectPooler>();

        pushX = Random.Range(-1f, 0);
        pushY = Random.Range(-1f, 1f);
        if (rb) rb.linearVelocity = new Vector2(pushX, pushY);

        float randomScale = Random.Range(0.6f, 1f);
        transform.localScale = new Vector2(randomScale, randomScale);

        lives = maxlives;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { 
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player) player.TakeDamage(damage);
        }
    }
    public void TakeDamage(int damage, bool giveExperiance)
    {
        lives -= damage;
        if (lives > 0)
        {
            flashWhite.Flash();
        }else {
            AudioManager.Instance.PlaySound(AudioManager.Instance.Squished);
            //Instantiate(destroyEffect, transform.position, transform.rotation);
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            destroyEffect.transform.localScale = transform.localScale;
            destroyEffect.SetActive(true);
            //Destroy(gameObject);
            flashWhite.Reset();
            gameObject.SetActive(false);
            if (giveExperiance) PlayerController.Instance.GainExperience(experienceToGive);
        }
    }

}
