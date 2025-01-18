using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeReward : MonoBehaviour
{
    public UnityEvent OnRewardClaimed;

    [SerializeField] private int _timeUntilReward;
    [SerializeField] private TextMeshProUGUI _textTime;
    [SerializeField] private ButtonBase _button;

    private Coroutine _updateTimeProcessCoroutine;

    private float _timeLeft;
    private string _lastRewardTimeStr;
    private bool _canTakeReward = true;

    private readonly string SaveName = "DailyReward";
    private readonly WaitForSeconds Interval = new(1f);

    public bool CanTakeReward
    {
        get => _canTakeReward;
        set
        {
            _canTakeReward = value;
            _button.Interactable = value;

            if(value) _textTime.text = "GET";
            else ActivateTimer();
        }
    }

    private void OnEnable() => CanTakeReward = GetSecondsUntilReward() == 0;
    private void OnDisable() => ReleaseCoroutine();

    private void ActivateTimer()
    {
        ReleaseCoroutine();

        _updateTimeProcessCoroutine = StartCoroutine(UpdateTimeProcess());
    }

    private IEnumerator UpdateTimeProcess()
    {
        while (!CanTakeReward)
        {
            _timeLeft = GetSecondsUntilReward();

            if(_timeLeft == 0) CanTakeReward = true;
            else _textTime.text = TextUtility.FormatTime(_timeLeft);

            yield return Interval;
        }
    }

    private void ReleaseCoroutine()
    {
        if (_updateTimeProcessCoroutine != null)
        {
            StopCoroutine(_updateTimeProcessCoroutine);
            _updateTimeProcessCoroutine = null;
        }
    }

    private float GetSecondsUntilReward()
    {
        _lastRewardTimeStr = PlayerPrefs.GetString(SaveName, string.Empty);

        if (DateTime.TryParse(_lastRewardTimeStr, out DateTime lastRewardTime))
        {
            DateTime currentTime = DateTime.UtcNow;
            TimeSpan timeSinceLastReward = currentTime - lastRewardTime;
            float secondsPassed = (float)timeSinceLastReward.TotalSeconds;
            float secondsUntilReward = _timeUntilReward - secondsPassed;

            return secondsUntilReward > 0 ? secondsUntilReward : 0;
        }

        return 0;
    }

    public void TakeReward()
    {
        if (CanTakeReward)
        {

            PlayerPrefs.SetString(SaveName, DateTime.UtcNow.ToString());
            OnRewardClaimed?.Invoke();
            CanTakeReward = false;
        }
    }
}