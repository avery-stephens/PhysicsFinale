using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] private float spawnMinTime;
	[SerializeField] private float spawnMaxTime;
	[SerializeField] private bool enableOnAwake = true;
	[SerializeField] private GameObject spawnPrefab;
	public bool spawnEnabled { get; set; }
	public bool spawnByButton = false;
    [SerializeField] KeyCode spawnKey;
    private float spawnTimer;

	protected void Start()
	{
		spawnTimer = Random.Range(spawnMinTime, spawnMaxTime);
		spawnEnabled = enableOnAwake;
	}

	void Update()
	{
		if (!spawnByButton)
		{
			if (!spawnEnabled) return;

			spawnTimer -= Time.deltaTime;
			if (spawnTimer < 0)
			{
				spawnTimer = Random.Range(spawnMinTime, spawnMaxTime);
				Spawn();
			}
		}
		else
		{
			if (Input.GetKeyDown(spawnKey)) Spawn();
        }
	}
	public void Spawn()
	{
		Instantiate(spawnPrefab);
	}
}