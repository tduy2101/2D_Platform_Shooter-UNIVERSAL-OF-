using UnityEngine;

public class Boss1 : MonoBehaviour
{
    private Animator animator;
    private float speedX;
    private float speedY;
    private bool charging;

    private float switchInterval;
    private float switchTimer;

    private int lives;
    private int maxLives = 500;
    private int damage = 20;
    private int experienceReward = 20;

    private ObjectPooler destroyEffectPool;

    void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        lives = maxLives;
        EnterChargeState();
        AudioManager.Instance.PlaySound(AudioManager.Instance.bossSpawn);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destroyEffectPool = GameObject.Find("Boom3Pool").GetComponent<ObjectPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerPosition = PlayerController.Instance.transform.position.x;
        if (switchTimer > 0)
        {
            switchTimer -= Time.deltaTime;
        }
        else
        {
            if (charging && transform.position.x > playerPosition)
            {
                EnterPatrolState();
            }
            else
            {
                EnterChargeState();
            }
        }

        if (transform.position.y > 3 || transform.position.y < -3)
        {
            speedY *= -1;
        } else if (transform.position.x < playerPosition)
        {
            EnterChargeState();
        }

        bool boost = PlayerController.Instance.boosting;
        float moveX;
        if (boost && !charging)
        {
            moveX = GameManager.Instance.worldSpeed * -0.5f * Time.deltaTime;
        } else
        {
            moveX = speedX * Time.deltaTime;
        }
        float moveY = speedY * Time.deltaTime;
        
        transform.position += new Vector3(moveX, moveY);
        if (transform.position.x < -11f)
        {
            gameObject.SetActive(false);
        }
    }

    void EnterPatrolState()
    {
        speedX = 0;
        speedY = Random.Range(-2f, 2f);
        switchInterval = Random.Range(5f, 10f);
        switchTimer = switchInterval;
        charging = false;
        animator.SetBool("charging", false);
    }

    void EnterChargeState()
    {
        if(!charging) { AudioManager.Instance.PlaySound(AudioManager.Instance.BossCharge); }
        speedX = -10f;
        speedY = 0f;
        switchInterval = Random.Range(0.6f, 1.3f);
        switchTimer = switchInterval;
        charging = true;
        animator.SetBool("charging", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid) asteroid.TakeDamage(damage, false);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player) player.TakeDamage(damage);
        }
    }
    public void TakeDamge(int damage)
    {
        lives -= damage;
        AudioManager.Instance.PlaySound(AudioManager.Instance.HitArmor);
        if(lives <= 0)
        {
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            destroyEffect.SetActive(true);

            gameObject.SetActive(false);
            PlayerController.Instance.GainExperience(experienceReward);
        }
    }

    
}

