using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Rigidbody2D rb;
    private Animator animator;
    private FlashWhite flashWhite;  

    [SerializeField] private Vector2 playerDirection;
    [SerializeField] private float moveSpeed;


    public bool boosting = false;

    [SerializeField] private float energy;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyRegen;

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    private ObjectPooler destroyEffectPool;
    [SerializeField] private ParticleSystem engineEffect;

    [SerializeField] private int experience;
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private List<int> playerLevels;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        destroyEffectPool = GameObject.Find("Boom1Pool").GetComponent<ObjectPooler>();

        flashWhite = GetComponent<FlashWhite>();

        for (int i = playerLevels.Count; i < maxLevel; i++)
        {
            playerLevels.Add(Mathf.CeilToInt(playerLevels[playerLevels.Count - 1] * 1.1f + 15));
        }

        energy = maxEnergy;
        UiController.Instance.UpdateEnergy(energy, maxEnergy);
        health = maxHealth;
        UiController.Instance.UpdateHealth(health, maxHealth);
        experience = 0;
        UiController.Instance.UpdateExperience(experience, playerLevels[currentLevel]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            float directionX = Input.GetAxis("Horizontal");
            float directionY = Input.GetAxis("Vertical");
            animator.SetFloat("moveX", directionX);
            animator.SetFloat("moveY", directionY);

            playerDirection = new Vector2(directionX, directionY).normalized;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))
            {
                EnterBoost();
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2"))
            {
                ExitBoost();
            }

            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetButtonDown("Fire1"))
            {
                PhaserWeapon.Instance.Shoot();
            }
        }
    }
    private void FixedUpdate() 
    {
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed,playerDirection.y *moveSpeed);

        if (boosting)
        {
            if (energy >= 0.5f)
            {
                energy -= 0.5f;
            }
            else
            {
                ExitBoost();
            }
        }
        else
        {
            if (energy < maxEnergy)
            {
                energy += energyRegen;
            }
        }

        if (UiController.Instance != null)
        {
            UiController.Instance.UpdateEnergy(energy, maxEnergy);
            UiController.Instance.UpdateHealth(health, maxHealth);
        }

    }

    public void EnterBoost()
    {
        if ( energy > 10)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.Fire);
            animator.SetBool("boosting", true);
            GameManager.Instance.SetWorldSpeed(7f);
            boosting = true;
            engineEffect.Play();
        }
    }

    public void ExitBoost()
    {
        animator.SetBool("boosting", false);
        GameManager.Instance.SetWorldSpeed(1f);
        boosting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid) asteroid.TakeDamage(1, false);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy) enemy.TakeDamage(1);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        AudioManager.Instance.PlaySound(AudioManager.Instance.Hit);
        flashWhite.Flash();
        if (UiController.Instance != null)
        {
            UiController.Instance.UpdateHealth(health, maxHealth);
        }; 

        if (health <= 0)
        {
            GameManager.Instance.SetWorldSpeed(0f);
            gameObject.SetActive(false);
            //Instantiate(destroyEffect, transform.position, transform.rotation);
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            destroyEffect.SetActive(true);

            GameManager.Instance.GameOver();
            AudioManager.Instance.PlaySound(AudioManager.Instance.Ice);
        }
    }
   
    public void GainExperience(int exp)
    {
        experience += exp;
        if (UiController.Instance != null)
        {
            UiController.Instance.UpdateExperience(experience, playerLevels[currentLevel]);
        };
        if (experience >= playerLevels[currentLevel])
        {
            LevelUp();
        }

    }

    public void LevelUp()
    {
        if (currentLevel < maxLevel - 1)
        {
            experience -= playerLevels[currentLevel];
            currentLevel++;
            UiController.Instance.UpdateExperience(experience, playerLevels[currentLevel]);
            PhaserWeapon.Instance.LevelUp();
            maxHealth++;
            health = maxHealth;
        }
    }
}
