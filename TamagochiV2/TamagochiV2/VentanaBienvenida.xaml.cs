using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TamagochiV2
{
    /// <summary>
    /// Lógica de interacción para VentanaBienvenida.xaml
    /// </summary>
    public partial class VentanaBienvenida : Window
    {
        MainWindow padre;
        public VentanaBienvenida(MainWindow padre)
        {
            InitializeComponent();
            this.padre = padre;
            MessageBox.Show("Bienvenido a TAMAGROOT. \n\n - Para jugar, debes mantener a Groot contento dandole de comer, durmiendolo o " +
                "haciendo que baile. \n\n - Al comenzar dispondras unicamente de los coleccionables (fondos y accesorios). " +
                "\n\n - Al ganar logros recibiras como premio música que podras reproducir mientras juegas. \n\n ¡DIVIERTETE!", "INSTRUCCIONES", MessageBoxButton.OK);
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (tbNombre.Text.Length > 8 || tbNombre.Text.Length == 0)
            {
                MessageBoxResult resultado = MessageBox.Show("El nombre debe tener entre 1 y 8 carácteres. \nIntentelo de nuevo.", "Error", MessageBoxButton.OK);
              
            }
            else
            {
                padre.setNombre(tbNombre.Text);
                this.Close();
            } 
        }

    }
}
