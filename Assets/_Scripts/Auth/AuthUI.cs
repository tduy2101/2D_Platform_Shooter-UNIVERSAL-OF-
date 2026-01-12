using UnityEngine;
using TMPro; // Dự án bạn dùng TextMeshPro
using UnityEngine.SceneManagement;

public class AuthUI : MonoBehaviour
{
    [Header("Login/Register Form")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText; 

    public void OnClick_Login()
    {
        string u = usernameInput.text;
        string p = passwordInput.text;

        if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
        {
            messageText.text = "Vui lòng nhập đủ thông tin!";
            return;
        }

        if (LocalDataManager.Instance.Login(u, p))
        {
            messageText.text = "Đăng nhập thành công!";
            // Load vào MainMenu của game
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            messageText.text = "Sai tên hoặc mật khẩu!";
        }
    }

    public void OnClick_Register()
    {
        string u = usernameInput.text;
        string p = passwordInput.text;

        if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p)) return;

        if (LocalDataManager.Instance.Register(u, p))
        {
            messageText.text = "Đăng ký thành công! Hãy Login.";
        }
        else
        {
            messageText.text = "Tên tài khoản đã tồn tại!";
        }
    }
}