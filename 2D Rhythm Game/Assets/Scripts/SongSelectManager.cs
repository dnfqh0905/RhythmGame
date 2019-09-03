using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

public class SongSelectManager : MonoBehaviour
{
    public Text startUI;
    public Text disableAlertUI;
    public Image disablePanelUI;
    public Button purchaseButtonUI;
    private bool disabled = true;

    public Image musicImageUI;
    public Text musicTitleUI;
    public Text bpmUI;

    private int musicIndex;
    private int musicCount = 3;

    // 회원가입 결과 UI
    public Text userUI;

    private void UpdateSong(int musicIndex)
    {
        // 곡을 바꾸면 플레이 할 수 없게 막음
        disabled = true;
        disablePanelUI.gameObject.SetActive(true);
        disableAlertUI.text = "Loading...";
        purchaseButtonUI.gameObject.SetActive(false);
        startUI.gameObject.SetActive(false);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        // 리소스에서 비트 텍스트파일 호출
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + musicIndex.ToString());
        StringReader stringReader = new StringReader(textAsset.text);
        // 첫번째 줄에 적힌 곡 이름을 읽어서 UI 업데이트
        musicTitleUI.text = stringReader.ReadLine();
        // 두번째 줄은 읽기만
        stringReader.ReadLine();
        // 셋째 줄 BPM 읽어서 UI 업데이트
        bpmUI.text = "BPM : " + stringReader.ReadLine().Split(' ')[0];
        // 리소스에서 비트 음악파일 호출 후 재생
        AudioClip audioClip = Resources.Load<AudioClip>("Beats/" + musicIndex.ToString());
        audioSource.clip = audioClip;
        audioSource.Play();
        // 리소스에서 비트 이미지 파일 호출
        musicImageUI.sprite = Resources.Load<Sprite>("Beats/" + musicIndex.ToString());
        // 파이어베이스 접근
        DatabaseReference reference;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://unity-rhythm-game-tutori-1f7bf.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.GetReference("charges").Child(musicIndex.ToString());
        // 데이터 셋의 모든 데이터를 JSON 형태로 가져옴
        reference.GetValueAsync().ContinueWith(task =>
        {
            // 데이터 가져오기 성공
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // 무료곡
                if (snapshot == null || !snapshot.Exists)
                {
                    disabled = false;
                    disablePanelUI.gameObject.SetActive(false);
                    disableAlertUI.text = "";
                    startUI.gameObject.SetActive(true);
                }
                else
                {
                    // 구매 이력이 있는 경우 플레이 가능
                    if (snapshot.Child(PlayerInformation.auth.CurrentUser.UserId).Exists)
                    {
                        disabled = false;
                        disablePanelUI.gameObject.SetActive(false);
                        disableAlertUI.text = "";
                        startUI.gameObject.SetActive(true);
                        purchaseButtonUI.gameObject.SetActive(false);
                    }
                    // 구매 확인
                    if (disabled)
                    {
                        purchaseButtonUI.gameObject.SetActive(true);
                        disableAlertUI.text = "플레이 할 수 없는 곡입니다.";
                        startUI.gameObject.SetActive(false);
                    }
                }
            }

        });
    }

    // 구매 정보 담는 charge 클래스
    class Charge
    {
        public double timestamp;
        public Charge(double timestmap)
        {
            this.timestamp = timestamp;
        }
    }

    public void Purchase()
    {
        // 데이터베이스 접속 설정
        DatabaseReference reference = PlayerInformation.GetDatabaseReference();
        // 삽입할 데이터 준비
        DateTime now = DateTime.Now.ToLocalTime();
        TimeSpan span = (now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
        int timestamp = (int)span.TotalSeconds;
        Charge charge = new Charge(timestamp);
        string json = JsonUtility.ToJson(charge);
        // 랭킹 점수 데이터 삽입
        reference.Child("charges").Child(musicIndex.ToString()).Child(PlayerInformation.auth.CurrentUser.UserId).SetRawJsonValueAsync(json);
        UpdateSong(musicIndex);
    }

    public void Right()
    {
        musicIndex = musicIndex + 1;
        if (musicIndex > musicCount) musicIndex = 1;
        UpdateSong(musicIndex);
    }

    public void Left()
    {
        musicIndex = musicIndex - 1;
        if (musicIndex < 1) musicIndex = musicCount;
        UpdateSong(musicIndex);
    }

    void Start()
    {
        userUI.text = PlayerInformation.auth.CurrentUser.Email + " 님 환영합니다!";
        musicIndex = 1;
        UpdateSong(musicIndex);
    }

    public void GameStart()
    {
        if (disabled) return;
        PlayerInformation.selectedMusic = musicIndex.ToString();
        SceneManager.LoadScene("GameScene");
    }

    public void LogOut()
    {
        PlayerInformation.auth.SignOut();
        SceneManager.LoadScene("LoginScene");
    }
}