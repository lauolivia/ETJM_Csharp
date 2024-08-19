using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;


namespace Simulador
{
    public class TorneoPorTitulo : Torneo
    {
        public override bool PermiteEmpate { get { return false; } }
        private bool torneoPorEquipos;
        private List<Player> partidas;
        private List<Team> partidasEquipos;

        public override bool TorneoPorEquipos
        {
            get
            {
                return torneoPorEquipos;
            }
        }
        public override bool EquipoContraEquipo
        {
            get
            {
                return torneoPorEquipos;
            }
        }
        
        public TorneoPorTitulo(List<Player> players, IGame<Player> juego) : base(players, juego)
        {
            partidas = new List<Player>();
            torneoPorEquipos = false;
            OrganizaPartidas();
            PartidasEnumerable = new TituloEnumerable( juego, partidas, puntuaciones);
            PartidasEnumerator = PartidasEnumerable.GetEnumerator();

        }
        public TorneoPorTitulo(List<Team> equipos, IGame<Player> juego) : base(equipos, juego)
        {
            partidasEquipos = new List<Team>();
            torneoPorEquipos = true;
            OrganizaPartidas();
            PartidasEnumerable = new TituloEnumerable(juego, partidasEquipos, puntuacionesEquipos);
            PartidasEnumerator = PartidasEnumerable.GetEnumerator();
        }

        /// <summary>
        /// Crea una copia de los jugadores para ir eligiendolos en orden aleatorio y pasando a las partidas cual sera el siguiente jugador que batira al campeon
        /// </summary>
        public override void OrganizaPartidas()
        {
            if (torneoPorEquipos)
            {
                OrganizaPartidasEquipos(); return;
            }
            List<Player> copia = Tools.Copia(Players);
            Random rnd = new Random();
            while (copia.Count > 0)
            {
                int valorRandom = rnd.Next(0, copia.Count);
                partidas.Add(copia[valorRandom]);
                copia.RemoveAt(valorRandom);
            }
        }
        private void OrganizaPartidasEquipos()
        {
            List<Team> copia = Tools.Copia(Equipos);
            Random rnd = new Random();
            while (copia.Count > 0)
            {
                int valorRandom = rnd.Next(0, copia.Count);
                partidasEquipos.Add(copia[valorRandom]);
                copia.RemoveAt(valorRandom);
            }
        }
        public override string ToString()
        {
            return "Torneo por el titulo";
        }
        protected override string ObtenResultados()
        {
            if (torneoPorEquipos)
            {
                Team winner = puntuacionesEquipos.Keys.First();
                int max = puntuacionesEquipos[puntuacionesEquipos.Keys.First()];
                foreach (var x in puntuacionesEquipos)
                {
                    if (x.Value > max) winner = x.Key;
                }
                if (winner == null) throw new Exception("No hay ganador");
                return $"El equipo {winner} ha ganado el torneo";
            }
            else
            {
                Player winner = Puntuaciones.Keys.First();
                int max = Puntuaciones[Puntuaciones.Keys.First()];
                foreach (var x in Puntuaciones)
                {
                    if (x.Value > max) winner = x.Key;
                }
                if (winner == null) throw new Exception("No hay ganador");
                return $"El jugador {winner} ha ganado el torneo";
            }
        }
    }
}
