using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public class Partida
    {
        public IEnumerable<Juego> JuegosEnumerable { get; }
        /// <summary>                                                         
        /// Permite conocer si el torneo que se esta realizando acepta que las partidas queden empatadas
        /// </summary>
        public bool PermiteEmpate
        {
            get;
        }
        /// <summary>
        /// En caso de no haber quedado empatada la partida devuelve el ganador
        /// </summary>
        public Player Winner { get { if (!PartidaTerminada) throw new InvalidOperationException("No se puede pedir el ganador a una partida que aun no esta acabada"); return winner; } }//protected set { Winner = value; } }
        /// <summary>
        /// En caso de haber quedado empatada la parida devuelve los jugadores de mayor puntuacion(ganadores)
        /// </summary>
        public List<Player> Winners { get { if (!PartidaTerminada) throw new InvalidOperationException("No se puede pedir el ganador a una partida que aun no esta acabada"); return winners; } /*protected set { Winners = value;}*/  }
        /// <summary>
        /// Devuelve si la partida actual ha quedaddo empatada
        /// </summary>
        public bool HayEmpate { get { if (!PartidaTerminada) throw new InvalidOperationException("No se puede saber si hay empate si la partida aun no esta acabada"); return hayEmpate; }/* protected set { HayEmpate = value; }*/ }
        public Juego JuegoActual
        {
            get
            {
                if (!PartidaComenzada) throw new InvalidOperationException("La partida no ha comenzado");
                if (PartidaTerminada)
                    throw new InvalidOperationException("La partida ha acabado");
                return juegoActual;
            }
        }
        public Dictionary<Player, int> Puntuaciones { get; }
        /// <summary>
        /// Devuelve la jugada que representa los resultados de la partida
        /// </summary>
        public Jugadas Resultados
        {
            get
            {
                if (!PartidaTerminada) throw new InvalidOperationException("El jugadas aun no ha terminado, no se pueden pedir los resultados");
                return resultados;
            }

        }
        public bool PartidaTerminada { get; private set; }
        public bool PartidaComenzada { get; private set; }
        public Jugadas Comienzo
        {
            get
            {
                if
                    (!PartidaComenzada)
                    ComienzaPartida();
                return comienzo;
            }
        }
        public List<Player> Jugadores { get; }
        public List<Team> Equipos { get; }
        public bool PorEquipos { get; }
        public Dictionary<Team, int> PuntuacionesEquipos { get; }

        private Jugadas comienzo;
        private IEnumerator<Juego> JuegosEnumerator;
        private Player winner;
        private Team winnerEquipo;
        private List<Player> winners;
        private List<Team> winnersEquipos;
        private bool hayEmpate;
        private IGame<Player> juego;
        private Juego juegoActual;
        private Jugadas resultados;


        public Partida(List<Player> jugadores, IGame<Player> juego , bool permiteEmpate = true)
        {
            PorEquipos = false;
            PuntuacionesEquipos = null;
            this.juego = juego;
            Equipos = null;
            Jugadores = jugadores;
            PermiteEmpate = permiteEmpate;
            Puntuaciones = new Dictionary<Player, int>();
            Tools.CreaPuntuaciones(Puntuaciones, jugadores); 
            JuegosEnumerable= new JuegosEnumerables(jugadores, juego, Puntuaciones,permiteEmpate);
            Reset();
        }
        public Partida(List<Team> equipo, IGame<Player> juego, bool permiteEmpate = true)
        {
            PorEquipos = true;
            this.juego = juego;
            Jugadores = null;
            Puntuaciones = null;
            PermiteEmpate = permiteEmpate;
            Puntuaciones = new Dictionary<Player, int>();
            PuntuacionesEquipos = new Dictionary<Team, int>();
            Tools.CreaPuntuaciones(PuntuacionesEquipos, Equipos);
            JuegosEnumerable = new JuegosEnumerables(Equipos, juego, PuntuacionesEquipos, permiteEmpate);
            Reset();
        }
        public void Reset()
        {
            JuegosEnumerator = JuegosEnumerable.GetEnumerator();
            PartidaComenzada = false;
            PartidaTerminada = false;
        }
        public void ComienzaPartida()
        {
            PartidaComenzada = true;
            string comienzo = "";
            if (!PorEquipos) comienzo += $"Comienza nueva partida entre: {Tools.GetStringDeJugadores(Jugadores)}";
            else comienzo += $"Comienza nueva partida entre los equipos: {Tools.GetStringDeJugadores(Equipos)}";
            JuegosEnumerator.MoveNext();
            juegoActual = JuegosEnumerator.Current;
            this.comienzo = new Jugadas(TipoJugada.Partida, comienzo);
        }
        public void SiguienteJuego()
        {
            if (PartidaTerminada) throw new InvalidOperationException("No se puede pedir el siguiente juego porque la partida ha acabado");
            if (!PartidaComenzada)
            {
                ComienzaPartida();
            }

            if (!juegoActual.JuegoTerminado) throw new InvalidOperationException("No se puede acceder al siguiente juego si el presente no ha acabado");

            if (JuegosEnumerator.MoveNext()) { juegoActual = JuegosEnumerator.Current; return; }
            PartidaTerminada = true;
            TerminarPartida();

        }

        protected virtual void BuscaResultado()
        {
            if (!PorEquipos)
            {
                winners = new List<Player>();
                Tools.BuscaResultados(winners, ref winner, Puntuaciones, ref hayEmpate);  
            }
            else
            {
                winnersEquipos = new List<Team>();
                Tools.BuscaResultados(winnersEquipos, ref winnerEquipo, PuntuacionesEquipos, ref hayEmpate);
                
            }
        }

        protected void TerminarPartida()
        {
            if (PartidaTerminada)
            {
                BuscaResultado();
                string result = "";
                if (!PorEquipos)
                {
                    if (HayEmpate)
                    {
                        result = result + $"La partida ha acabado en empate, los ganadores son: {Tools.GetStringDeJugadores(Winners)}";

                    }
                    else
                    {
                        result = $"La partida ha acabado, el ganador es {Winner} ";
                    }
                }
                else
                {
                    if (HayEmpate)
                    {
                        result = result + $"La partida ha acabado en empate, los equipos ganadores son: {Tools.GetStringDeJugadores(winnersEquipos)}";

                    }
                    else
                    {
                        result = $"La partida ha acabado, el equipo ganador es {winnerEquipo} ";
                    }
                }
                resultados = new Jugadas(TipoJugada.Partida, result); 
            }
        }
        
        
    }
    
}
