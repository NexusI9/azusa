using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DayState
{
    SUNRISE,
    DAY,
    SUNSET,
    NIGHT
}

public class DayMoment
{
    public DayState state;
    public float startPercent;
    public float transitionTime;
}

public class DayTime
{
    public float hours;
    public float minutes;
    public float seconds;
    public float length;
}


public class EnvironmentManager : MonoBehaviour
{
    private static EnvironmentManager _instance;
    public static EnvironmentManager Instance
    {
        get
        {
            return _instance;
        }
    }

    //Time
    [SerializeField]
    public bool _runTime = true;
    public float _dayTotalTime = 24f; //length of a day in minute
    public float _speedFactor = 1f;
    private DayTime time = new DayTime { hours = 0f, minutes = 0f, seconds=0f };

    //Cycle
    [Space]
    private DayMoment[] dayMoments = new DayMoment[]
    {
        new DayMoment{ state = DayState.SUNRISE, startPercent = 15f, transitionTime = 20f },
        new DayMoment{ state = DayState.DAY, startPercent = 20f, transitionTime = 10f },
        new DayMoment{ state = DayState.SUNSET, startPercent = 60f, transitionTime = 20f},
        new DayMoment{ state = DayState.NIGHT, startPercent = 70f, transitionTime = 10f }
    };
   
    private int currentMomentIndex = 0;
    protected static DayState currentDayState;

    private void updateTime()
    {

        //Update Global Hour an Minutes
        time.seconds += Time.deltaTime * _speedFactor;
        if(time.seconds >= time.length)
        {
            time.seconds = 0;
        }

        time.hours = Mathf.FloorToInt(time.seconds / 60f);
        time.minutes = Mathf.FloorToInt(time.seconds % 60f);

        //Debug.Log(time.hours + ":" + time.minutes);

        //update day moment depending on current time
        for(int d = 0; d < dayMoments.Length; d++) {
            DayMoment currentDayMoment = dayMoments[d];
            if (time.hours >= time.length * currentDayMoment.startPercent/100 && currentMomentIndex != d)
            {
                currentMomentIndex = d;
                updateMoment(d, currentDayMoment);
            }
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    protected virtual void updateMoment(int index, DayMoment moment)
    {
        Debug.Log(moment);
    }


    void Start()
    {
   
        //Start Cycle
        time.length = _dayTotalTime * 60f;
    }

    void Update()
    {
        if (_runTime)
        {
            updateTime();
        }
    }
}
