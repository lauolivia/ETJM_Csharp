using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    class PartidaEnumerator : IEnumerator<Partida>
    {
        private IGame<Player> juego;
        private List<Partida> partidas;
        private Dictionary<Player, int> puntuaciones;
        private bool huboMN;
        private bool terminoLaPartida;
        private Partida current;
        private Dictionary<Team, int> puntuacionesEquipos;
        private bool porEquipos;
        public PartidaEnumerator(List<Player> Players, IGame<Player> Juego, List<Partida> Partidas, Dictionary<Player, int> puntuaciones)
        {
            juego = Juego;
            partidas = Partidas;
            puntuacionesEquipos = null;
            this.puntuaciones = puntuaciones;
            porEquipos = false;
             Reset();
        }
        public PartidaEnumerator(List<Team> Equipos, IGame<Player> Juego, List<Partida> Partidas, Dictionary<Team, int> puntuacionesEquipos)
        {
            juego = Juego;
            partidas = Partidas;
            puntuaciones = null;
            this.puntuacionesEquipos = puntuacionesEquipos;
            porEquipos = true;
             Reset();
        }

        public void Reset()
        {
            huboMN = false;
            terminoLaPartida = false;
            current = null;
        }
        public Partida Current
        {
            get
            {
                if (!huboMN) throw new InvalidOperationException("El torneo no ha comenzado");
                if (terminoLaPartida) throw new InvalidOperationException("El torneo ha acabado");
                return current;
            }
        }
        public bool MoveNext()
        {
            if(huboMN) ActualizaPuntuaciones();
            if (!huboMN) huboMN = true;
            if (terminoLaPartida) return false;
            if (partidas.Count==0) return terminoLaPartida = false;
            Random rnd = new Random();
            int indicePartida = rnd.Next(0, partidas.Count);
            current = partidas[indicePartida];
            partidas.RemoveAt(indicePartida);
            return true;
        }
        private void ActualizaPuntuaciones()
        {
            if(!porEquipos)
                foreach (var x in current.Puntuaciones)
                {
                    puntuaciones[x.Key] += x.Value;
                }
            else
                foreach (var x in current.PuntuacionesEquipos)
                {
                    puntuacionesEquipos[x.Key] += x.Value;
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
