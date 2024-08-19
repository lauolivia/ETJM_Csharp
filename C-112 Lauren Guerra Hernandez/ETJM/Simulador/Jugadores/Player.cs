using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IJuego;

namespace Simulador
{

    public abstract class Player
    {
        public string Name { get; }
        public Player(string name)
        {
            Name = name;
        }
        public abstract void Play(IGame<Player> juego);
        public override bool Equals(object a)
        {
            if (Equals(this.GetType(), a.GetType())) return ((Player)a).Name == this.Name;
            return false;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() + this.GetType().GetHashCode();
        }
        public override string ToString()
        {
            return Name;
        }

    }

   

   

}
