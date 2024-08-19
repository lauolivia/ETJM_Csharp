using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public class JugadasEnumerable : IEnumerable<Jugadas>
    {
        private IGame<Player> juego;
        private List<Player> jugadores;
        private Dictionary<Player, int> puntuaciones;
        private Dictionary<Team, int> puntuacionesEquipos;
        private bool porEquipos;
        private List<Team> equipos;
        public JugadasEnumerable(IGame<Player> juego, List<Player> jugadores, Dictionary<Player, int> puntuaciones)
        {
            this.juego = juego;
            this.jugadores = jugadores;
            this.puntuaciones = puntuaciones;
            porEquipos = false;
        }
        public JugadasEnumerable(IGame<Player> juego, List<Team> equipos, Dictionary<Team, int> puntuacionesEquipos)
        {
            this.juego = juego;
            this.equipos = equipos;
            this.puntuacionesEquipos = puntuacionesEquipos;
            porEquipos = true;
        }
        public IEnumerator<Jugadas> GetEnumerator()
        {
            if(porEquipos) return new JugadasEnumerator(juego, equipos, puntuacionesEquipos);

            return new JugadasEnumerator(juego, jugadores,puntuaciones);
        }
        

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
