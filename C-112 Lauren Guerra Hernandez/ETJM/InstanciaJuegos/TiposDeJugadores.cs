using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;
using Simulador;
using Tic_Tac_Toe;

namespace InstanciaJuegos
{
    public class TiposDeJugadores<T>
    {
        private Dictionary<string, Type> crea = new Dictionary<string, Type>();
        public Type this[string x] { get { return crea[x]; } }
        public IEnumerable<string> Keys { get { return crea.Keys; } }
        public IEnumerable<Type> Values { get { return crea.Values; } }
        public int Count { get { return crea.Count; } }
        public TiposDeJugadores(IGame<T> juego)
        {
            crea.Add("Jugador Aleatorio", new RandomPlayer("").GetType());
            crea.Add("Jugador Goloso", new GreedyPlayer("").GetType());

            JugadoresEspecificos(juego);            
        }
        private void JugadoresEspecificos(IGame<T> juego)
        {
           ////////////////En este metodo se deben agregar al diccionario los jugadores especificos segun el tipo de juego que sea IGame<T>
        }

        public bool EsteTipoPertenece(Type tipo)
        {
            foreach (var x in crea)
            {
                if (Equals(tipo, x.Value)) return true;
            }
            return false;
        }
    }
}
