using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SongSelectManager : MonoBehaviour
{
    public Image musicImageUI;
    public Text musicTitleUI;
    public Text bpmUI;

    private int musicIndex;
    private int musicCount = 3;

    private void UpdateSong(int musicIndex)
    {
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
        musicIndex = 1;
        UpdateSong(musicIndex);
    }

    public void GameStart()
    {
        PlayerInformation.selectedMusic = musicIndex.ToString();
        SceneManager.LoadScene("GameScene");
    }
}