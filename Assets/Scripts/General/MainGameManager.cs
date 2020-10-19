using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1500)]
public class MainGameManager : MonoBehaviour
{
    //The current level.
    public static Transform Tower { get; private set; }
    public static Transform Player { get; private set; }
    public static readonly string towerTag = "Tower";
    public static readonly string playerTag = "Player";

    [System.NonSerialized]
    public float startingPoint;

    //Reference to the scene's current game manager.
    public static MainGameManager Main { get; private set; }

    public TimeComponent timeComponent;
    public ScoreComponent scoreComponent;

    void Awake()
    {
        //We can't have more than one manager in a scene.
        if (Main != null)
        {
            Debug.LogError($"Only one manager is allowed to exist in the scene at once. Destroying object '{gameObject.name}'...");
            Destroy(gameObject);
            return;
        }

        Tower = GameObject.FindGameObjectWithTag(towerTag).transform;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform;
        Main = this;

        startingPoint = Player.transform.position.y;

        timeComponent.Initialize();
        scoreComponent.Initialize(this);
    }

    private void Update()
    {
        timeComponent.TimerUpdate(Time.deltaTime);
        scoreComponent.ScoreUpdate();
    }

    //Sub-class that handles time-related features/mechanics.
    [Serializable]
    public class TimeComponent
    {
        [Min(0), Tooltip("The number of seconds the level starts with.")]
        public int startTime = 500;

        [Min(0), Tooltip("How many seconds it takes for the timer to tick down one second.")]
        public float secondInterval = 1.0f;

        public bool paused = false;
        public Text counterText;

        public Action timeIsUp;
        public Action<int> secondTick;

        private float timer;
        public float Timer
        {
            get => timer;

            private set
            {
                if (value <= 0)
                {
                    if (timer > 0)
                        timeIsUp?.Invoke();

                    value = 0;
                }

                timer = value;
                secondTick?.Invoke((int)Mathf.Ceil(timer));
            }
        }

        
        
        public void Initialize()
        {
            Timer = startTime;
        }

        //The timer basically just ticks down every frame.
        public void TimerUpdate(float deltaTime)
        {
            if (!paused)
                Timer -= deltaTime;
        }
    }

    [System.Serializable]
    public class ScoreComponent
    {
        private int score;
        //Score cannot go below zero, and notifies subscribers if it's been
        //changed.
        public int Score
        {
            get => score;

            private set
            {
                if (value < 0)
                {
                    value = 0;
                }

                score = value;
                scoreHasUpdated(score);
            }
        }

        [Tooltip("Score increases every time the player's height peaks. ")]
        public float scoreMultiplier = 10;

        public float perSecondMultiplier = 0.001f;

        public float TotalSecondMultiplier => 1 + perSecondMultiplier * _main.timeComponent.Timer;

        private float _highestPointReached;

        private MainGameManager _main;

        public void Initialize(MainGameManager main)
        {
            _main = main;
            _highestPointReached = _main.startingPoint;

        }

        public void ScoreUpdate()
        {
            if (Player.position.y > _highestPointReached)
            {
                Score += (int)((Player.position.y - _highestPointReached) * scoreMultiplier * TotalSecondMultiplier);
                _highestPointReached = Player.position.y;
            }
        }

        public void ResetScore()
        {
            Score = 0;
        }

        public Action<int> scoreHasUpdated;
    }

}
