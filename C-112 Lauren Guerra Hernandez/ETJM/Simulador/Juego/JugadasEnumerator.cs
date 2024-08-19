using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public class JugadasEnumerator : IEnumerator<Jugadas>
    {
        private Dictionary<Player, int> puntuaciones;
        private bool JuegoTerminado { get { return resultados; } }
        private IGame<Player> juego;
        private List<Player> jugadores;
        private Jugadas current;
        private bool huboMN, hayMN;
        private int jugadorActual, equipoActual;
        private bool resultados;        
        private Dictionary<Team, int> puntuacionesEquipos;
        private bool porEquipos;
        private List<Team> equipos;
        public JugadasEnumerator(IGame<Player> juego, List<Player> jugadores, Dictionary<Player, int> puntuaciones)
        {
            this.juego = juego;
            this.jugadores = jugadores;
            this.puntuaciones = puntuaciones;
            this.equipos = null;
            this.puntuacionesEquipos = null;
            porEquipos = false;

            Reset();
        }
        public JugadasEnumerator(IGame<Player> juego, List<Team> equipos, Dictionary<Team, int> puntuacionesEquipos)
        {
            this.juego = juego;
            this.equipos = equipos;
            this.puntuacionesEquipos = puntuacionesEquipos;
            this.jugadores = null;
            puntuaciones = new Dictionary<Player, int>();
            Tools.CreaPuntuaciones(puntuaciones, Tools.ObtenLosJugadoresdeEstosEquipos(equipos));
            porEquipos = true;
            Reset();

        }
        public void Reset()
        {
            resultados = false;
            huboMN = false;
            hayMN = true;
            equipoActual = 0;
            jugadorActual = 0;
            juego.NuevoJuego(jugadores);
        }
        
        public Jugadas Current
        {
            get
            {
                if (!huboMN) throw new InvalidOperationException("No ha comenzado el jugadas");
                else if (!hayMN) throw new InvalidOperationException("El juego ha terminado");
                return current;
            }
        }
        public bool MoveNext()
        {
            if (!huboMN) huboMN = true;
            if (!hayMN) return false;
            if (!porEquipos)
            {
                if (juego.CantPosiblesJugadasEsteJugador(jugadores[jugadorActual]) > 0)///Si este jugador aun puede jugar, juega
                {
                    jugadores[jugadorActual].Play(juego);
                    current = new Jugadas(TipoJugada.Jugadas, juego.UltimaJugada);
                    jugadorActual = (jugadorActual + 1) % jugadores.Count;
                    return hayMN = true;
                }
                if (!juego.Finalizado) { jugadorActual = (jugadorActual + 1) % jugadores.Count; return MoveNext(); }///Si el juego no ha terminado pero el jugador anterior no tiene mas jugadas por hacer, juega el siguiente
                else if (!resultados)///Si no se han dado aun los resultados,estos se actualizan// se agregan al Enumerator de jugadas
                {
                    ActualizaPuntuaciones();
                    if (porEquipos)
                        ActualizaPuntuacionesEquipo();
                    resultados = true;
                    return hayMN = false;

                }
            }
            if (porEquipos)
            {
                if (juego.CantPosiblesJugadasEsteJugador(equipos[equipoActual].Players[jugadorActual]) <= 0) return hayMN = false;
                equipos[equipoActual].Play(jugadorActual, juego);
                current = new Jugadas(TipoJugada.Jugadas, juego.UltimaJugada);
                if (equipoActual == equipos.Count - 1) jugadorActual = (jugadorActual + 1) % jugadores.Count;
                equipoActual = (equipoActual + 1) % equipos.Count;
                return true;
            }
            else if (!resultados)///Si no se han dado aun los resultados,estos se actualizan// se agregan al Enumerator de jugadas
            {
                ActualizaPuntuaciones();
                if (porEquipos)
                    ActualizaPuntuacionesEquipo();
                resultados = true;
                return hayMN = false;

            }
            
           
            return hayMN = false;

        }
        private void ActualizaPuntuacionesEquipo()
        {
            foreach (var x in puntuaciones)
            {
                puntuacionesEquipos[Tools.EquipoAlQuePerteneceEsteJugador(equipos,x.Key)] += x.Value;
            }
        }
        private void ActualizaPuntuaciones()
        {
            foreach (var x in juego.Puntuaciones)
            {
                puntuaciones[x.Key] = x.Value;
            }
        }

        public void Dispose()
        {
        }
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

    }

}
