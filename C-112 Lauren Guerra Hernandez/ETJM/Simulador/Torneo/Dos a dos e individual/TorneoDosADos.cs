using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public class TorneoDosADos : Torneo
    {
        private List<Partida> partidas;
        private List<Player> winners;
        private bool hayEmpate;
        private Player winner;
        private bool torneoPorEquipos;
        private bool equipoContraEquipo;

        private bool hayEmpateEquipo;
        private Team winnerEquipo;
        private List<Team> winnersEquipos;

        public override bool PermiteEmpate { get { return true; } }
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
                return equipoContraEquipo;
            }
        }

        public TorneoDosADos(List<Player> players, IGame<Player> juego) : base(players, juego)
        {
            torneoPorEquipos = false;
            equipoContraEquipo = false;
            partidas = new List<Partida>();            
            OrganizaPartidas();
            PartidasEnumerable = new PartidasEnumerable(Players,Juego, partidas, puntuaciones);
            PartidasEnumerator = PartidasEnumerable.GetEnumerator(); hayEmpate = false;

        }
        public TorneoDosADos(List<Team> equipos, IGame<Player> juego):base(equipos,juego)
        {
            torneoPorEquipos = true;
            equipoContraEquipo = juego.PermiteEquipos;
            partidas = new List<Partida>();
            OrganizaPartidas();
            PartidasEnumerable = new PartidasEnumerable(Players, Juego, partidas, puntuaciones);
            PartidasEnumerator = PartidasEnumerable.GetEnumerator();
            Tools.CreaPuntuaciones(puntuacionesEquipos, Equipos);
            hayEmpateEquipo = false;
        }
        public override void OrganizaPartidas()
        {
            if (EquipoContraEquipo)
            {
                OrganizaPartidasEntreEquipos(); return;
            }
            if (TorneoPorEquipos)
            {
                OrganizaPartidasConEquipos(); return;
            }
            for (int i = 0; i < Players.Count; i++)
            {
                for (int j = i+1; j < Players.Count; j++)
                {
                    List<Player> jugadoresPartidaActual = new List<Player>();
                    jugadoresPartidaActual.Add(Players[i]);
                    jugadoresPartidaActual.Add(Players[j]);
                    partidas.Add(new Partida(jugadoresPartidaActual, Juego, PermiteEmpate));
                }
            }
        }
        /// <summary>
        /// Organiza partidas donde los equipos se enfrentan entre si
        /// </summary>
        public void OrganizaPartidasEntreEquipos()
        {
            for (int i = 0; i < Equipos.Count; i++)
            {
                for (int j = i + 1; j < Equipos.Count; j++)
                {
                    List<Team> equiposPartidaActual = new List<Team>();
                    equiposPartidaActual.Add(Equipos[i]);
                    equiposPartidaActual.Add(Equipos[j]);
                    partidas.Add(new Partida(equiposPartidaActual, Juego, PermiteEmpate));
                }
            }
        }
        /// <summary>
        /// Organiza partidas donde los jugadores de un equipo se enfrentan individualmente con los jugadores de otros equipos
        /// </summary>
        public void OrganizaPartidasConEquipos()
        {
            for (int i = 0; i < Equipos.Count; i++)
            {
                for (int j = i+1; j < Equipos.Count; j++)
                {
                    foreach (var jugador1 in Equipos[i].Players)
                    {
                        foreach (var jugador2 in Equipos[j].Players)
                        {
                            List<Player> jugadoresDeEstaPartida = new List<Player>();
                            jugadoresDeEstaPartida.Add(jugador1);
                            jugadoresDeEstaPartida.Add(jugador2);
                            partidas.Add(new Partida(jugadoresDeEstaPartida, Juego, PermiteEmpate));
                        }
                    }
                }
               
            } 
                
            
        }

        protected override string ObtenResultados()
        {
            string result = "Lista de puntuaciones";

            BuscaResultado();
            if (TorneoPorEquipos)
            {
                int max = puntuacionesEquipos[puntuacionesEquipos.Keys.First()];
                foreach (var x in puntuacionesEquipos)
                {
                    result += $"\n {x.Key}: {x.Value} puntos";
                }
                if (hayEmpateEquipo)
                {
                    result += $"\n Los equipos ganadores del torneo son {Tools.GetStringDeJugadores(winnersEquipos)}";
                }
                else
                {
                    result += $"\n El equipo ganador del torneo es {winnerEquipo}";

                }
            }
            else
            {
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
            }
            return result;

        }
        private void ActualizaPuntuacionesDeEquipos()
        {
            foreach (var x in Puntuaciones)
            {
                puntuacionesEquipos[Tools.EquipoAlQuePerteneceEsteJugador(Equipos, x.Key)] += x.Value;
            }
        }
        private void BuscaResultado()
        {
            if (TorneoPorEquipos)
            {
                winnersEquipos = new List<Team>();
                if (!EquipoContraEquipo) ActualizaPuntuacionesDeEquipos();
                Tools.BuscaResultados(winnersEquipos, ref winnerEquipo, puntuacionesEquipos, ref hayEmpateEquipo);
            }

            else
            {
                winners = new List<Player>();
                Tools.BuscaResultados(winners, ref winner, Puntuaciones, ref hayEmpate);

            }
        }
        public override string ToString()
        {
            return "Torneo dos a dos";
        }

    }
}
