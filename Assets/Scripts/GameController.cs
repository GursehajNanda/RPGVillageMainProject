using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.VFX;

public class GameController : MonoBehaviour
{
    [SerializeField] TMP_Text m_simulationButtonText;
    [SerializeField] TMP_Text m_timeText;
    [SerializeField] private TMP_Dropdown m_weatherDropDown;
    [SerializeField] private ParticleSystem m_rain;
    [SerializeField] private VisualEffect m_fog;

    private int m_startingSpeeed;
    ClimateData m_climateData;
  

    private void Awake()
    {
        m_climateData = ClimateData.Instance;  
    }

    private void Start()
    {
        m_startingSpeeed = 1;
        m_climateData.SetMonth(Month.June);
        ClearWeather();
    }

    void OnEnable()
    {
     
        m_weatherDropDown.onValueChanged.AddListener(OnWeatherDropdownChanged);
    }

    void OnDisable()
    {
       m_weatherDropDown.onValueChanged.RemoveListener(OnWeatherDropdownChanged);
    }


    private void Update()
    {
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

  

    private void OnWeatherDropdownChanged(int selectedIndex)
    {
        if(selectedIndex == 0)
        {
            ClearWeather();
        }
        else if( selectedIndex == 1)
        {
            AddRain();
        }
        else
        {
            AddFog();
        }
    }

    private void ClearWeather()
    {
        m_climateData.RemoveRunningWeather(WeatherType.Rainy);
        m_climateData.RemoveRunningWeather(WeatherType.Cloudy);
        m_climateData.RemoveRunningWeather(WeatherType.Foggy);
        m_climateData.AddRunningWeather(WeatherType.Clear, WeatherBehaviour.None);
        m_climateData.SetCloudStrength(0.0f);
        m_rain.Stop();
        m_fog.Stop();
    }


    private void AddClouds()
    {
        m_climateData.SetCloudStrength(0.8f);
        m_climateData.AddRunningWeather(WeatherType.Cloudy, WeatherBehaviour.Heavy);
    }

    private void AddRain()
    {
        AddClouds();
        m_climateData.RemoveRunningWeather(WeatherType.Clear);
        m_climateData.AddRunningWeather(WeatherType.Rainy, WeatherBehaviour.Heavy);
        m_rain.Play();
    }


    private void AddFog()
    {
        m_fog.Play();
        m_climateData.RemoveRunningWeather(WeatherType.Clear);
        m_climateData.AddRunningWeather(WeatherType.Foggy, WeatherBehaviour.Heavy);
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


    private void OnApplicationQuit()
    {
        m_climateData.UpdateMinutesToLastTheDay(24);
    }
}
