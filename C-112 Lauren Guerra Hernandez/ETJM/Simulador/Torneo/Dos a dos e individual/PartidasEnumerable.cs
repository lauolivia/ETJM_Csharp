using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    class PartidasEnumerable : IEnumerable<Partida>
    {
        List<Player> players;
        IGame<Player> juego;
        List<Partida> partidas;
        Dictionary<Player, int> puntuaciones;
        Dictionary<Team, int> puntuacionesEquipos;
        bool porEquipos;
        List<Team> equipos;
        public PartidasEnumerable(List<Player> players ,IGame<Player> juego, List<Partida> partidas, Dictionary<Player, int> puntuaciones)
        {
            porEquipos = false;
            equipos = null;
            this.players = players;
            this.juego = juego;
            this.partidas = partidas;
            this.puntuaciones = puntuaciones;
        }
        public PartidasEnumerable(List<Team> equipos, IGame<Player> juego, List<Partida> partidas, Dictionary<Team, int> puntuaciones)
        {
            this.equipos = equipos;
            this.players = null;
            this.juego = juego;
            this.partidas = partidas;
            this.puntuacionesEquipos = puntuaciones;
        }
        public IEnumerator<Partida> GetEnumerator()
        {
            if (porEquipos) return new PartidaEnumerator(equipos,juego,partidas, puntuacionesEquipos);
            return  new PartidaEnumerator(players, juego, partidas, puntuaciones);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

       


        
    }
}
