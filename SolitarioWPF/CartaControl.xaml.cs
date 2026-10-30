
using System.Windows.Controls;
using System.Windows.Media;
using Solitario.Models;

namespace SolitarioWPF
{
    public partial class CartaControl : UserControl
    {
        public CartaControl(Carta carta)
        {
            InitializeComponent();

            MostrarCarta(carta);
        }

        private void MostrarCarta(Carta carta)
        {
            string valor = carta.Valor switch
            {
                1 => "A",
                11 => "J",
                12 => "Q",
                13 => "K",
                _ => carta.Valor.ToString()
            };

            string palo = carta.Palo switch
            {
                Palo.Corazones => "♥",
                Palo.Diamantes => "♦",
                Palo.Treboles => "♣",
                Palo.Picas => "♠",
                _ => ""
            };

            Brush color = carta.Palo == Palo.Corazones ||
                          carta.Palo == Palo.Diamantes
                ? Brushes.Red
                : Brushes.Black;

            TxtValor.Text = valor;
            TxtPalo.Text = palo;
            TxtPaloCentral.Text = palo;

            TxtValor.Foreground = color;
            TxtPalo.Foreground = color;
            TxtPaloCentral.Foreground = color;
        }
    }
}