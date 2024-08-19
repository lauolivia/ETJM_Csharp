using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IJuego
{
    //public interface IPlayer
    //{
    //    int Score { get; set; }
    //}



    public interface IPartida
    {
        /// <summary>
        /// Devuelve la cantidad de juegos que se han jugado en esta partida
        /// </summary>
        int CantidadDeJuegosJugados { get; }
        /// <summary>
        /// Devuelve si la partida puedde acabar en empate
        /// </summary>
        bool PermiteEmpate { get; }


    }
    public interface IGame<T>
    {
        /// <summary>
        /// Devuelve si en el presente juego ya no quedan jugadas por hacer por tanto se pueden obtener sus resultados
        /// </summary>
        bool Finalizado { get; }
        /// <summary>
        /// Devuelve el ganador del juego, su puntuacion y la puntuacion de sus oponentes una vez finalizado el juego
        /// </summary>
        string Resultados { get; }
        /// <summary>
        /// Devuelve la ultima jugada que se ha realizado
        /// </summary>
        string UltimaJugada { get; }
        /// <summary>
        /// Representa la condicion que debe cumplir una partida de este juego en particular, devuelve true si ya se cumple esta condicion
        /// </summary>
        /// <returns></returns>
        bool PartidaTerminada(IPartida partida);// {  }
        /// <summary>
        /// Se van guardando las puntuaciones de los jugadores 
        /// </summary>
        Dictionary<T,int> Puntuaciones { get; }
        /// <summary>
        /// Lista que guarda los jugadores que participan en este encuentro
        /// </summary>
        List<T> Jugadores { get; }
        /// <summary>
        /// Devuelve true si el juego asigna puntuaciones a los jugadores, especifico para jugar el tipo torneo por clasificacion individual
        /// </summary>
        bool JuegoPorPuntuaciones { get; }
        
        /// <summary>
        /// Devuelve la cantidad de puntos que debe alcanzar un jugador para concluir una partida de este juego
        /// </summary>
        int CantidadDePuntosPartida { get; }
        /// <summary>
        /// Devuelve la minima cantidad de jugadores que deben participar en el juego
        /// </summary>
        int MinCantPlayers { get; }
        /// <summary>
        /// Devuelve la maxima cantidad de jugadores que pueden participar en el juego
        /// </summary>
        int MaxCantidadPlayers { get; }
        /// <summary>
        /// Devuelve true si el juego se puede jugar por equipos
        /// </summary>
        bool PermiteEquipos { get; }
        /// <summary>
        /// Devuelve, en el caso de permitirse equipos en este juego, la maxima cantidad de jugadores que puede tener
        /// </summary>
        int MaxCantidadDeJugadoresPorEquipo { get; }

        /// <summary>
        /// Devuelve true si en el juego se puede llegar a un empate
        /// </summary>
        bool Empatable { get; }

        /// <summary>
        /// Devuelve true si una partida de este juego se puede jugar en el formato de acumulacion de puntos
        /// </summary>
        bool JuegoPorPuntos { get; }
        /// <summary>
        /// Devuelve true si una partida de este juego se puede jugar en le formato de que el ganador sea el que mas juegos gane
        /// </summary>
        bool JuegoPorPartidas { get; }
        /// <summary>
        /// Devuelve la maxima cantidad de jugadores que puede tener un equipo de este juego
        /// </summary>
        int MaximoDeJugadoresPorEquipo { get; }
        
        /// <summary>
        /// Devuelve el ganador en caso de haberse terminado el juego
        /// </summary>
        T Winner { get; }

       
        /// <summary>
        /// Realiza una jugada a nombre de un jugador especifico y devuelve una descripcion de esta
        /// </summary>
        /// <param name="jugada"></param>
        /// <param name="jugador"></param>
        void Juega(int jugada, T jugador);

        /// <summary>
        /// Devuelve el numero de jugadas que le quedan para realizarse a un jugador especifico
        /// </summary>
        int CantPosiblesJugadasEsteJugador(T jugador);

        /// <summary>
        /// Devuelve el numero de jugadas que quedan para realizarse en general
        /// </summary>
        int CantPosiblesJugadas { get; }
        /// <summary>
        /// Comienza el juego desde el principio
        /// </summary>
        void Reset();

        /// <summary>
        /// devuelve un array de enteros en el cual en cada posicion (analoga a una jugada del array de jugadas posibles de cada jugador) tiene el valor de esta jugada en el juego
        /// </summary>
        /// <param name="jugador"></param>k
        /// <returns></returns>
        int[] ValoresDeLasJugadasPosiblesParaEsteJugador(T jugador);
        /// <summary>
        /// Comienza un juego con los jugadores que se le pasen
        /// </summary>
        /// <param name="jugadores"></param>
        void NuevoJuego(List<T> jugadores);

        /////Se recomienda guardar las jugadas posibles en una lista o alguna otra estructura de datos en las que a traves de un entero se pueda acceder a una jugada especifica

    }
}
