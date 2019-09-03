using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class GameResultManager : MonoBehaviour
{
    public Text musicTitleUI;
    public Text scoreUI;
    public Text maxComboUI;
    public Image RankUI;

    public Text rank1UI;
    public Text rank2UI;
    public Text rank3UI;

    void Start()
    {
        musicTitleUI.text = PlayerInformation.musicTitle;
        scoreUI.text = "점수 : " + (int)PlayerInformation.score;
        maxComboUI.text = "최대 콤보 : " + PlayerInformation.maxCombo;
        // 리소스에서 비트 텍스트파일 호출
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + PlayerInformation.selectedMusic);
        StringReader reader = new StringReader(textAsset.text);
        // 첫째줄과 둘째줄 무시
        reader.ReadLine();
        reader.ReadLine();
        // 셋째 줄 비트 정보(랭크 점수)
        string beatInformation = reader.ReadLine();
        int scoreS = Convert.ToInt32(beatInformation.Split(' ')[3]);
        int scoreA = Convert.ToInt32(beatInformation.Split(' ')[4]);
        int scoreB = Convert.ToInt32(beatInformation.Split(' ')[5]);
        // 성적에 맞는 랭크 이미지 호출
        if (PlayerInformation.score >= scoreS)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank S");
        }
        else if (PlayerInformation.score >= scoreA)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank A");
        }
        else if (PlayerInformation.score >= scoreB)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank B");
        }
        else
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank C");
        }
        rank1UI.text = "데이터를 불러오는 중입니다.";
        rank2UI.text = "데이터를 불러오는 중입니다.";
        rank3UI.text = "데이터를 불러오는 중입니다.";
        DatabaseReference reference = PlayerInformation.GetDatabaseReference().Child("ranks").Child(PlayerInformation.selectedMusic);
        // 데이터 셋의 모든 데이터를 JSON형태로 가져옴
        reference.OrderByChild("score").GetValueAsync().ContinueWith(task =>
        {
            // 데이터 가져오기 성공
            if (task.IsCompleted)
            {
                List<string> rankList = new List<string>();
                List<string> emailList = new List<string>();
                DataSnapshot snapshot = task.Result;
                // JSON 데이터 각 원소에 접근
                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary rank = (IDictionary)data.Value;
                    emailList.Add(rank["email"].ToString());
                    rankList.Add(rank["score"].ToString());
                }
                // 정렬 이후 내림차순 정렬
                emailList.Reverse();
                rankList.Reverse();
                // Top3 출력
                rank1UI.text = "기록이 없습니다.";
                rank2UI.text = "기록이 없습니다.";
                rank3UI.text = "기록이 없습니다.";
                List<Text> textList = new List<Text>();
                textList.Add(rank1UI);
                textList.Add(rank2UI);
                textList.Add(rank3UI);
                int count = 1;
                for (int i = 0; i < rankList.Count && i < 3; i++)
                {
                    textList[i].text = count + "위 : " + emailList[i] + " (" + rankList[i] + " 점";
                    count = count + 1;
                }
            }
        });
    }

    public void Replay()
    {
        SceneManager.LoadScene("SongSelectScene");
    }
}