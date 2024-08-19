using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulador;
using IJuego;
using Tic_Tac_Toe;

namespace InstanciaJuegos
{
    public class CreaJuego<T>
    {
        private Dictionary<string, IGame<T>> crea = new Dictionary<string, IGame<T>>();
        public IGame<T> this[string x] { get { return crea[x]; } }
        public IEnumerable<string> Keys { get { return crea.Keys; } }
        public IEnumerable<IGame<T>> Values { get { return crea.Values; } }
        public int Count { get { return crea.Count; } }
        public CreaJuego()
        {
            crea.Add("Tic Tac Toe", new TicTacToe<T>());
            
        }
    }
}
