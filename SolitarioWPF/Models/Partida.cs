using System.Collections.Generic;

namespace Solitario.Models
{
    public class Partida
    {
        public List<Carta> Mazo { get; set; }
        public List<Carta> Descarte { get; set; }
        public List<List<Carta>> Columnas { get; set; }
        public List<List<Carta>> Fundaciones { get; set; }

        public Partida()
        {
            Mazo = new List<Carta>();
            Descarte = new List<Carta>();

            Columnas = new List<List<Carta>>();

            for (int i = 0; i < 7; i++)
            {
                Columnas.Add(new List<Carta>());
            }

            Fundaciones = new List<List<Carta>>();

            for (int i = 0; i < 4; i++)
            {
                Fundaciones.Add(new List<Carta>());
            }
        }

        public void IniciarPartida()
        {
            Baraja baraja = new Baraja();
            baraja.Barajar();

            Mazo.Clear();
            Descarte.Clear();

            foreach (List<Carta> columna in Columnas)
            {
                columna.Clear();
            }

            foreach (List<Carta> fundacion in Fundaciones)
            {
                fundacion.Clear();
            }

            int posicion = 0;

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    Carta carta = baraja.Cartas[posicion];

                    if (j == i)
                    {
                        carta.EstaBocaArriba = true;
                    }

                    Columnas[i].Add(carta);
                    posicion++;
                }
            }

            for (int i = posicion; i < baraja.Cartas.Count; i++)
            {
                Carta carta = baraja.Cartas[i];
                carta.EstaBocaArriba = false;

                Mazo.Add(carta);
            }
        }

        public void RobarCarta()
        {
            if (Mazo.Count > 0)
            {
                Carta carta = Mazo[Mazo.Count - 1];

                Mazo.RemoveAt(Mazo.Count - 1);

                carta.EstaBocaArriba = true;

                Descarte.Add(carta);
            }
            else if (Descarte.Count > 0)
            {
                for (int i = Descarte.Count - 1; i >= 0; i--)
                {
                    Carta carta = Descarte[i];

                    carta.EstaBocaArriba = false;

                    Mazo.Add(carta);
                }

                Descarte.Clear();
            }
        }

        private bool EsRoja(Carta carta)
        {
            return carta.Palo == Palo.Corazones ||
                   carta.Palo == Palo.Diamantes;
        }

        public bool MoverCartas(int columnaOrigen, int indiceCarta, int columnaDestino)
        {
            if (columnaOrigen < 0 || columnaOrigen >= 7 ||
                columnaDestino < 0 || columnaDestino >= 7 ||
                columnaOrigen == columnaDestino)
            {
                return false;
            }

            List<Carta> origen = Columnas[columnaOrigen];
            List<Carta> destino = Columnas[columnaDestino];

            if (indiceCarta < 0 || indiceCarta >= origen.Count)
            {
                return false;
            }

            Carta primeraCarta = origen[indiceCarta];

            if (!primeraCarta.EstaBocaArriba)
            {
                return false;
            }

            // Comprobar que el destino permite colocar la carta.
            if (destino.Count == 0)
            {
                if (primeraCarta.Valor != 13)
                {
                    return false;
                }
            }
            else
            {
                Carta ultimaCarta = destino[destino.Count - 1];

                if (!ultimaCarta.EstaBocaArriba ||
                    EsRoja(primeraCarta) == EsRoja(ultimaCarta) ||
                    primeraCarta.Valor != ultimaCarta.Valor - 1)
                {
                    return false;
                }
            }

            // Obtener las cartas que vamos a mover.
            List<Carta> cartasMover = origen.GetRange(
                indiceCarta,
                origen.Count - indiceCarta
            );

            // Mover las cartas al destino.
            destino.AddRange(cartasMover);
            origen.RemoveRange(indiceCarta, cartasMover.Count);

            // Voltear la última carta de la columna de origen.
            if (origen.Count > 0)
            {
                origen[origen.Count - 1].EstaBocaArriba = true;
            }

            return true;
        }

        private bool PuedeColocarEnFundacion(Carta carta, int indiceFundacion)
        {
            if (indiceFundacion < 0 || indiceFundacion >= 4)
            {
                return false;
            }

            List<Carta> fundacion = Fundaciones[indiceFundacion];

            if (fundacion.Count == 0)
            {
                return carta.Valor == 1;
            }

            Carta ultimaCarta = fundacion[fundacion.Count - 1];

            return carta.Palo == ultimaCarta.Palo &&
                   carta.Valor == ultimaCarta.Valor + 1;
        }

        public bool MoverColumnaAFundacion(int columnaOrigen, int indiceFundacion)
        {
            if (columnaOrigen < 0 || columnaOrigen >= 7)
            {
                return false;
            }

            List<Carta> columna = Columnas[columnaOrigen];

            if (columna.Count == 0)
            {
                return false;
            }

            Carta carta = columna[columna.Count - 1];

            if (!carta.EstaBocaArriba ||
                !PuedeColocarEnFundacion(carta, indiceFundacion))
            {
                return false;
            }

            columna.RemoveAt(columna.Count - 1);
            Fundaciones[indiceFundacion].Add(carta);

            if (columna.Count > 0)
            {
                columna[columna.Count - 1].EstaBocaArriba = true;
            }

            return true;
        }

        public bool MoverDescarteAFundacion(int indiceFundacion)
        {
            if (Descarte.Count == 0)
            {
                return false;
            }

            Carta carta = Descarte[Descarte.Count - 1];

            if (!PuedeColocarEnFundacion(carta, indiceFundacion))
            {
                return false;
            }

            Descarte.RemoveAt(Descarte.Count - 1);
            Fundaciones[indiceFundacion].Add(carta);

            return true;
        }

        public bool MoverDescarteAColumna(int columnaDestino)
        {
            if (columnaDestino < 0 || columnaDestino >= 7)
            {
                return false;
            }

            if (Descarte.Count == 0)
            {
                return false;
            }

            Carta carta = Descarte[Descarte.Count - 1];

            List<Carta> destino = Columnas[columnaDestino];

            if (destino.Count == 0)
            {
                if (carta.Valor != 13)
                {
                    return false;
                }
            }
            else
            {
                Carta ultimaCarta = destino[destino.Count - 1];

                if (!ultimaCarta.EstaBocaArriba ||
                    EsRoja(carta) == EsRoja(ultimaCarta) ||
                    carta.Valor != ultimaCarta.Valor - 1)
                {
                    return false;
                }
            }

            Descarte.RemoveAt(Descarte.Count - 1);
            destino.Add(carta);

            return true;
        }

        public bool HaGanado()
        {
            int totalCartas = 0;

            foreach (List<Carta> fundacion in Fundaciones)
            {
                totalCartas += fundacion.Count;
            }

            return totalCartas == 52;
        }
    }
}