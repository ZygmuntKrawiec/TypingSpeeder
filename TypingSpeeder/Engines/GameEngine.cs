using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TypingSpeeder.Controllers;
using TypingSpeeder.Model;
using TypingSpeeder.Providers;

namespace TypingSpeeder.Engines
{
    class GameEngine
    {
     System.Timers.Timer timer;
    int stepIndex;
    public bool IsRunning { get; set; } = false;

    Mode gameMode = null;
    public event Action<string> NextSampleAdded;
    public event Action<double> NextStepMade;

    public GameEngine()
    {
        setTimer();
    }

    private void setTimer()
    {
        timer = new System.Timers.Timer();
        timer.Interval = 1000;
        timer.Elapsed += Timer_Elapsed;
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        Task.Run(() => NextStepMade(gameMode.Step));
        addNextSample();
    }

    private void addNextSample()
    {
        if (stepIndex == gameMode.Delay)
        {
            var text = DataProvider.GenerateRandomValue(gameMode.Level, gameMode.TextType, gameMode.MaxNumberRange);
            NextSampleAdded(text);
            stepIndex = 0;
            if (timer.Interval > gameMode.MinTime)
                timer.Interval -= gameMode.TimeDrop;
        }
        else
        {
            stepIndex++;
        }
    }

    public void SetGameMode(Level level)
    {
        var result = ModeController.GetModeValues(level);
        gameMode = result;
    }

    public void SetGameTextType(TextType textType)
    {
        if (gameMode == null)
        {
            gameMode = ModeController.GetModeValues(Level.Normal);
        }
        gameMode.TextType = textType;
    }

    public void Start()
    {
        if (gameMode == null)
        {
            SetGameMode(Level.Normal);
            SetGameTextType(TextType.Numbers);
        }

        stepIndex = gameMode.Delay;
        addNextSample();
        timer.Start();
        IsRunning = true;
    }

    public void Stop()
    {
        timer.Stop();
        IsRunning = false;
    }

    public bool ValidateValue(string answer, string value)
    {
        return value.Equals(answer);
    }
}
}
