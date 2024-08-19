using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    class TituloEnumerator : IEnumerator<Partida>
    {
        private IGame<Player> juego;
        private List<Player> partidas;
        private Player campeon;
        private Team equipoCampeon;
        private bool huboMN;
        private bool terminoLaPartida;
        private Partida current;
        private int indiceActual;
        private Dictionary<Player, int> puntuaciones;
        private Dictionary<Team, int> puntuacionesEquipos;
        private List<Team> partidasEquipos;
        private bool porEquipos;
        private bool yaOcurrioLaPrimeraPartida;
        private  bool permiteEmpate { get { return false; } }

        public TituloEnumerator( IGame<Player> Juego, List<Player> Partidas, Dictionary<Player,int>puntuaciones)
        {
            porEquipos = false; 
            juego = Juego;
            partidas = Partidas;
            partidasEquipos = null;
            this.puntuaciones = puntuaciones;
            puntuacionesEquipos = null;
             Reset();
        }
        public TituloEnumerator(IGame<Player> Juego, List<Team> Partidas, Dictionary<Team, int> puntuaciones)
        {
            porEquipos = true;
            juego = Juego;
            partidas = null;
            partidasEquipos = Partidas;
            this.puntuaciones = null;
            this.puntuacionesEquipos = puntuaciones;
            Reset();
        }
        public void Reset()
        {
            indiceActual = 0;
            campeon = null;
            equipoCampeon = null;
            huboMN = false;
            terminoLaPartida = false;
            current = null;
            yaOcurrioLaPrimeraPartida = false;
           
        }
        public Partida Current
        {
            get
            {
                if (!huboMN) throw new InvalidOperationException("El torneo no ha comenzado");
                if(terminoLaPartida) throw new InvalidOperationException("El torneo ha acabado");
                return current;
            }
        }

        public bool MoveNext()
        {
            if(!yaOcurrioLaPrimeraPartida)
            {
                yaOcurrioLaPrimeraPartida = true;
                huboMN = true;
                if (porEquipos)
                {
                    List<Team> equiposDeLaPartidaActual = new List<Team>();
                    equiposDeLaPartidaActual.Add(partidasEquipos[0]);
                    equiposDeLaPartidaActual.Add(partidasEquipos[1]);
                    current = new Partida(equiposDeLaPartidaActual, juego, permiteEmpate);
                }
                else
                {
                    List<Player> jugadoresDeLaPartidaActual = new List<Player>();
                    jugadoresDeLaPartidaActual.Add(partidas[0]);
                    jugadoresDeLaPartidaActual.Add(partidas[1]);
                    current = new Partida(jugadoresDeLaPartidaActual, juego, permiteEmpate);
                }
                indiceActual = 2;
                return huboMN=true;
            }

            if (porEquipos)
            {
                ActualizaEquipoCampeon();
                ActualizaPuntuacionesEquipos(); 
            }
            else
            {
                ActualizaCampeon();
                ActualizaPuntuaciones();
            }
            if (terminoLaPartida) return false;
            if (indiceActual >= partidas.Count) return terminoLaPartida = false;
            if (porEquipos)
            {
                List<Team> equiposDeLaPartidaActual = new List<Team>();
                equiposDeLaPartidaActual.Add(equipoCampeon);
                equiposDeLaPartidaActual.Add(partidasEquipos[indiceActual]);
                current = new Partida(equiposDeLaPartidaActual, juego, permiteEmpate);
            }
            else
            {
                List<Player> jugadoresDeLaPartidaActual = new List<Player>();
                jugadoresDeLaPartidaActual.Add(campeon);
                jugadoresDeLaPartidaActual.Add(partidas[indiceActual]);
                current = new Partida(jugadoresDeLaPartidaActual, juego, permiteEmpate);
            }
            indiceActual++;
            return true;

        }
        private void ActualizaCampeon()
        {
            if (current.HayEmpate)
            {
                campeon = current.Jugadores[0];
                return;
            }
            int max = int.MinValue;
            foreach (var x in current.Puntuaciones)
            {
                if (x.Value > max) { max = x.Value; campeon = x.Key; }
            }
        }
        private void ActualizaEquipoCampeon()
        {
            if (current.HayEmpate)
            {
                equipoCampeon = current.Equipos[0];
                return;
            }
            int max = int.MinValue;
            foreach (var x in current.PuntuacionesEquipos)
            {
                if (x.Value > max) { max = x.Value; equipoCampeon = x.Key; }
            }
        }
        private void ActualizaPuntuacionesEquipos()
        {
            foreach (var x in partidasEquipos)
            {
                if (Equals(x, equipoCampeon)) puntuacionesEquipos[x] = 1;
                else puntuacionesEquipos[x] = 0;
            }
        }
        private void ActualizaPuntuaciones()
        {
            foreach (var x in partidas)
            {
                if (Equals(x, campeon)) puntuaciones[x] = 1;
                else puntuaciones[x] = 0;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        public void Dispose()
        {

        }
    }
}
