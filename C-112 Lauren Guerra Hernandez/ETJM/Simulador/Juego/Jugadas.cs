using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador
{
    public class Jugadas
    {
        /// <summary>
        /// Devuelve el numero de prioridad que tiene esta jugada 
        /// </summary>
        public TipoJugada TipoJugada { get; }
        public string Jugada { get; }
        public Jugadas(TipoJugada tipoJugada, string jugada)
        {
            TipoJugada = tipoJugada;
            Jugada = jugada;
        }
        public override string ToString()
        {
            return Jugada;
        }
    }
}
