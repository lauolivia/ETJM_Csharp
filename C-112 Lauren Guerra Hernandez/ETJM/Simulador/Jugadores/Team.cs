
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public class Team
    {
        public List<Player> Players { get; }
        public string Name { get; }
        public Team(string name, List<Player> players = null)
        {
            if (players == null) Players = new List<Player>();
            else Players = players;
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
        public void Play(int jugador, IGame<Player> juego)
        {
            Players[jugador].Play(juego);
        }
    }

}
