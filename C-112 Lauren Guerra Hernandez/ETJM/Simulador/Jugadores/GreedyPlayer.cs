using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{
    public class GreedyPlayer : Player
    {
        public GreedyPlayer(string name) : base(name)
        {

        }

        public override void Play(IGame<Player> juego)
        {
            Random rnd = new Random();
            int[] valores = juego.ValoresDeLasJugadasPosiblesParaEsteJugador(this);
            int jugada = 0;
            for (int i = 0; i < valores.Length; i++)
            {
                if (valores[jugada] < valores[i]) jugada = i;
                else if (valores[jugada] == valores[i])
                {
                    if (rnd.Next(2) == 1) jugada = i;
                }
            }
            juego.Juega(jugada, this);
        }
    }
}
