using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class LocalDataManager : MonoBehaviour
{
    public static LocalDataManager Instance;

    private string filePath;
    public GameData gameData;
    public UserData currentUser; // Người đang chơi hiện tại

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Đường dẫn file: C:/Users/[Tên]/AppData/LocalLow/[Company]/[Game]/savefile.json
            filePath = Path.Combine(Application.persistentDataPath, "savefile.json");
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            gameData = new GameData();
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(filePath, json);
    }

    // Trả về true nếu đăng ký thành công
    public bool Register(string user, string pass)
    {
        if (gameData.allUsers.Exists(x => x.username == user)) return false; // Tên đã tồn tại

        UserData newUser = new UserData { username = user, password = pass, highestLevel = 0 };
        gameData.allUsers.Add(newUser);
        SaveData();
        return true;
    }

    // Trả về true nếu đăng nhập đúng
    public bool Login(string user, string pass)
    {
        UserData found = gameData.allUsers.Find(x => x.username == user && x.password == pass);
        if (found != null)
        {
            currentUser = found;
            return true;
        }
        return false;
    }

    public void UpdateLevel(int level)
    {
        if (currentUser != null && level > currentUser.highestLevel)
        {
            currentUser.highestLevel = level;
            SaveData();
        }
    }
}