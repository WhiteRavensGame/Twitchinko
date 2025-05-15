using System;
using TwitchIntegration;
using UnityEngine;
using TMPro;

public class GameManager : TwitchMonoBehaviour
{
    [SerializeField] private GameObject _testObject;
    [SerializeField] private GameObject _foodObject;

    [SerializeField] private TextMeshProUGUI _bestTimeText;
    [SerializeField] private TextMeshProUGUI _currentTimeText;
    private float bestTime = 0;

    private Vector3 _targetPosition;

    //private void Awake()
    //{
    //    //if (Instance == null) Instance = this;
    //    //else Destroy(this.gameObject);
    //}

    private void Start()
    {
        bestTime = PlayerPrefs.GetFloat("best_time", 0);
        UpdateBestTimeUI();
    }

    [TwitchCommand("move_object", "move", "m")]
    public void MovePillpet(float x, float y)
    {
        Pillpet p = _testObject.GetComponent<Pillpet>();
        p.Move(x, -4.5f);
    }

    [TwitchCommand("drop_food", "food")]
    public void DropFood()
    {
        float spawnPoint = UnityEngine.Random.Range(-8f, 8f);
        _foodObject.transform.position = new Vector3(spawnPoint, 0, 0);
        _foodObject.SetActive(true);
    }

    [TwitchCommand("revive", "revive")]
    public void Revive()
    {
        Pillpet p = _testObject.GetComponent<Pillpet>();
        p.Revive();
    }

    public void RecordTime(float timeAlive)
    {
        if(timeAlive > bestTime)
        {
            bestTime = timeAlive;
            UpdateBestTimeUI();
            PlayerPrefs.SetFloat("best_time", bestTime);
        }
       
    }


    private static string FloatToTimeFormat(float totalSeconds)
    {
        int hours = Mathf.FloorToInt(totalSeconds / 3600);
        int minutes = Mathf.FloorToInt((totalSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);

        return string.Format("{0}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

    public void UpdateTimeUI(float time)
    {
        _currentTimeText.text = "Time: " + FloatToTimeFormat(time);
    }

    private void UpdateBestTimeUI()
    {
        _bestTimeText.text = "Best Time: " + FloatToTimeFormat(bestTime);
    }

    // Update is called once per frame
    void Update()
    {
        //_testObject.transform.position = Vector3.Lerp(_testObject.transform.position, _targetPosition, Time.deltaTime * 3);
    }
}
