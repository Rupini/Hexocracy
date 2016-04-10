using Hexocracy.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hexocracy.Core
{
    public class Player
    {
        #region Static

        public static implicit operator int(Player player)
        {
            return player.Index;
        }


        private static Dictionary<int, Player> players;
        public static Player NeutralPassive { get; private set; }
        public static Player NeutralAggressive { get; private set; }

        public static List<Player> GetAll
        {
            get
            {
                return players.Values.ToList();
            }
        }

        public static Player GetByIndex(int index)
        {
            if (players.ContainsKey(index))
            {
                return players[index];
            }
            else
            {
                return null;
            }
        }

        public static void Initialize(List<PlayerData> playersData)
        {
            players = new Dictionary<int, Player>();

            NeutralPassive = new Player() { Index = -1, Name = "Neutral Passive" };
            NeutralAggressive = new Player { Index = -2, Name = "Neutral Aggressive" };

            foreach (var data in playersData)
            {
                if (data.index < 0 || players.ContainsKey(data.index))
                {
                    throw new Exception("Bad player index: " + data.index + " Index must be greater then 0 and Index must be unique!");
                }

                players[data.index] = new Player()
                {
                    Index = data.index,
                    Name = data.playerName,
                    TeamColor = data.color
                };
            }

            var dataMap = playersData.ToDictionary(p => p.index);

            foreach (var player in players.Values)
            {
                foreach (var enemy in dataMap[player.Index].enemies)
                {
                    player.enemies[enemy] = players[enemy];
                }

                foreach (var ally in dataMap[player.Index].allies)
                {
                    player.allies[ally] = players[ally];
                }

                player.allies[NeutralPassive.Index] = NeutralPassive;
                player.enemies[NeutralAggressive.Index] = NeutralAggressive;
            }
        }

        #endregion

        #region Instance

        private Dictionary<int, Player> enemies;
        private Dictionary<int, Player> allies;

        public int Index { get; private set; }

        public string Name { get; private set; }

        public Color TeamColor { get; private set; }

        private Player()
        {
            enemies = new Dictionary<int, Player>();
            allies = new Dictionary<int, Player>();
        }

        public bool IsEnemy(Player player)
        {
            return enemies.ContainsKey(player.Index);
        }

        public bool IsAlly(Player player)
        {
            return player == this || allies.ContainsKey(player.Index);
        }

        public override bool Equals(object obj)
        {
            return (obj is Player) && ((Player)obj).Index == Index;
        }

        public override int GetHashCode()
        {
            return Index;
        }

        public override string ToString()
        {
            return Index + " - " + Name;
        }
        #endregion
    }
}
