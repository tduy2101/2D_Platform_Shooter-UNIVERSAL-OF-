//using UnityEngine;

//public class ParallaxBackground : MonoBehaviour
//{
//    [SerializeField] private float moveSpeed;
//    float backgroundImageWidth;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created

//    void Start()
//    {
//        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
//        backgroundImageWidth = sprite.texture.width / sprite.pixelsPerUnit;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        float moveX = moveSpeed * GameManager.Instance.worldSpeed  * Time.deltaTime;
//        transform.position += new Vector3(moveX, 0);
//        if (Mathf.Abs(transform.position.x) - backgroundImageWidth >= 0)
//        {
//            transform.position = new Vector3(0f, transform.position.y);
//        }
//    }
//}
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    // Thêm biến này: Tốc độ trôi mặc định khi không có GameManager (VD: Ở Menu)
    [SerializeField] private float defaultScrollSpeed = 1f;

    private float backgroundImageWidth;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;
        backgroundImageWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    void Update()
    {
        float currentWorldSpeed = 0f;

        // --- ĐOẠN CODE SỬA LỖI ---

        // 1. Kiểm tra xem GameManager có tồn tại không?
        if (GameManager.Instance != null)
        {
            // Nếu ĐANG CHƠI GAME: Lấy tốc độ từ GameManager (để đồng bộ với tàu)
            currentWorldSpeed = GameManager.Instance.worldSpeed;
        }
        else
        {
            // Nếu ĐANG Ở MENU/AUTH: Dùng tốc độ mặc định tự quy định
            currentWorldSpeed = defaultScrollSpeed;
        }
        // -------------------------

        float moveX = moveSpeed * currentWorldSpeed * Time.deltaTime;

        transform.position += new Vector3(moveX, 0);

        if (Mathf.Abs(transform.position.x) - backgroundImageWidth >= 0)
        {
            float offset = (transform.position.x > 0) ? -backgroundImageWidth : backgroundImageWidth;
            transform.position += new Vector3(offset, 0, 0);
        }
    }
}