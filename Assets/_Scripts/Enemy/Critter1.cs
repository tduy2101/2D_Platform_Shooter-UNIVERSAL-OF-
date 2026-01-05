using UnityEngine;

public class Critter1 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    private float moveSpeed;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private float moveTimer;
    private float moveInterval;

    private ObjectPooler burnEffectPool;
    private ObjectPooler zappedEffectPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        zappedEffectPool = GameObject.Find("Critter1_ZappedPool").GetComponent<ObjectPooler>();
        burnEffectPool = GameObject.Find("Critter1_BurnPool").GetComponent<ObjectPooler>();

        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        moveSpeed = Random.Range(0.5f, 3f);
        GenerateRandomPosition();
        moveInterval = Random.Range(1f, 2f);
        moveTimer = moveInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTimer > 0) {
            moveTimer -= Time.deltaTime;
        }
        else {
            GenerateRandomPosition();
            moveInterval = Random.Range(0.3f, 2f);
            moveTimer = moveInterval;
        }

        targetPosition -= new Vector3(GameManager.Instance.worldSpeed * Time.deltaTime, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 relativePos = targetPosition - transform.position;
        if (relativePos != Vector3.zero) {
            targetRotation = Quaternion.LookRotation(Vector3.forward, relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1080 * Time.deltaTime);
        }
    }
    private void GenerateRandomPosition()
    {
        float randomX = Random.Range(-5f, 5f);
        float randomY = Random.Range(-5f, 5f);
        targetPosition = new Vector2(randomX, randomY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //Instantiate(zappedEffect, transform.position, transform.rotation);
            GameObject zappedEffect = zappedEffectPool.GetPooledObject();
            zappedEffect.transform.position = transform.position;
            zappedEffect.transform.rotation = transform.rotation;
            zappedEffect.SetActive(true);
            gameObject.SetActive(false);
            GameManager.Instance.critterCounter += 1;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.Burn);
            //Instantiate(burnEffect, transform.position, transform.rotation);
            GameObject burnEffect = burnEffectPool.GetPooledObject();
            burnEffect.transform.position = transform.position;
            burnEffect.transform.rotation = transform.rotation;
            burnEffect.SetActive(true);
            gameObject.SetActive(false);
            GameManager.Instance.critterCounter += 1;
        }
    }
}
