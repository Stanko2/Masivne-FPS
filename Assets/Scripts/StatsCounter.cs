using System;
using System.Collections.Generic;
using Photon.Pun;
using UI;
using UnityEngine;

public class StatsCounter : MonoBehaviourPunCallbacks
{
    public static event Action OnStatsUpdated;
    public static StatsCounter Instance;
    public Dictionary<int, PlayerStats> stats;


    private void Start()
    {
        Instance = this;
        stats = new Dictionary<int, PlayerStats>();
    }

    public void RegisterPlayer(int playerId, string playerName)
    {
        if (stats.ContainsKey(playerId))
            return;
        stats[playerId] = new PlayerStats
        {
            Kills = 0,
            Deaths = 0,
            Name = playerName,
            Order = stats.Count,
            PlayerId = playerId
        };
        OnStatsUpdated?.Invoke();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        stats.Remove(otherPlayer.ActorNumber);
    }

    public void OnKill(int killer, int dead)
    {
        var playerStats = stats[killer];
        playerStats.Kills++;
        stats[killer] = playerStats;
        var stat = stats[dead];
        stat.Deaths++;
        stats[dead] = stat;
        OnStatsUpdated?.Invoke();
    }
}