using UnityEngine;


public class GoalObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.WinLevel();
            }
            gameObject.SetActive(false);
        }
    }
}