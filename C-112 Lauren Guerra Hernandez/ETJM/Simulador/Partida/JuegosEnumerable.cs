using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;


namespace Simulador
{
    public class JuegosEnumerables : IEnumerable<Juego>
    {
        private List<Player> players;
        private IGame<Player> juego;
        private Dictionary<Player, int> puntuaciones;
        private Dictionary<Team, int> puntuacionesEquipos;
        private bool porEquipos;
        private List<Team> equipos;
        private bool permiteEmpate;

        public JuegosEnumerables(List<Player> players, IGame<Player> juego, Dictionary<Player, int> puntuaciones, bool permiteEmpate = true)
        {
            porEquipos = false;
            equipos = null;
            this.players = players;
            this.juego = juego;
            this.puntuaciones = puntuaciones;
            this.permiteEmpate = permiteEmpate;

        }
        public JuegosEnumerables(List<Team> equipos, IGame<Player> juego, Dictionary<Team, int> puntuaciones, bool permiteEmpate = true)
        {
            this.equipos = equipos;
            this.players = null;
            this.juego = juego;
            this.puntuacionesEquipos = puntuaciones;        
            this.permiteEmpate = permiteEmpate;

        }
        public IEnumerator<Juego> GetEnumerator()
        {
            if (porEquipos) return new JuegosEnumerator(equipos, juego, puntuacionesEquipos,permiteEmpate);
            return new JuegosEnumerator(players, juego, puntuaciones,permiteEmpate);
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

      
    }
}
