using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class Player
    {
        #region Static
        private static List<Player> players = new List<Player>();
        public static Player NeutralPassive { get; private set; }
        public static Player NeutralAggressive { get; private set; }

        public static Player GetByIndex(int index)
        {
            if (index < players.Count)
                return players[index];
            else
                return null;
        }

        #endregion
        #region Builder

        public static class Builder
        {
            public static List<Player> Build(string[] names, int[][] enemies, int[][] allies)
            {
                NeutralPassive = new Player("Neutral Passive", -1);
                NeutralAggressive = new Player("Neutral Aggressive", -2);

                int n = 0;
                foreach (var name in names)
                {
                    players.Add(new Player(name, n));
                    n++;
                }

                int i = 0;

                foreach (var player in players)
                {
                    if (enemies != null)
                        for (int j = 0; j < enemies[i].Length; j++)
                            player.enemies.Add(enemies[i][j], players[enemies[i][j]]);

                    if (allies != null)
                        for (int j = 0; j < allies[i].Length; j++)
                            player.allies.Add(allies[i][j], players[allies[i][j]]);

                    player.enemies.Add(NeutralAggressive.Index, NeutralAggressive);
                    NeutralAggressive.enemies.Add(player.Index, player);

                    i++;
                }

                return players;
            }
        }

        #endregion
        public int Index { get; private set; }

        public string Name { get; private set; }

        private Dictionary<int, Player> enemies = new Dictionary<int, Player>();

        private Dictionary<int, Player> allies = new Dictionary<int, Player>();

        private Player(string name, int index)
        {
            Name = name;
            Index = index;
        }

        public bool IsEnemy(Player player)
        {
            return enemies.ContainsKey(player.Index);
        }

        public void SwitchActiveState(bool active)
        {
            if (active)
                InputController.I.ActivatePlayer(this);
            else
                InputController.I.DeactivatePlayer(this);
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

    }
}
