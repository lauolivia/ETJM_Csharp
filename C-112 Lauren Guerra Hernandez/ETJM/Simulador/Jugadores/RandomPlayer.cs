using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public class RandomPlayer : Player
    {
        public RandomPlayer(string name) : base(name)
        {

        }

        public override void Play(IGame<Player> juego)
        {
            int max = juego.CantPosiblesJugadasEsteJugador(this);
            Random rnd = new Random();
            int jugadaNumero = rnd.Next(max);
            juego.Juega(jugadaNumero, this);
        }
    }

}
