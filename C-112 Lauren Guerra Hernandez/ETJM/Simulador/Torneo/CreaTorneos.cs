using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public class CreaTorneos
    {
        private Dictionary<string, Torneo> crea = new Dictionary<string, Torneo>();
        public Torneo this[string x] { get { return crea[x]; } }
        public IEnumerable<string> Keys { get { return crea.Keys; } }
        public IEnumerable<Torneo> Values { get { return crea.Values; } }
        public int Count { get { return crea.Count; } }
        public CreaTorneos(List<Player> jugadores,IGame<Player> juego)
        {
            crea.Add("Torneo por el titulo", new TorneoPorTitulo(jugadores, juego));
            crea.Add("Torneo dos a dos", new TorneoDosADos(jugadores, juego));
            if(jugadores.Count % juego.MaxCantidadPlayers == 0) crea.Add("Torneo individual", new TorneoIndividual(jugadores, juego));

        }
        public CreaTorneos(List<Team> equipos, IGame<Player> juego)
        {
            if(juego.PermiteEquipos)crea.Add("Torneo por el titulo", new TorneoPorTitulo(equipos, juego));
            crea.Add("Torneo dos a dos", new TorneoDosADos(equipos, juego));

        }
    }
}
