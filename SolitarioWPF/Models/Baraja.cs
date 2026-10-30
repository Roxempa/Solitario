
using System;
using System.Collections.Generic;

namespace Solitario.Models
{
    public class Baraja
    {
        public List<Carta> Cartas { get; set; }

        public Baraja()
        {
            Cartas = new List<Carta>();

            foreach (Palo palo in Enum.GetValues(typeof(Palo)))
            {
                for (int valor = 1; valor <= 13; valor++)
                {
                    Cartas.Add(new Carta(palo, valor));
                }
            }
        }

        public void Barajar()
        {
            Random random = new Random();

            for (int i = Cartas.Count - 1; i > 0; i--)
            {
                int posicionAleatoria = random.Next(i + 1);

                Carta temporal = Cartas[i];
                Cartas[i] = Cartas[posicionAleatoria];
                Cartas[posicionAleatoria] = temporal;
            }
        }
    }
}
