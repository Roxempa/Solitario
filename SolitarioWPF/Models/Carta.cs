
namespace Solitario.Models
{
	public enum Palo
	{
		Corazones,
		Diamantes,
		Treboles,
		Picas
	}

	public class Carta
	{
		public Palo Palo { get; set; }
		public int Valor { get; set; }
		public bool EstaBocaArriba { get; set; }

		public Carta(Palo palo, int valor)
		{
			Palo = palo;
			Valor = valor;
			EstaBocaArriba = false;
		}
	}
}
