using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

using Unity.VisualScripting;
using System;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Texture2D mouseCursor;

    [SerializeField] AudioData gameMusic;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerStart;

    //[SerializeField] GameObject[] pickups;

    [SerializeField] private float gameTimerMax = 60;

    [Header("Events")]
    [SerializeField] EventRouter startGameEvent;
    [SerializeField] EventRouter stopGameEvent;
    [SerializeField] EventRouter winGameEvent;

    GameObject cplayer;

    DateTime startTime = DateTime.Now;
    //DateTime endTime;
    private float score;
    [SerializeField] private int enemyKills;
    [SerializeField] private int totalEnemies;
    public enum State
    { 
        TITLE,
        START_GAME,
        PLAY_GAME,
        GAME_OVER,
        GAME_WON
    }

    State state = State.TITLE;
    float stateTimer = 0;
    private void Start()
    {
        if (mouseCursor != null) Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);
        foreach (var enemies in FindObjectsOfType<AICharacter2D>())
        {
            totalEnemies++;
        }
    }

	private void Update()
	{
        if (Input.GetMouseButtonDown(1)) SetGameOver();
        //float timeSpeed = FindObjectOfType<TimeManager>().timeSpeed;
        var music = GetComponent<AudioSource>();

        //if (enemyKills >= totalEnemies)
        //{
        //    SetGameWon();
        //    enemyKills = 0;
        //}
        //Debug.Log(state);
        switch (state)
        {
            case State.TITLE:
                //Debug.Log("Title");
                music.Stop();
                //gameMusic.Play(transform);
                //if (!gameMusicPlayer.isPlaying) gameMusicPlayer.Play();
                //var cam = FindObjectOfType<RollerCamera>();
                //cam.GetComponent<RollerCamera>().ResetView();
                UIManager.Instance.ShowTitle(true);
                UIManager.Instance.ShowGameOver(false);
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
                //foreach (var pickup in pickups)
                //{
                //	//pickup.gameObject.SetActive(true);
                //}
                var player = FindObjectOfType<ControllerCharacter2D>();
                if (player != null) Destroy(player.gameObject);
                //score = 0;
				break;
            case State.START_GAME:
                //Debug.Log("Start Game II");
                UIManager.Instance.SetScore((int)score);
                if (music != null)
                {
                    //music.clip = gameMusic.audioClips[0];
                    music.Play();
                }
                //startGameEvent.Notify();
				//gameTimer = gameTimerMax;
                //gameMusicPlayer.Stop();
                //gameMusicPlayer.clip = gameMusic;
                //gameMusicPlayer.Play();
                UIManager.Instance.ShowTitle(false);
                //Cursor.lockState = CursorLockMode.Locked;
                //if (FindObjectOfType<CharacterPlayer>() == null) Instantiate(playerPrefab, playerStart.position, playerStart.rotation);
                if (FindObjectOfType<ControllerCharacter2D>() == null)
                {
                    cplayer = Instantiate(playerPrefab, playerStart.position, playerStart.rotation);
                }
                player = FindObjectOfType<ControllerCharacter2D>();
                
                //Respawnable[] respawnables = FindObjectsOfType<Respawnable>();
                //foreach (Respawnable respawnable in respawnables)
                //{
                //    Debug.Log("Respawn");
                //    respawnable.Respawn();
                //}
                //player.Enable();
                //var enemies = FindObjectsOfType<AICharacter2D>();
                //foreach (var enemy in enemies)
                //{
                //    Destroy(enemy.gameObject);
                //}
                if (player != null)
                {
                    //FindObjectOfType<CinemachineFreeLook>().LookAt = player.transform;
                    //FindObjectOfType<CinemachineFreeLook>().Follow = player.transform;
                }
				//UIManager.Instance.SetScore(0);
				//UIManager.Instance.SetHealth(100);
                state = State.PLAY_GAME;
                break;
            case State.PLAY_GAME:
                //Debug.Log("Play Game");
                //Debug.Log("Play Game");
                //gameTimer -= Time.deltaTime * timeSpeed;
                //UIManager.Instance.SetTimer(gameTimer, gameTimerMax);
                //if (gameTimer <= 0)
                //{
                //    var playerHP = FindObjectOfType<Health>();
                //    playerHP.OnApplyDamage(1000);
                //}
                //gameTimer += Time.deltaTime;
                break;
            case State.GAME_OVER:
                //Debug.Log("Game Over");
                if (music != null)
                {
                    music.Stop();
                }
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    UIManager.Instance.ShowGameOver(false);
                    state = State.TITLE;
                }
                break;
            case State.GAME_WON:
                //Debug.Log("Game Won");
                player = FindObjectOfType<ControllerCharacter2D>();
                if (player != null) Destroy(player.gameObject);
                //UIManager.Instance.ShowGameWin(true);
                //if (music != null)
                //{
                //    music.Stop();
                //    music.clip = gameMusic.audioClips[1];
                //    music.Play();
                //}
                stateTimer -= Time.deltaTime;
				if (stateTimer <= 0)
				{
					UIManager.Instance.ShowGameWin(false);
					state = State.TITLE;
				}
				break;
		}
        //Debug.Log(state);
        if (cplayer != null)
        {
            //Debug.Log(cplayer.transform.position);
        }
	}

	private void FixedUpdate()
	{
		//var player = FindObjectOfType<RollerPlayer>();
  //      if (player != null) 
  //      { 
  //          transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.2f);
  //      }
	}

	
    //public void AddTime(float time)
    //{
    //    gameTimer+= time;
    //    gameTimer = Mathf.Clamp(gameTimer, 0, gameTimerMax);
    //}

    public void SetGameOver()
    {
		stopGameEvent.Notify();
		//gameMusic.Stop();
		UIManager.Instance.ShowGameOver(true);
        state = State.GAME_OVER;
        stateTimer = 3;
        //Debug.Log("gameover");
    }
	public void SetGameWon()
	{
		//gameMusicPlayer.Stop();
		UIManager.Instance.ShowGameWin(true);
        //UIManager.Instance.SetTimer(DateTime.Now - startTime);
		state = State.GAME_WON;
		stateTimer = 5;
	}
	public void OnStartGame()
    {
        //FindObjectOfType<ArenaManager>().ResetArena();
        //var respawns = FindObjectsOfType<Respawnable>();
        //foreach (var respawn in respawns)
        //{
        //    respawn.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        //}
        startGameEvent.Notify();
        //Debug.Log("Start game");
        state = State.START_GAME;
    }
    public void AddScore(int score)
    {
        this.score += score;
        UIManager.Instance.SetScore((int)this.score);
    }
    public void AddKill()
    {
        enemyKills++;
    }
}
