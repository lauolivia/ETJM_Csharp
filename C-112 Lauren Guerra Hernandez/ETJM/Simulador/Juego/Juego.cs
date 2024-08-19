using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public class Juego
    {
        public bool JuegoTerminado { get; private set; }
        public bool JuegoComenzado { get; private set; }
        /// <summary>
        /// Devuelve el IEnumerable de las jugadas que se desarrollan en el jueog
        /// </summary>
        public IEnumerable<Jugadas> jugadasEnumerable { get; }

        /// <summary>
        /// Al terminar el juego, guarda las puntuaciones obtenidas por los jugadores
        /// </summary>
        public Dictionary<Player, int> Puntuaciones
        {
            get
            {
                if (JuegoTerminado && !porEquipos) return puntuaciones;
                throw new InvalidOperationException("El juego aun no ha terminado, no se pueden pedir las puntuaciones");
            }
        }
        /// <summary>
        /// Devuelve la ultima jugada hecha
        /// </summary>
        public Jugadas Actual
        {
            get
            {
                if (!JuegoComenzado) throw new InvalidOperationException("El juego actual no ha iniciado");
                return current;
            }
        }
        /// <summary>
        /// Jugada que equivale al comienzo del juego
        /// </summary>
        public Jugadas Comienzo { get; private set; }
        /// <summary>
        /// Cuando el juego acaba, devuelve sus resultados
        /// </summary>

        public Dictionary<Team, int> PuntuacionesEquipos
        {
            get
            {
                if (JuegoTerminado && porEquipos) return puntuacionesEquipos;
                throw new InvalidOperationException("El juego aun no ha terminado, no se pueden pedir las puntuaciones");
            }
        }

        public Jugadas Resultados
        {
            get
            {
                if (!JuegoTerminado) throw new InvalidOperationException("El jugadas aun no ha terminado, no se pueden pedir los resultados");
                return resultados;// new Jugadas(TipoJugada.Juego, Jugadas.Resultados);
            }

        } 
        
        private Jugadas resultados;
        private Jugadas current;
        private IEnumerator<Jugadas> jugadasEnumerator;//Enumerador de jugadas, se utiliza para obtener de manera manual la siguiente jugada
        private IGame<Player> juego;
        private List<Player> jugadores;       
        private Dictionary<Player, int> puntuaciones;
        private List<Team> equipos;
        private Dictionary<Team, int> puntuacionesEquipos;
        private bool porEquipos;

        public Juego(IGame<Player> juego, List<Player> jugadores)
        {
            this.juego = juego;
            this.jugadores = jugadores;
            equipos = null;
            puntuaciones = new Dictionary<Player, int>();
            Tools.CreaPuntuaciones(puntuaciones, jugadores);
            porEquipos = false;
             jugadasEnumerable = new JugadasEnumerable(juego, jugadores, puntuaciones);
            Reset();
        }
        public Juego(IGame<Player> juego, List<Team> equipos)
        {
            this.equipos = equipos;
            this.juego = juego;
            this.jugadores = null;
            porEquipos = true;
             puntuacionesEquipos = new Dictionary<Team, int>();
            Tools.CreaPuntuaciones(puntuacionesEquipos, equipos);
            jugadasEnumerable = new JugadasEnumerable(juego, equipos, puntuacionesEquipos);
            Reset();
        }
        public void Reset()
        {
            jugadasEnumerator = jugadasEnumerable.GetEnumerator();
            JuegoComenzado = false;
            JuegoTerminado = false;
        }
       /// <summary>
       /// Da comienzo al juego
       /// </summary>
        public void ComienzaJuego()
        {
            JuegoComenzado = true;
            string comienzo = $"Comienza nuevo juego entre: ";
            if (porEquipos) comienzo += $"{Tools.GetStringDeJugadores(equipos)}";
            else comienzo += $"{Tools.GetStringDeJugadores(jugadores)}";
            Comienzo = new Jugadas(TipoJugada.Juego, comienzo);
        }
        /// <summary>
        /// Si el juego no ha terminado, pide al Enumerator la siguiente jugada
        /// </summary>
        public void SiguienteJugada()
        {
            if (JuegoTerminado) return;
            
            if (!JuegoComenzado)
            {
                ComienzaJuego();                
            }

            if (jugadasEnumerator.MoveNext()) current = jugadasEnumerator.Current;
            else
            {
                JuegoTerminado = true;
                resultados = new Jugadas(TipoJugada.Juego, juego.Resultados);
                current = resultados;
            }
        }

      
    }


}
