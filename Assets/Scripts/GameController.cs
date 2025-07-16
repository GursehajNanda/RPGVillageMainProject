using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [SerializeField] TMP_Text m_simulationButtonText;
    [SerializeField] TMP_Text m_timeText;
    [SerializeField] private TMP_Dropdown m_weekDaysDropDown;

    private int m_startingSpeeed;
    private int m_previousDay = 4;
    private int m_currentDay = 4;
    ClimateData m_climateData;
    Timer m_timer = new Timer(1.0f, 0.0f, null, null);

    private static readonly Dictionary<int, int> WeekdayToGameDay = new()
    {
        { 0, 10 }, // Sunday
        { 1, 4 },  // Monday
        { 2, 5 },
        { 3, 6 },
        { 4, 7 },
        { 5, 8 },
        { 6, 9 }   // Saturday
    };

    private static readonly Dictionary<int, int> GameDayToWeekday = new()
    {
        { 10, 0 }, // Sunday
        { 4, 1 },  // Monday
        { 5, 2 },
        { 6, 3 },
        { 7, 4 },
        { 8, 5 },
        { 9, 6 }   // Saturday
    };

    private void Awake()
    {
        m_climateData = ClimateData.Instance;  
    }
    private void Start()
    {
        m_startingSpeeed = 1;
        m_previousDay = m_currentDay;
    }

    void OnEnable()
    {
       m_weekDaysDropDown.onValueChanged.AddListener(OnDropdownChanged);
    }

    void OnDisable()
    {
       m_weekDaysDropDown.onValueChanged.RemoveListener(OnDropdownChanged);
    }


    private void Update()
    {

        m_currentDay = m_climateData.GetDateTimeYearData().Day;

        if (m_previousDay != m_currentDay)
        {
            m_previousDay = m_currentDay;

          
            if (GameDayToWeekday.TryGetValue(m_currentDay, out int weekdayIndex))
            {
                m_weekDaysDropDown.SetValueWithoutNotify(weekdayIndex);
                m_weekDaysDropDown.RefreshShownValue();
            }
        }

        UpateTime();
    }

    private void LateUpdate() 
    {
        int currentDay = m_climateData.GetDateTimeYearData().Day;

        if (currentDay > 10)
        {
            m_climateData.SetDayInMonth(4);
        }
        else if (currentDay < 4)
        {
            m_climateData.SetDayInMonth(10);
        }
    }

    private void OnDropdownChanged(int selectedIndex)
    {
        if (WeekdayToGameDay.TryGetValue(selectedIndex, out int newDay))
        {
            m_climateData.SetDayInMonth(newDay);
        }
    }


    public void ClearWeather()
    {
        m_climateData.ResetWeatherEffects();

    }

    public void AddPartiallyClouds()
    {
        ClearWeather();
        m_climateData.ForceWeatherEffects(WeatherType.Cloudy, WeatherBehaviour.Normal);
    }

    public void AddClouds()
    {
        ClearWeather();
        m_climateData.ForceWeatherEffects(WeatherType.Cloudy, WeatherBehaviour.Heavy);
    }

    public void AddRain()
    {
        AddClouds();
        m_timer = new Timer(1.0f, 2.0f, null, AddRainDelay);
        m_timer.Start();
    }

    public void AddWinds()
    {
        ClearWeather();
        m_climateData.ForceWeatherEffects(WeatherType.Windy, WeatherBehaviour.Moderate);
    }

    public void AddFog()
    {
        ClearWeather();
        m_climateData.ForceWeatherEffects(WeatherType.Foggy, WeatherBehaviour.Heavy);
    }

    public void AddThunder()
    {
        ClearWeather();
        AddClouds();
        m_timer = new Timer(1.0f, 2.0f, null, AddThunderDelay);
        m_timer.Start();
    }

    public void UpdateSimulationSpeed()
    {
        m_startingSpeeed = m_startingSpeeed % 4 + 1;
        m_simulationButtonText.text = m_startingSpeeed.ToString() + "X";
        int newSpeed = 24 / m_startingSpeeed;
        m_climateData.UpdateMinutesToLastTheDay(newSpeed);
    }

    public void UpateTime()
    {
        string hour = m_climateData.GetDateTimeYearData().Hour.ToString();
        string min = m_climateData.GetDateTimeYearData().Minute.ToString();
        m_timeText.text = hour + ":" + min;
    }
    private void AddRainDelay()
    {
        m_climateData.ForceWeatherEffects(WeatherType.Rainy, WeatherBehaviour.Heavy);
    }

    private void AddThunderDelay()
    {
        m_climateData.ForceWeatherEffects(WeatherType.Thunder, WeatherBehaviour.Moderate);
    }


    private void OnApplicationQuit()
    {
        m_climateData.UpdateMinutesToLastTheDay(24);
    }
}
