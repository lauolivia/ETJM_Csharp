using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;
namespace Simulador
{
    class TituloEnumerable : IEnumerable<Partida>
    {
        private IGame<Player> juego;
        private List<Player> partidas;
        private Dictionary<Player, int> puntuaciones;
        private Dictionary<Team, int> puntuacionesEquipos;
        private List<Team> partidasEquipos;
        private bool porEquipos;

        public TituloEnumerable(IGame<Player> Juego, List<Player> Partidas, Dictionary<Player, int> puntuaciones)
        {
            porEquipos = false;
               juego = Juego;
            partidas = Partidas;
            this.puntuaciones = puntuaciones;
        }
        public TituloEnumerable(IGame<Player> Juego, List<Team> Partidas, Dictionary<Team, int> puntuaciones)
        {
            porEquipos = true;
               juego = Juego;
            partidasEquipos = Partidas;
            this.puntuacionesEquipos = puntuaciones;
        }
        public IEnumerator<Partida> GetEnumerator()
        {
            if(!porEquipos)return new TituloEnumerator(juego,partidas,puntuaciones);
            return new TituloEnumerator(juego, partidasEquipos, puntuacionesEquipos);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

       


        
    }
}
