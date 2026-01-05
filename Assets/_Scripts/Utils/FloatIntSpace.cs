using UnityEngine;

public class FloatIntSpace : MonoBehaviour
{
    private void Update()
    {
        float moveX = GameManager.Instance.adjustedWorldSpeed;
        transform.position += new Vector3(-moveX, 0);
        if (transform.position.x < -11f)
        {
            gameObject.SetActive(false);
        }
    }
}
