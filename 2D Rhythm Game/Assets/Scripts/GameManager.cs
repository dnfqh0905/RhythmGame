using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /*
     * BAD : 1
     * GOOD : 2
     * PERFECT : 3
     * MISS : 4
     */

    public enum judges { NONE = 0, BAD, GOOD, PERFECT, MISS };

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}