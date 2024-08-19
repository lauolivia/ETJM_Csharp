using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador
{
    public static class Tools
    {
        /// <summary>
        /// Devuelve un string con los nombres de los jugadores
        /// </summary>
        /// <param name="jugadores"></param>
        /// <returns></returns>
        public static string GetStringDeJugadores<T>(List<T> jugadores)
        {
            string result = "";
            for (int i = 0; i < jugadores.Count; i++)
            {
                if (i == 0) { result += jugadores[i]; continue; }
                if (i == jugadores.Count - 1) { result += $" y {jugadores[i]}"; continue; }
                result += $", {jugadores[i]}";
            }
            return result;
        }
        public static void CreaPuntuaciones<T>(Dictionary<T, int> puntuaciones, List<T> jugadores)
        {
            foreach (var jugador in jugadores)
            {
                puntuaciones.Add(jugador, 0);
            }
        }
        public static List<T> Copia<T>(List<T> Players)
        {
            List<T> copia = new List<T>();
            foreach (var x in Players)
            {
                copia.Add(x);
            }
            return copia;
        }

        public static Team EquipoAlQuePerteneceEsteJugador(List<Team>equipos,Player jugador)
        {
            foreach (var equipo in equipos)
            {
                foreach (var jugadorx in equipo.Players)
                {
                    if (Equals(jugador, jugadorx)) return equipo;
                }
            }
            return null;
        }
        public static List<Player> ObtenLosJugadoresdeEstosEquipos(List<Team> equipos)
        {
            List<Player> jugadores = new List<Player>();
            foreach (var equipo in equipos)
            {
                foreach (var player in equipo.Players)
                {
                    jugadores.Add(player);
                }
            }
            return jugadores;
        }
        public static void BuscaResultados<T>(List<T>winners,ref T winner, Dictionary<T,int>puntuaciones,ref bool hayEmpate)
        {
            int max = 0;
            foreach (var x in puntuaciones)
            {
                if (winners.Count == 0) { winners.Add(x.Key); max = x.Value; }
                else
                {
                    if (x.Value == max) winners.Add(x.Key);
                    if (x.Value > max) { winners.Clear(); winners.Add(x.Key); max = x.Value; }
                }
            }
            if (winners.Count == 1)
                winner = winners[0];
            else { hayEmpate = true; winner = default(T); }
        }
    }
}
