using UnityEngine;
using Game;
using System;
using System.Collections;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL;
    private long _sessionID;
    private string _numTimeSwitches;
    private string _mostRecentCheckpoint;
    private string _timePassedSinceGameStart;
    private string _currentLevel;
    private string _eventType;



    public void Send(string eventType)
    {
        _sessionID = GlobalData._sessionID;
        _mostRecentCheckpoint = GlobalData.CheckPointData.GetLastCheckPointPosition().ToString();
        _numTimeSwitches = GlobalData.numberOfTimeSwitches.ToString();
        _currentLevel = GlobalData.LevelData.GetCurrentLevel().ToString();
        _timePassedSinceGameStart = TimeSpan.FromTicks(DateTime.Now.Ticks - GlobalData._sessionID).TotalMinutes.ToString();
        _eventType = eventType;


        StartCoroutine(Post(_sessionID.ToString(), _numTimeSwitches,_mostRecentCheckpoint, _timePassedSinceGameStart, _currentLevel, _eventType));

    }

    private IEnumerator Post(string sessionID, string numTimeSwitches, string mostRecentCheckpoint, string timePassedSinceGameStart, string currentLevel, string eventType)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1399250158", sessionID);
        form.AddField("entry.1012006095", numTimeSwitches);
        form.AddField("entry.1612456413", mostRecentCheckpoint);
        form.AddField("entry.1169508124", timePassedSinceGameStart);
        form.AddField("entry.869404102", currentLevel);
        form.AddField("entry.17129386", eventType);


        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
