using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public class Simula
    {
        public Jugadas JugadaActual { get { if (!torneoIniciado) throw new InvalidOperationException("No se puede pedir la jugada actual porque no ha comenzado el torneo"); return jugada; } }
        private bool torneoIniciado;
        private Jugadas jugada;
        private bool resultadosPartidaActual;

        private Torneo torneo;
        private Partida partidaActual;
        private Juego juegoActual;
        public bool Fin { get; private set; }
        public Simula(Torneo torneo)
        {
            this.torneo = torneo;
            Reset(); 
        }
        public void Reset()
        {
            torneoIniciado = false;
            Fin = false;
            resultadosPartidaActual = false;            
        }

        /// <summary>
        /// Devuelve la siguiente jugada de la ejecucion
        /// </summary>
        public void SiguienteJugada()
        {
            if (Fin) return;
            
            if (!torneo.TorneoComenzado) { ComienzaTorneo(); return; }
            if (!partidaActual.PartidaComenzada) { ComienzaPartida(); return; }

            if (!juegoActual.JuegoComenzado) { ComienzaJuego(); return; }
            if (torneo.TorneoTerminado) { TorneoTerminado(); return; }
            if (partidaActual.PartidaTerminada&&!resultadosPartidaActual)
            {
                jugada = partidaActual.Resultados;
                resultadosPartidaActual = true;
                return;
            }
            if(partidaActual.PartidaTerminada && resultadosPartidaActual)
            {
                resultadosPartidaActual = false;

                torneo.SiguientePartida();
                partidaActual = torneo.PartidaActual;
                SiguienteJugada();
                return;
            }
            if (juegoActual.JuegoTerminado)
            { 
                partidaActual.SiguienteJuego();
                if (partidaActual.PartidaTerminada) { SiguienteJugada(); return; }
                juegoActual = partidaActual.JuegoActual;
                SiguienteJugada();
                return;
            }

            juegoActual.SiguienteJugada();
            jugada = juegoActual.Actual;
        }
        
        #region Comienzos
        public void ComienzaTorneo()
        {
            if (torneoIniciado) throw new InvalidOperationException("El torneo ya ha iniciado");
            torneoIniciado = true;
            torneo.IniciaTorneo();
            partidaActual = torneo.PartidaActual;
            jugada = torneo.Comienzo;
        }
        public void ComienzaPartida()
        {
            resultadosPartidaActual = false;

            partidaActual.ComienzaPartida();
            juegoActual = partidaActual.JuegoActual;
            jugada = partidaActual.Comienzo;
        }
        public void ComienzaJuego()
        {
            juegoActual.ComienzaJuego();
            jugada = juegoActual.Comienzo;
        }
        #endregion
        

        #region Terminar
        public IEnumerable<Jugadas> TerminarJuego()
        {
            while (!juegoActual.JuegoTerminado)
            {
                SiguienteJugada();
                yield return jugada;
            }
        }
        public IEnumerable<Jugadas> TerminarPartida()
        {
            while (!partidaActual.PartidaTerminada)
            {
                SiguienteJugada();
                yield return jugada;
            }
        }
        
        public IEnumerable<Jugadas> TerminarTorneo()
        {
           
            while(!Fin)
            {
                SiguienteJugada();
                yield return jugada;
            }

        }
        private void TorneoTerminado()
        {
            Fin = true;
            jugada = torneo.ResultadosTorneo;

        }
        #endregion
    }
}
