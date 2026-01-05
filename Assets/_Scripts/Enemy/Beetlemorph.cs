using NUnit.Compatibility;
using UnityEngine;

public class Beetlemorph : Enemy
{
    [SerializeField] private Sprite[] sprites;
    private float timer;
    private float frequency;
    private float amplitude;
    private float centerY;

    public override void OnEnable()
    {
        base.OnEnable();
        timer = transform.position.y;
        frequency = Random.Range(0.3f, 1f);
        amplitude = Random.Range(0.8f, 1.5f);
        centerY = transform.position.y;
    }

    public override void Start()
    {
        base.Start();
        int index = Random.Range(0, sprites.Length);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprites[index];
        }
        destroyEffectPool = GameObject.Find("Boom3Pool").GetComponent<ObjectPooler>();
    
        speedX = Random.Range(-0.8f, -1.5f);

    }

    public override void Update()
    {
        base.Update();

        timer -= Time.deltaTime;
        float sine = Mathf.Sin(timer * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, centerY + sine);
        
    }

}
