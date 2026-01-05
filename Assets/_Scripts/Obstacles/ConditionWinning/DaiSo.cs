using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DaiSo : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level_4_Completed");
        }
    }
}
