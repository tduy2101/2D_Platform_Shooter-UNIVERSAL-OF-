using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class DashboardUI : MonoBehaviour
{
    public Transform contentTransform;
    public GameObject rowPrefab;

    void OnEnable()
    {
        // Khi bảng vừa bật lên, gọi hàm hiển thị ngay
        ShowLeaderboard();
    }

    public void ShowLeaderboard()
    {
        // [DEBUG] Kiểm tra xem Manager có tồn tại không
        if (LocalDataManager.Instance == null)
        {
            Debug.LogError("LỖI: Không tìm thấy LocalDataManager");
            return;
        }

        // [QUAN TRỌNG] Gọi hàm này để chắc chắn load file save mới nhất từ ổ cứng lên
        LocalDataManager.Instance.LoadData();

        Debug.Log("Bắt đầu hiển thị bảng xếp hạng...");

        // 1. Xóa danh sách cũ (Clear list)
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        // 2. Lấy danh sách
        List<UserData> sortedList = LocalDataManager.Instance.gameData.allUsers
            .OrderByDescending(x => x.highestLevel)
            .ToList();

        Debug.Log("Tìm thấy số người chơi: " + sortedList.Count);

        // 3. Hiển thị
        int rank = 1;
        foreach (var user in sortedList)
        {
            if (rank > 10) break;

            GameObject newRow = Instantiate(rowPrefab, contentTransform);
            TMP_Text[] texts = newRow.GetComponentsInChildren<TMP_Text>();

            // Kiểm tra xem Prefab có đủ 3 cái Text không
            if (texts.Length >= 3)
            {
                texts[0].text = "#" + rank;
                texts[1].text = user.username;
                texts[2].text = "Lv." + user.highestLevel;
            }
            else
            {
                Debug.LogWarning("Prefab RowTemplate bị thiếu Text Component!");
            }
            rank++;
        }
    }
}