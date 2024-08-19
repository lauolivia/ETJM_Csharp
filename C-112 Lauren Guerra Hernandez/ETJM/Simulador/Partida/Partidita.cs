using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    /// <summary>
    /// Clase auxiliar para guardar las condiciones de una partida, para poder preguntar al juego si esta esta acabada
    /// </summary>
    public class Partidita : IPartida
    {
        public int CantidadDeJuegosJugados
        {
            get
            {
                return cantidadDeJuegos;
            }
        }

        public bool PermiteEmpate
        {
            get
            {
                return permiteEmpate;
            }
        }
        private bool permiteEmpate;
        private int cantidadDeJuegos;
        public Partidita(bool permiteEmpate, int cantidadDeJuegos)
        {
            this.cantidadDeJuegos = cantidadDeJuegos;
            this.permiteEmpate = permiteEmpate;

        }
    }
}
