using System;
using System.Collections.Generic;
using IJuego;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tic_Tac_Toe
{
    public class TicTacToe<T> : IGame<T>
    {
        #region campos
        /// <summary>
        /// Jugadores de el presente juego
        /// </summary>
        private List<T> jugadores;// { get; set; }
        /// <summary>
        /// guarda los valores del tablero(aqui se van actualizando)
        /// </summary>
        private T[,] board;
        /// <summary>
        /// Lista que guarda tuplas de los lugares del tablero que estan disponibles
        /// </summary>
        private List<Tuple<int, int>> posiblesJugadas;
        /// <summary>
        /// En el caso de haberse terminado el juego, gurda al ganador
        /// </summary>
        private T winner;
        /// <summary>
        /// Denota si el juego ya acabo
        /// </summary>
        private bool fin;
        /// <summary>
        /// Denota si el turno correspoonde al jugador 1 o al jugador 2
        /// </summary>
        private int turno;
        /// <summary>
        /// Denota si el juego termino en empate
        /// </summary>
        private bool empate;
        /// <summary>
        /// Guarda las puntuaciones de los jugadores
        /// </summary>
        private Dictionary<T, int> puntuaciones;
        /// <summary>
        /// Devuelve la deescripcion de la ultima jugada realizada
        /// </summary>
        private string ultimaJugada;
        /// <summary>
        /// Cuando se termina el juego, guarda los resultados de este
        /// </summary>
        private string resultados;

        #endregion

        #region interface

        #region Propiedades       
        public T[,] Board { get { return board; } }//No pertenece a la interface
        public List<Tuple<int, int>> PosiblesJugadas {  get { return posiblesJugadas; } }//No pertenece a la interface
        public int MaxCantidadDeJugadoresPorEquipo
        {
            get
            {
                throw new Exception("El juego no permite equipos");
            }
        }
        public Dictionary<T, int> Puntuaciones
        {
            get
            {
                if (JuegoFinalizado) return puntuaciones;
                throw new InvalidOperationException("Al no haberse terminado el juego no se pueden asignar puntuaciones");
            }
        }
        public int CantPosiblesJugadas
        { get { return posiblesJugadas.Count; } }
        public bool Finalizado { get { return fin; } }
        public string Resultados
        {
            get
            {
                if (fin) return resultados;
                throw new InvalidOperationException("El juego aun no ha finalizado");
            }
        }
        public string UltimaJugada { get { return ultimaJugada; } }
        public bool JuegoPorPuntuaciones { get { return false; } }
        public List<T> Jugadores { get { return jugadores; } }
        public int CantidadDePuntosPartida { get { return 5; } }
        public int MaxCantidadPlayers { get { return 2; } }
        public int MinCantPlayers { get { return 2; } }
        public bool PermiteEquipos { get { return false; } }
        public bool Empatable { get { return true; } }
        public bool JuegoPorPartidas { get { return true; } }
        public bool JuegoFinalizado { get { return fin; } }
        public bool JuegoPorPuntos { get { return false; } }
        public T Winner
        {
            get
            {
                if (empate) throw new Exception("No hay ganador porque el juego quedo empatado");
                if (fin) return winner;
                else throw new InvalidOperationException("Aun no hay un ganador porque el juego no ha acabado.");
            }
        }
        public int MaximoDeJugadoresPorEquipo { get { throw new Exception("Este juego no permite equipos"); } }


        #endregion

        #region Metodos
        public void NuevoJuego(List<T> jugadores)
        {
            this.jugadores = jugadores;
            Reset();
        }
        public void Reset()
        {
            resultados = "";
            board = new T[3, 3];
            fin = false;
            winner = default(T);
            posiblesJugadas = new List<Tuple<int, int>>();
            turno = 0;
            empate = false;
            puntuaciones = new Dictionary<T, int>();
            ultimaJugada = "";
            CreaPuntuaciones();
            AgregaJugadas();
        }
        private void CreaPuntuaciones()
        {
            foreach (var jugador in jugadores)
            {
                puntuaciones.Add(jugador, 0);
            }
        }
        public int CantPosiblesJugadasEsteJugador(T jugador)
        {
            if (JuegoFinalizado) return 0;
            return posiblesJugadas.Count;
        }
        public int[] ValoresDeLasJugadasPosiblesParaEsteJugador(T jugador)
        {
            int[] valores = new int[CantPosiblesJugadasEsteJugador(jugador)];

            for (int i = 0; i < valores.Length; i++)
            {
                valores[i] = EvaluaJugada(posiblesJugadas[i], jugador);
            }

            return valores;

        }
        public void Juega(int jugada, T jugador)
        {
            int x = posiblesJugadas[jugada].Item1;
            int y = posiblesJugadas[jugada].Item2;
            board[x, y] = jugador;
            posiblesJugadas.RemoveAt(jugada);
            string marca = turno++ % 2 == 0 ? "X" : "0";
            JuegoTerminado();
            ultimaJugada = $"El jugador {jugador} marco {marca} en la casilla {x},{y}";
            if (fin)
            {
                if (empate)
                {
                    resultados = "El juego ha quedado empatado";
                }
                else resultados = resultados + $"El jugador {winner} ha ganado el juego";
                foreach (var p in jugadores)
                {
                    int n = puntuaciones[p];
                    string s = (n == 1) ? "punto" : "puntos";
                    resultados = resultados + "\n" + $"El jugador {p} ha ganado {n} {s}";
                }
            }
        }
        public bool PartidaTerminada(IPartida partida)
        {
            if (empate)
            {
                if (partida.PermiteEmpate) return true;
                if (partida.CantidadDeJuegosJugados >= 10) return true;
                return false;
            }
            return true;

        }
        #endregion

        #endregion


        #region Propio
        /// <summary>
        /// Llena la lista de posiciones vacias del tablero
        /// </summary>
        private void AgregaJugadas()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (Equals(board[i, j], default(T)))
                    {
                        posiblesJugadas.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
        }
        /// <summary>
        /// Busca si hay tres marcas del mismo jugador en linea, si las hay devuelve el jugador, que sera el ganador, de no haberlo devuelve el valor por defecto de T
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private T TresEnlinea(T[,] board)
        {

            //int[] dr = { -1, -1, 0, 1, 1, 1, 0, -1 };
            //int[] dc = {  0, 1, 1, 1, 0, -1, -1, -1 };

            int[] dr = { -1, -1, 0, 1, };//1, 1, 0, -1 };
            int[] dc = { 0, 1, 1, 1 };//, 0, -1, -1, -1 };

            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                    if (!Equals(board[i, j], default(T)))
                        for (int d = 0; d < dr.Length; d++)
                            if (BuscaEnLinea(board, i, j, dr[d], dc[d]) >= 3)
                                return (T)board[i, j];
            return default(T);
        }
        /// <summary>
        /// Devuelve la maxima cantidad de un mismo tipo que hay en linea, comenzando por la posicion i,j y siguiendo la direccionn dr,dc
        /// </summary>
        /// <param name="board"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="dr"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        private int BuscaEnLinea(T[,] board, int i, int j, int dr, int dc)
        {
            int count = 1;
            //while (EnRango(board, i + count * dr, j + count * dc) && Equals(board[i, j], board[i + count * dr, j + count * dc]))
            //    count++;


            while  (EnRango(board, i + count * dr, j + count * dc))
            {
                if (Equals(board[i, j], board[i + count * dr, j + count * dc])) count++;
                else break;
            }
            return count;
        }
        /// <summary>
        /// Denota si la posicion r,c esta en el rango del tablero
        /// </summary>
        /// <param name="board"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool EnRango(T[,] board, int r, int c)
        {
            return r >= 0 && r < board.GetLength(0) && c >= 0 && c < board.GetLength(1);
        }
        
        /// <summary>
        /// Revisa si el juego ha terminado, actualizando la variable fin
        /// </summary>
        private void JuegoTerminado()
        {
            var ganador = TresEnlinea(board);
            if (Equals(ganador, default(T)))///Comprueba si algun jugador ha ganado
            {
                if (CantPosiblesJugadas > 0) return;
                if (CantPosiblesJugadas == 0)///Si no quedan jugadas y no hay un ganador, el juego termina en empate
                {
                    fin = true;
                    empate = true; winner = default(T);
                    //foreach (var item in jugadores)
                    //{
                    //    puntuaciones[item] += 1;
                    //}
                }
                //return;
            }
            fin = true;
            winner = ganador;
            ActualizaPuntuaciones();
            //puntuaciones[winner] += 2;///Actualiza la puntuacion del ganador
        }

        /// <summary>
        /// Guarda en un diccionario las puntuaciones obtenidas por cada jugador(1 punto para cada uno si hay empate o 2 puntos al ganador y 0 al perdedor)
        /// </summary>
        private void ActualizaPuntuaciones()
        {
            if (empate)
            {
                foreach (var x in jugadores)
                {
                    puntuaciones[x] += 1;
                }
                return;
            }
            puntuaciones[winner] += 2;
        }
        /// <summary>
        /// En este metodo se devuelve el valor que tiene una jugada especifica para un jugador especufico, se devuelve 2 si gana este jugador, se devuelve 1 si evita que el oponente gane y 0 si no trae cambios a corto plazo
        /// <param name="jugada"></param>
        /// <param name="jugador"></param>
        /// <returns></returns>
        private int EvaluaJugada(Tuple<int, int> jugada, T jugador)
        {
            if (jugador == null) return int.MinValue;
            T oponente;
            if (jugador.Equals(jugadores[0])) oponente = jugadores[1];
            else oponente = jugadores[0];
            board[jugada.Item1, jugada.Item2] = jugador;
            if (Equals(TresEnlinea(board), jugador)) { board[jugada.Item1, jugada.Item2] = default(T); return 2; }
            board[jugada.Item1, jugada.Item2] = oponente;
            if (Equals(TresEnlinea(board), oponente)) { board[jugada.Item1, jugada.Item2] = default(T); return 1; }
            board[jugada.Item1, jugada.Item2] = default(T);
            return 0;
        }
        #endregion
        public TicTacToe<T> GetCopy()
        {
            return new TicTacToe<T>();
        }
        public override string ToString()
        {
            return "Tic Tac Toe";
        }
    }
}
