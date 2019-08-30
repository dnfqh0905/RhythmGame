using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 싱글톤 처리
    public static GameManager instance { get; set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public float noteSpeed;

    public GameObject scoreUI;
    private float score;
    private Text scoreText;

    public GameObject comboUI;
    private int combo;
    private Text comboText;
    private Animator comboAnimator;

    /*
     * BAD : 1
     * GOOD : 2
     * PERFECT : 3
     * MISS : 4
     */

    public enum judges { NONE = 0, BAD, GOOD, PERFECT, MISS };
    public GameObject judgeUI;
    private Sprite[] judgeSprites;
    private Image judgementSpriteRenderer;
    private Animator judgementSpriteAnimator;

    public GameObject[] trails;
    private SpriteRenderer[] trailSpriteRenderers;

    // 음악 변수
    private AudioSource audioSource;
    public string music = "1";

    // 음악 실행 함수
    void MusicStart()
    {
        // 리로스에서 비트 음악 파일 재생
        AudioClip audioClip = Resources.Load<AudioClip>("Beats/" + music);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    void Start()
    {
        Invoke("MusicStart", 2);

        judgementSpriteRenderer = judgeUI.GetComponent<Image>();
        judgementSpriteAnimator = judgeUI.GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<Text>();
        comboText = comboUI.GetComponent<Text>();
        comboAnimator = comboUI.GetComponent<Animator>();

        // 판정 결과를 보여주는 이미지 미리 초기화
        judgeSprites = new Sprite[4];
        judgeSprites[0] = Resources.Load<Sprite>("Sprites/Bad");
        judgeSprites[1] = Resources.Load<Sprite>("Sprites/Good");
        judgeSprites[2] = Resources.Load<Sprite>("Sprites/Miss");
        judgeSprites[3] = Resources.Load<Sprite>("Sprites/Perfect");

        trailSpriteRenderers = new SpriteRenderer[trails.Length];
        for (int i = 0; i < trails.Length; i++)
        {
            trailSpriteRenderers[i] = trails[i].GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        // 사용자가 입력한 키 라인을 빛나게 처리
        if (Input.GetKey(KeyCode.D)) ShineTrail(0);
        if (Input.GetKey(KeyCode.F)) ShineTrail(1);
        if (Input.GetKey(KeyCode.J)) ShineTrail(2);
        if (Input.GetKey(KeyCode.K)) ShineTrail(3);

        // 한 번 빛난 라인은 반복적으로 다시 어둡게 처리
        for (int i = 0; i < trailSpriteRenderers.Length; i++)
        {
            Color color = trailSpriteRenderers[i].color;
            color.a -= 0.01f;
            trailSpriteRenderers[i].color = color;

        }
    }

    // 특정한 키를 눌러 해당 라인을 빛나게 처리
    public void ShineTrail(int index)
    {
        Color color = trailSpriteRenderers[index].color;
        color.a = 0.32f;
        trailSpriteRenderers[index].color = color;
    }

    // 노트 판정 이후 판정 결과를 화면에 노출
    void showJudgement()
    {
        // 점수이미지
        string scoreFormat = "000000";
        scoreText.text = score.ToString(scoreFormat);
        // 판정이미지
        judgementSpriteAnimator.SetTrigger("Show");
        // 콤보가 2이상일때 콤보 이미지 노출
        if (combo >= 2)
        {
            comboText.text = combo.ToString() + " COMBO!";
            comboAnimator.SetTrigger("Show");
        }
    }

    // 노트 판정
    public void processJudge(judges judge, int notType)
    {
        if (judge == judges.NONE) return;

        // MISS인 경우 콤보 종료, 감점
        if (judge == judges.MISS)
        {
            judgementSpriteRenderer.sprite = judgeSprites[2];
            combo = 0;
            if (score >= 15) score -= 15;
        }

        // BAD인 경우 콤보 종료, 소량 감점
        else if (judge == judges.BAD)
        {
            judgementSpriteRenderer.sprite = judgeSprites[0];
            combo = 0;
            if (score >= 15) score -= 5;
        }

        // 이외엔 콤보 및 점수 증가
        else
        {
            if (judge == judges.PERFECT)
            {
                judgementSpriteRenderer.sprite = judgeSprites[3];
                score += 20;
            }
            else if (judge == judges.GOOD)
            {
                judgementSpriteRenderer.sprite = judgeSprites[1];
                score += 10;
            }
            combo += 1;
            score += (float)combo * 0.1f;
        }
        showJudgement();
    }
}