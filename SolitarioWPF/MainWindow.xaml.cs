using System.Windows;
using System.Windows.Controls;
using Solitario.Models;
using System.Windows.Input;

namespace Solitario
{
    public partial class MainWindow : Window
    {
        private Partida partida;
        private Carta cartaSeleccionada;
        private bool origenDescarte = false;
        private int columnaOrigen = -1;
        private int indiceCartaOrigen = -1;

        public MainWindow()
        {
            InitializeComponent();

            partida = new Partida();
            partida.IniciarPartida();

            MostrarColumnas();
            MostrarMazo();
        }

        private void MostrarColumnas()
        {
            PanelColumnas.Children.Clear();

            for (int i = 0; i < partida.Columnas.Count; i++)
            {
                StackPanel columnaVisual = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                int indice = 0;

                foreach (Carta carta in partida.Columnas[i])
                {
                    CartaControl control = new CartaControl(carta);
                    control.MouseLeftButtonDown += (sender, e) =>
                    {
                        int columnaDestino = PanelColumnas.Children.IndexOf(
                            columnaVisual
                        );

                        if (origenDescarte)
                        {
                            if (partida.MoverDescarteAColumna(columnaDestino))
                            {
                                origenDescarte = false;
                                cartaSeleccionada = null;

                                MostrarColumnas();
                                MostrarDescarte();
                                MostrarMazo();
                            }

                            e.Handled = true;
                            return;
                        }

                        if (columnaOrigen != -1 && cartaSeleccionada != null)
                        {
                            if (partida.MoverCartas(
                                columnaOrigen,
                                indiceCartaOrigen,
                                columnaDestino))
                            {
                                columnaOrigen = -1;
                                indiceCartaOrigen = -1;
                                cartaSeleccionada = null;

                                MostrarColumnas();
                            }

                            e.Handled = true;
                            return;
                        }

                        if (carta.EstaBocaArriba)
                        {
                            columnaOrigen = columnaDestino;
                            indiceCartaOrigen = columnaVisual.Children.IndexOf(control);
                            cartaSeleccionada = carta;
                        }

                        e.Handled = true;
                    };

                    // Superponer las cartas dejando visible parte de cada una.
                    control.Margin = new Thickness(
                        0,
                        indice == 0 ? 0 : -95,
                        0,
                        0
                    );

                    columnaVisual.Children.Add(control);

                    indice++;
                }

                PanelColumnas.Children.Add(columnaVisual);
            }
        }
        private void MostrarMazo()
        {
            TxtCartasMazo.Text = partida.Mazo.Count.ToString();

            if (partida.Mazo.Count == 0)
            {
                TxtCartasMazo.Text = "↻";
            }
        }
        private void MostrarDescarte()
        {
            PanelDescarte.Content = null;

            if (partida.Descarte.Count > 0)
            {
                Carta carta = partida.Descarte[
                    partida.Descarte.Count - 1
                ];

                CartaControl control = new CartaControl(carta);

                control.MouseLeftButtonDown += Descarte_MouseLeftButtonDown;

                PanelDescarte.Content = control;
            }
        }
        private void Descarte_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (partida.Descarte.Count == 0)
                return;

            cartaSeleccionada = partida.Descarte[
                partida.Descarte.Count - 1
            ];

            origenDescarte = true;
        }
        private void Mazo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            partida.RobarCarta();

            MostrarMazo();
            MostrarDescarte();
        }
    }
}