using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public float minX = -25f;
    public float maxX = 25f;
    public float minY = -25f;
    public float maxY = 25f;

    private void Start()
    {
        var pos = new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
        
    }
}