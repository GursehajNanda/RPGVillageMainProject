using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuToGameTransition : MonoBehaviour
{

    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "Game";

    [Header("UI / Fade")]
    [SerializeField] private ScreenFader fader;
    [SerializeField] private float fadeOutDuration = 0.35f;
    [SerializeField] private float fadeInDuration = 0.25f;

    [Header("UI Input")]
    [SerializeField] private TMP_InputField hourInputField;
    [SerializeField] private TMP_Dropdown  weekDaysDropDown;

    private int m_day = 10;
    private int m_hour = 1;

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



    void OnEnable()
    {
        if (hourInputField) hourInputField.onValueChanged.AddListener(OnHourValueChanged);
        weekDaysDropDown.onValueChanged.AddListener(OnDropdownChanged);
    }

    void OnDisable()
    {
        if (hourInputField) hourInputField.onValueChanged.RemoveListener(OnHourValueChanged);
        weekDaysDropDown.onValueChanged.RemoveListener(OnDropdownChanged);
    }

    public void OnClickPlay()
    {
        if (isActiveAndEnabled) StartCoroutine(PlayFlow());
    }

    private IEnumerator PlayFlow()
    {

        if (fader) yield return fader.FadeOut(fadeOutDuration);

        var data = ClimateData.Instance;
        int engineHour = (m_hour == 24) ? 0 : m_hour; 
        data.SetDayInMonth(m_day);
        data.SetHourOfDay(engineHour);

        var op = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Single);
        while (!op.isDone) yield return null;
        yield return null; 

        if (fader) yield return fader.FadeIn(fadeInDuration);
    }

    private static void SetTMP(TMP_InputField field, string text)
    {
        if (!field) return;
        if (field.text != text)
        {
            field.SetTextWithoutNotify(text);
            field.caretPosition = text.Length;
            field.ForceLabelUpdate();
        }
    }

    public void OnHourValueChanged(string value)
    {
        if (!int.TryParse(value, out int hour)) { SetTMP(hourInputField, "1"); hour = 1; }
        hour = Mathf.Clamp(hour, 1, 24); // UI shows 1..24
        if (m_hour != hour) { m_hour = hour; SetTMP(hourInputField, hour.ToString()); }
    }

    private void OnDropdownChanged(int selectedIndex)
    {
        if (WeekdayToGameDay.TryGetValue(selectedIndex, out int newDay))
        {
            m_day = newDay;
        }
    }
}
