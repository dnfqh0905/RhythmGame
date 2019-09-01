using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JoinManager : MonoBehaviour
{
    // 파이어베이스 인증 기능 객체
    private FirebaseAuth auth;

    // 이메일 및 패스워드 UI
    public InputField emailInputField;
    public InputField passwordInputField;

    //회원가입 결과 UI
    public Text messageUI;

    void Start()
    {
        // 파이어베이스 인증 객체 초기화
        auth = FirebaseAuth.DefaultInstance;
        messageUI.text = "";
    }

    bool InputCheck()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;
        if (email.Length < 8)
        {
            messageUI.text = "8자 이상 이메일 주소를 입력해주세요.";
            return false;
        }
        else if (password.Length < 8)
        {
            messageUI.text = "8자 이상 비밀번호를 입력해주세요.";
            return false;
        }
        messageUI.text = "";
        return true;
    }

    public void Check()
    {
        InputCheck();
    }

    public void Join()
    {
        if (!InputCheck())
        {
            return;
        }

        string email = emailInputField.text;
        string password = passwordInputField.text;

        // 인증 객체를 이용해 이메일과 비밀번호로 회원가입
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
            task =>
            {
                if (!task.IsCanceled && !task.IsFaulted)
                {
                    SceneManager.LoadScene("LoginScene");
                }
                else
                {
                    messageUI.text = "회원가입에 실패했습니다.";
                }
            }
        );
    }

    public void Back()
    {
        SceneManager.LoadScene("LoginScene");
    }
}