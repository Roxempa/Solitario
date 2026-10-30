using Solitario.Models;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Solitario.Models;

namespace Solitario
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Partida partida = new Partida();

            partida.IniciarPartida();

            // Comprobar distribución inicial
            Debug.WriteLine("Cartas en el mazo: " + partida.Mazo.Count);
            Debug.WriteLine("Cartas en el descarte: " + partida.Descarte.Count);

            for (int i = 0; i < partida.Columnas.Count; i++)
            {
                Debug.WriteLine(
                    "Columna " + (i + 1) + ": " +
                    partida.Columnas[i].Count + " cartas"
                );
            }

            // Probar robar una carta
            partida.RobarCarta();

            Debug.WriteLine("Después de robar:");
            Debug.WriteLine("Mazo: " + partida.Mazo.Count);
            Debug.WriteLine("Descarte: " + partida.Descarte.Count);

        }
    }
}