using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Player;
using UnityEngine;

namespace UI
{
    public class PlayerTable : MonoBehaviourPun
    {
        public Transform contentParent;
        public GameObject playerPanel;
        public GameObject rowPrefab;
        
        private void Start()
        {
            StatsCounter.OnStatsUpdated += UpdateTable;
        }

        private void Update()
        {
            playerPanel.SetActive(Input.GetKey(KeyCode.Tab));
        }

        private void UpdateTable()
        {
            for (int i = 1; i < contentParent.childCount; i++)
            {
                Destroy(contentParent.GetChild(i).gameObject);
            }

            var playerStatsList = StatsCounter.Instance.stats.Values.ToList();
            playerStatsList.Sort();
            for (int i = 0; i < playerStatsList.Count; i++)
            {
                var player = playerStatsList[i];
                player.Order = i + 1;
                var row = Instantiate(rowPrefab, contentParent).GetComponent<PlayerTableRow>();
                row.SetStats(player);
            }
        }
    }
}