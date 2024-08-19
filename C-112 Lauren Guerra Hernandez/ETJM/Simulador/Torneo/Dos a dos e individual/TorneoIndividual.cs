using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;


namespace Simulador
{
    public class TorneoIndividual : Torneo
    {
        public override bool PermiteEmpate { get { return true; } }

        public override bool TorneoPorEquipos
        {
            get
            {
                return false;            }
        }
        public override bool EquipoContraEquipo
        {
            get
            {
                return false;
            }
        }
        private List<Partida> partidas;

        private List<Player> winners;
        private bool hayEmpate;
        private Player winner;
        public TorneoIndividual(List<Player> players, IGame<Player> juego) : base(players, juego)
        {
            if (players.Count % juego.MaxCantidadPlayers != 0) throw new ArgumentException("La cantidad de jugadores del torneo debe ser multiplo de la cantidad de jugadores que admite el juego");
            partidas = new List<Partida>();
            OrganizaPartidas();
            PartidasEnumerable = new PartidasEnumerable(Players, Juego, partidas, puntuaciones);
            PartidasEnumerator = PartidasEnumerable.GetEnumerator();
        }
      
        public override void OrganizaPartidas()
        {
            List<Player> copia = Tools.Copia(Players);
            Random rnd = new Random();
            while (copia.Count > 0)
            {
                List<Player> jugadoresDeEstaPartida = new List<Player>();
                for (int i = 0; i < Juego.MaxCantidadPlayers; i++)
                {                       
                    int r = rnd.Next(0, copia.Count);
                    jugadoresDeEstaPartida.Add(copia[r]);
                    copia.RemoveAt(r);
                }
                partidas.Add(new Partida(jugadoresDeEstaPartida, Juego, PermiteEmpate));
            }
        }
       
        public override string ToString()
        {
            return "Torneo individual";
        }

        protected override string ObtenResultados()
        {
            BuscaResultado();
            string result = "Lista de puntuaciones";
            int max = Puntuaciones[Puntuaciones.Keys.First()];
            foreach (var x in Puntuaciones)
            {
                result += $"\n {x.Key}: {x.Value} puntos";
            }
            if (hayEmpate)
            {
                result += $"\n Los ganadores del torneo son {Tools.GetStringDeJugadores(winners)}";
            }
            else
            {
                result += $"\n El ganador del torneo es {winner}";

            }
            return result;
        }
        private void BuscaResultado()
        {
            winners = new List<Player>();

            Tools.BuscaResultados(winners, ref winner, Puntuaciones, ref hayEmpate);
            
        }
    }
}
