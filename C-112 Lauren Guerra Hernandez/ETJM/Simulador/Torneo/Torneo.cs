using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public abstract class Torneo
    {
        public abstract bool PermiteEmpate { get; }
        protected IEnumerable<Partida> PartidasEnumerable;
        protected IEnumerator<Partida> PartidasEnumerator;
        public bool TorneoComenzado { get; protected set; }
        public bool TorneoTerminado { get; protected set; }
        public Jugadas ResultadosTorneo { get { if (!TorneoTerminado) throw new InvalidOperationException("No se pueden pedir los resultados prque el torneo no ha acabado aun");return resultados; } }
        protected Jugadas resultados;
        protected Dictionary<Team, int> puntuacionesEquipos;
        public List<Player> Players { get; }
        public List<Team> Equipos { get; }
        public IGame<Player> Juego { get; }
        public Partida PartidaActual { get; private set; }
        public Dictionary<Player, int> Puntuaciones { get { return puntuaciones; } }
        protected Dictionary<Player, int> puntuaciones;
        public abstract bool TorneoPorEquipos { get; }
        public abstract bool EquipoContraEquipo { get; }
        public Torneo(List<Player> players, IGame<Player> juego)
        {
            Players = players;
            Juego = juego;
            puntuaciones = new Dictionary<Player, int>();
           
            Reset();

        }
        public Torneo(List<Team> equipos, IGame<Player> juego)
        {
            Equipos = equipos;
            Juego = juego;
            if (!EquipoContraEquipo) { Players = new List<Player>(); CreaJugadores(); }
            else Players = null;
            if (!EquipoContraEquipo) puntuaciones = new Dictionary<Player, int>();
            puntuacionesEquipos = new Dictionary<Team, int>();
            Reset();
        }
        
        public void Reset()
        {
            if (!EquipoContraEquipo) Tools.CreaPuntuaciones(puntuaciones, Players);
            //if (TorneoPorEquipos) Tools.CreaPuntuaciones(puntuacionesEquipos, Equipos);
            Comienzo = null;
            PartidaActual = null;
            TorneoComenzado = false;
            TorneoTerminado = false;
        }
        protected void CreaJugadores()
        {
            foreach (var equipo in Equipos)
            {
                foreach (var jugador in equipo.Players)
                {
                    Players.Add(jugador);
                }
            }
        }
        #region Comienzo
        public Jugadas Comienzo { get; protected set; }
        public virtual void IniciaTorneo()
        {
            string comienzo = $"Ha iniciado el {this.ToString()} del juego {Juego} entre ";
            if(!TorneoPorEquipos)comienzo+=$"los jugadores: {Tools.GetStringDeJugadores(Players)}";
            else comienzo += $"los equipos: {Tools.GetStringDeJugadores(Equipos)}";
            Comienzo = new Jugadas(TipoJugada.Torneo, comienzo);
            TorneoComenzado= true;
            PartidasEnumerator.MoveNext();
            PartidaActual = PartidasEnumerator.Current;
        }
        public abstract void OrganizaPartidas();

        #endregion


        public virtual void SiguientePartida()
        {
            if (TorneoTerminado) return;
            if (PartidasEnumerator.MoveNext())
            {
                PartidaActual = PartidasEnumerator.Current;
                return;
            }
            TerminoElTorneo();
        }

        #region Termina
        private void TerminoElTorneo()
        {
            TorneoTerminado = true;
            string resultadosString = ObtenResultados();
            resultados = new Jugadas(TipoJugada.Torneo, resultadosString);
        }
        protected abstract string ObtenResultados();
        #endregion


        
    }

   
}
