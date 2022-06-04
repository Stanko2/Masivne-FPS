using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public static SpawnPlayers instance;
    public GameObject playerPrefab;
    public GameObject deathCanvas;
    public Text respawnText;
    public int respawnTime;
    public float minX = -25f;
    public float maxX = 25f;
    public float minY = -25f;
    public float maxY = 25f;

    private bool _localPlayerDead = false;
    private bool _canRespawn;
    private void Start()
    {
        Spawn();
        instance = this;
    }

    public void OnPlayerDied()
    {
        deathCanvas.SetActive(true);
        _localPlayerDead = true;
        _canRespawn = false;
        StartCoroutine(Respawn());
    }
    
    private void Update()
    {
        if (_localPlayerDead)
        {
            if(Input.GetKeyDown(KeyCode.Space) && _canRespawn)
                Spawn();                
        }
    }

    private IEnumerator Respawn()
    {
        _canRespawn = false;
        
        for (var i = 0; i < respawnTime; i++)
        {
            respawnText.text = $"Respawn in {respawnTime - i} seconds";
            yield return new WaitForSeconds(1);
        }

        _canRespawn = true;
        respawnText.text = "Press space to respawn";
    }

    private void Spawn()
    {
        _localPlayerDead = false;
        deathCanvas.SetActive(false);
        var pos = new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);

    }
}