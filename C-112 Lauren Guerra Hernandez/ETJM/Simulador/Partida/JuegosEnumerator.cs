using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;
using System.Collections;

namespace Simulador
{
    public class JuegosEnumerator : IEnumerator<Juego>
    {
        
        public Juego Current
        {
            get
            {
                if (noHaComenzadolaPartida) throw new InvalidOperationException("No ha comenzado la partida");
                if (partidaTerminada) throw new InvalidOperationException("La partida ha acabado");
                return juegoActual;
            }
        }

        private Dictionary<Player, int> puntuaciones;        
        private List<Player> jugadores;
        private IGame<Player> juego;
        private bool partidaTerminada;
        private bool noHaComenzadolaPartida;
        private Juego juegoActual;
        private int count;
        private bool permiteEmpate;
        private Player winner;
        private List<Player> winners;

        private Dictionary<Team, int> puntuacionesEquipos;
        private bool porEquipos;
        private List<Team> equipos;


        public JuegosEnumerator(List<Player> jugadores, IGame<Player> juego, Dictionary<Player, int> puntuaciones, bool permiteEmpate = true)
        {
            this.juego = juego;
            this.jugadores = jugadores;
            this.permiteEmpate = permiteEmpate;
            this.puntuaciones = puntuaciones;
            porEquipos = false;
            Reset();
        }
        public JuegosEnumerator(List<Team> equipos, IGame<Player> juego, Dictionary<Team, int> puntuaciones, bool permiteEmpate = true)
        {
            this.equipos = equipos;
            jugadores = null;
            this.juego = juego;
            puntuacionesEquipos = puntuaciones;
            this.permiteEmpate = permiteEmpate;
            porEquipos = true;

        }
        public void Reset()
        {
            partidaTerminada = false;
            noHaComenzadolaPartida = true;
            count = 0;
        }


        public bool MoveNext()
        {
            if (noHaComenzadolaPartida) { noHaComenzadolaPartida = false; juegoActual = new Juego(juego, jugadores); return true; }
            if (!partidaTerminada) PartidaTerminada();
            if (partidaTerminada) return false;
            if (porEquipos)
                ActualizaPuntuacionesEquipos();
            else ActualizaPuntuaciones();
            count++;
            if(porEquipos) juegoActual = new Juego(juego, equipos);
            else juegoActual = new Juego(juego, jugadores);
            return true;
        }
        /// <summary>
        /// Comprueba si la partida termino, si es asi busca sus resultados
        /// </summary>
        protected void PartidaTerminada()
        {
            if (!Current.JuegoTerminado) return;
            partidaTerminada = juego.PartidaTerminada(new Partidita(permiteEmpate, count));
            if (partidaTerminada)
            {
                ActualizaPuntuaciones();
                BuscaResultado();
               
            }
        }
        private void BuscaResultado()
        {
            winners = new List<Player>();
            bool hayEmpate = false;
            Tools.BuscaResultados(winners, ref winner, puntuaciones, ref hayEmpate);
            
        }
        private void ActualizaPuntuaciones()
        {
            foreach (var x in juegoActual.Puntuaciones)
            {
                puntuaciones[x.Key] += x.Value;
            }
        }
        private void ActualizaPuntuacionesEquipos()
        {
            foreach (var x in juegoActual.PuntuacionesEquipos)
            {
                puntuacionesEquipos[x.Key] += x.Value;
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
