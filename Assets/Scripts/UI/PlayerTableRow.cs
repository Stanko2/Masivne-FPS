using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public struct PlayerStats : IComparable<PlayerStats>
    {
        public int PlayerId;
        public string Name;
        public int Kills;
        public int Deaths;
        public int Order;
        
        public int CompareTo(PlayerStats objStats)
        {
            return objStats.Kills == Kills ? objStats.Deaths.CompareTo(Deaths) : objStats.Kills.CompareTo(Kills);
        }
    }
    public class PlayerTableRow : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text killText;
        [SerializeField] private Text deathText;
        [SerializeField] private Text orderText;

        public void SetStats(PlayerStats stats)
        {
            nameText.text = stats.Name;
            orderText.text = stats.Order.ToString();
            deathText.text = stats.Deaths.ToString();
            killText.text = stats.Kills.ToString();
        }
    }
}