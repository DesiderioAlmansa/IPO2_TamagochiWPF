using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TamagochiV2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        DispatcherTimer t1;
        SoundPlayer Player = new SoundPlayer();
        MediaPlayer mediaPlayer = new MediaPlayer();

        double incremento = 5.0;
        double decremento = 0.1;
        int puntos;

        Boolean pasa1 = false;
        Boolean pasa2 = false;
        Boolean pasa3 = false;
        Boolean pasa4 = false;
        Boolean pasa5 = false;
        

        string nombre;

        public MainWindow()
        {

            InitializeComponent();
            VentanaBienvenida pantallaInicio = new VentanaBienvenida(this);
            pantallaInicio.ShowDialog();
            t1 = new DispatcherTimer();
            t1.Interval = TimeSpan.FromMilliseconds(1000);
            t1.Tick += new EventHandler(reloj);
            t1.Start();

            this.tbInformacion.Text = "Bienvenido al juego " + nombre;
            leerFicheroRanking();

        }



        private void reloj(object sender, EventArgs e)
        {
            puntos += DateTime.Now.Second;
            logros(puntos);
            lblPuntuacionActual.Content = "PUNTOS : " + puntos;

            this.pbHambre.Value -= (decremento + 0.5);
            this.pbEnergia.Value -= decremento + 0.2;
            this.pbDiversion.Value -= (decremento + 0.7);
            decremento += 0.1;
            if(decremento <= 5.0)
            {
                incremento += 0.1;
            }

            if (this.pbDiversion.Value <= 30 && this.pbHambre.Value <= 30 && this.pbEnergia.Value <= 30)
            {
                this.tbInformacion.Text = "Groot esta muy mal. Haz algo.";
                bocaSeria.Visibility = Visibility.Visible;
                parpado1.Visibility = Visibility.Visible;
                parpado2.Visibility = Visibility.Visible;
                lengua.Visibility = Visibility.Hidden;
                boca.Visibility = Visibility.Hidden;
                

                Storyboard sbSerio = (Storyboard)this.Resources["eventoSerio"];
                sbSerio.Begin();
            }
            else
            {
                bocaSeria.Visibility = Visibility.Hidden;
                parpado1.Visibility = Visibility.Hidden;
                parpado2.Visibility = Visibility.Hidden;
                lengua.Visibility = Visibility.Visible;
                boca.Visibility = Visibility.Visible;
            }

            if (this.pbDiversion.Value <= 30)
            {
                this.tbInformacion.Text = "Groot se aburre. Ponle un poco de musica para que se divierta.";
            }
            else if (this.pbHambre.Value <= 30)
            {
                this.tbInformacion.Text = "Groot tiene hambre. Dale una galleta.";
            }
            else if (this.pbEnergia.Value <= 30)
            {
                this.tbInformacion.Text = "Groot tiene sueño. Haz que descanse un poco.";
            }
                
            

            if (this.pbDiversion.Value == 0 || this.pbHambre.Value == 0 || this.pbEnergia.Value == 0)
            {

                Player.SoundLocation = "Musica/gameOver.wav";
                Player.Play();

                //To Do: Finalizar juego si alguna barra llega a cero
                this.tbInformacion.Text = "GROOT HA MUERTO Y SE HA MARCHITADO. FIN DEL JUEGO";
                tbLogros.Text = "";
                parpado1.Visibility = Visibility.Hidden;
                parpado2.Visibility = Visibility.Hidden;
                bocaSeria.Visibility = Visibility.Hidden;
                boca.Visibility = Visibility.Visible;
                lengua.Visibility = Visibility.Visible;
                pupilaDerecha.Visibility = Visibility.Hidden;
                pupilaIzquierda.Visibility = Visibility.Hidden;
                ojoMuerto1.Visibility = Visibility.Visible;
                ojoMuerto2.Visibility = Visibility.Visible;
                Storyboard sbFinJuego = (Storyboard)this.Resources["eventoFinJuego"];
                sbFinJuego.Completed += new EventHandler(finAnimacionFINGJUEGO);
                sbFinJuego.Begin();

                //HACER: guardar puntuacion en excel y hacer que se muestre en ranking si es necesario
                escribirJuador(nombre, puntos);

                //To Do: Dar puntuación según número de segundos sobrevividos
                MessageBoxResult resultado = MessageBox.Show("HAS CONSEGUIDO UN TOTAL DE " + puntos + " PUNTOS. \n¿DESEAS JUGAR DE NUEVO COMO " + nombre + "?", "GAME OVER", MessageBoxButton.YesNo);
                switch (resultado)
                {
                    case MessageBoxResult.Yes:
                        this.mediaPlayer.Stop();
                        leerFicheroRanking();
                        decremento = 0.5;
                        parpado1.Visibility = Visibility.Visible;
                        parpado2.Visibility = Visibility.Visible;
                        bocaSeria.Visibility = Visibility.Visible;
                        boca.Visibility = Visibility.Hidden;
                        lengua.Visibility = Visibility.Hidden;
                        pupilaDerecha.Visibility = Visibility.Visible;
                        pupilaIzquierda.Visibility = Visibility.Visible;
                        ojoMuerto1.Visibility = Visibility.Hidden;
                        ojoMuerto2.Visibility = Visibility.Hidden;

                        imPremio1.Visibility = Visibility.Hidden;
                        imPremio2.Visibility = Visibility.Hidden;
                        imPremio3.Visibility = Visibility.Hidden;
                        imPremio4.Visibility = Visibility.Hidden;
                        imPremio5.Visibility = Visibility.Hidden;
                        imPremioEasterEgg.Visibility = Visibility.Hidden;

                        this.pasa1 = false;
                        this.pasa2 = false;
                        this.pasa3 = false;
                        this.pasa4 = false;
                        this.pasa5 = false;

                        Storyboard sbVuelveJugar = (Storyboard)this.Resources["eventoVuelveAJugar"];
                        sbVuelveJugar.Begin();

                        puntos = 0;

                        this.pbDiversion.Value = 70;
                        this.pbHambre.Value = 70;
                        this.pbEnergia.Value = 70;

                        this.tbInformacion.Text = "Bienvenido de nuevo al juego " + nombre;
                       
                        t1 = new DispatcherTimer();
                        t1.Interval = TimeSpan.FromMilliseconds(1000);
                        t1.Tick += new EventHandler(reloj);
                        t1.Start();
                        break;

                    case MessageBoxResult.No:
                        this.Close();
                        break;
                }


            }



        }

        private void finAnimacionFINGJUEGO(object sender, EventArgs e)
        {
            t1.Stop();
        }

        private void cambiarFondo(object sender, MouseButtonEventArgs e)
        {
            this.imgFondoPaisaje.Source = ((Image)sender).Source;
        }

        private void btnComer_Click(object sender, RoutedEventArgs e)
        {
            btnDormir.IsEnabled = false;
            btnComer.IsEnabled = false;
            btnJugar.IsEnabled = false;

            boca.Visibility = Visibility.Visible;
            lengua.Visibility = Visibility.Visible;
            bocaSeria.Visibility = Visibility.Hidden;

            path.Visibility = Visibility.Visible;
            path1.Visibility = Visibility.Visible;
            path2.Visibility = Visibility.Visible;
            path3.Visibility = Visibility.Visible;
            path4.Visibility = Visibility.Visible;
            path5.Visibility = Visibility.Visible;
            path6.Visibility = Visibility.Visible;
            path7.Visibility = Visibility.Visible;
            path8.Visibility = Visibility.Visible;
            path9.Visibility = Visibility.Visible;
            path10.Visibility = Visibility.Visible;

            Player.SoundLocation = "Musica/comer.wav";
            Player.Play();

            this.pbHambre.Value += (incremento + 10);

            Storyboard sbComer = (Storyboard)this.Resources["eventoComer"];
            sbComer.Completed += new EventHandler(finAnimacion);
            sbComer.Completed += new EventHandler(quitarGalleta);
            sbComer.Begin();

        }

        private void eventoDescansar(object sender, RoutedEventArgs e)
        {
            btnDormir.IsEnabled = false;
            btnComer.IsEnabled = false;
            btnJugar.IsEnabled = false;

            pupilaDerecha.Visibility = Visibility.Hidden;
            pupilaIzquierda.Visibility = Visibility.Hidden;

            z1.Visibility = Visibility.Visible;
            z2.Visibility = Visibility.Visible;
            z3.Visibility = Visibility.Visible;

            Player.SoundLocation = "Musica/dormir.wav";
            Player.Play();

            this.pbEnergia.Value += (incremento+8);

            Storyboard sbDormir = (Storyboard)this.Resources["eventoDormir"];
            sbDormir.Completed += new EventHandler(finAnimacion);
            sbDormir.Completed += new EventHandler(quitarZs);
            sbDormir.Begin();


        }

        private void quitarZs(object sender, EventArgs e)
        {

            pupilaDerecha.Visibility = Visibility.Visible;
            pupilaIzquierda.Visibility = Visibility.Visible;

            z1.Visibility = Visibility.Hidden;
            z2.Visibility = Visibility.Hidden;
            z3.Visibility = Visibility.Hidden;

        }

        private void finAnimacion(object sender, EventArgs e)
        {
            btnDormir.IsEnabled = true;
            btnComer.IsEnabled = true;
            btnJugar.IsEnabled = true;
        }

        private void quitarGalleta(object sender, EventArgs e)
        {
            path.Visibility = Visibility.Hidden;
            path1.Visibility = Visibility.Hidden;
            path2.Visibility = Visibility.Hidden;
            path3.Visibility = Visibility.Hidden;
            path4.Visibility = Visibility.Hidden;
            path5.Visibility = Visibility.Hidden;
            path6.Visibility = Visibility.Hidden;
            path7.Visibility = Visibility.Hidden;
            path8.Visibility = Visibility.Hidden;
            path9.Visibility = Visibility.Hidden;
            path10.Visibility = Visibility.Hidden;
        }

        private void acercaDe(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("Programa realizado por: \n\n\tDesiderio Almansa Porrero", "Acerca de...", MessageBoxButton.OK);

            switch (resultado)
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }

        private void btnJugar_Click(object sender, RoutedEventArgs e)
        {
            btnDormir.IsEnabled = false;
            btnComer.IsEnabled = false;
            btnJugar.IsEnabled = false;

            pupilaDerecha.Visibility = Visibility.Hidden;
            pupilaIzquierda.Visibility = Visibility.Hidden;

            ojoFelizDer.Visibility = Visibility.Visible;
            ojoFelizIzq.Visibility = Visibility.Visible;

            imgWalkmanStarLord.Visibility = Visibility.Visible;
            imgMusica.Visibility = Visibility.Visible;

            Player.SoundLocation = "Musica/jugar.wav";
            Player.Play();

            this.pbDiversion.Value += (incremento + 6);

            Storyboard sbJugar = (Storyboard)this.Resources["eventoJugar"];
            sbJugar.Completed += new EventHandler(finAnimacion);
            sbJugar.Completed += new EventHandler(quitarOjosFeliz);
            sbJugar.Begin();

        }

        private void quitarOjosFeliz(object sender, EventArgs e)
        {
            pupilaDerecha.Visibility = Visibility.Visible;
            pupilaIzquierda.Visibility = Visibility.Visible;

            ojoFelizDer.Visibility = Visibility.Hidden;
            ojoFelizIzq.Visibility = Visibility.Hidden;

            imgWalkmanStarLord.Visibility = Visibility.Hidden;
            imgMusica.Visibility = Visibility.Hidden;


        }


        private void inicioArrastrarColeccionable(object sender, MouseButtonEventArgs e)
        {
            DataObject dataO = new DataObject(((Image)sender));
            DragDrop.DoDragDrop((Image)sender, dataO, DragDropEffects.Move);
        }

        private void colocarColeccionable(Object sender, DragEventArgs e)
        {
            Image aux = (Image)e.Data.GetData(typeof(Image));
            switch (aux.Name)
            {
                case "imgColeccionableLokiCasco":
                    imgLokiCasco.Visibility = Visibility.Visible;
                    break;

                case "coleccionable_Stormbreaker":
                    imgStormbreaker.Visibility = Visibility.Visible;
                    break;

                case "coleccionable_GuanteDelInfinito":
                   imgGuanteDelInfinito.Visibility = Visibility.Visible;
                    break;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            imgLokiCasco.Visibility = Visibility.Hidden;
            imgStormbreaker.Visibility = Visibility.Hidden;
            imgGuanteDelInfinito.Visibility = Visibility.Hidden;
        }

        private void logros(int puntos)
        {
            if (puntos >= 250 && pasa1 == false)
            {
                sonidoLogro();
                tbLogros.Text += "\r\n > Consigue 250pt.";
                this.pasa1 = true;
                imPremio1.Visibility = Visibility.Visible;
            }  

            if (puntos >= 400 && pasa2 == false)
            {
                sonidoLogro();
                tbLogros.Text += "\r\n > Consigue 400pt.";
                this.pasa2 = true;
                imPremio2.Visibility = Visibility.Visible;
            }

            if (puntos >= 500 && pasa3 == false)
            {
                sonidoLogro();
                tbLogros.Text += "\r\n > Consigue 500pt.";
                this.pasa3 = true;
                imPremio3.Visibility = Visibility.Visible;
            }

            if (puntos >= 800 && pasa4 == false)
            {
                sonidoLogro();
                tbLogros.Text += "\r\n > Consigue 800pt.";
                this.pasa4 = true;
                imPremio4.Visibility = Visibility.Visible;
            }

            if (puntos >= 1100 && pasa5 ==false)
            {
                sonidoLogro();
                tbLogros.Text += "\r\n > Consigue 1100pt.";
                this.pasa5 = true;
                imPremio5.Visibility = Visibility.Visible;
            }
        }

        private void easterEgg(object sender, MouseButtonEventArgs e)
        {
            sonidoLogro();
            tbLogros.Text += "\r\n > EasterEgg MARVEL.";
            imPremioEasterEgg.Visibility = Visibility.Visible;
        }

        private void sonidoLogro()
        {
            SoundPlayer Player2 = new SoundPlayer();
            Player2.SoundLocation = "Musica/logro.wav";
            Player2.Play();
        }

        private void musica1(object sender, MouseButtonEventArgs e)
        {
            this.mediaPlayer.Stop();
            this.mediaPlayer.Open(new Uri("Musica/premio1.wav", UriKind.Relative));
            this.mediaPlayer.Play();
        }

        private void musica2(object sender, MouseButtonEventArgs e)
        {
            this.mediaPlayer.Stop();
            this.mediaPlayer.Open(new Uri("Musica/premio2.wav", UriKind.Relative));
            this.mediaPlayer.Play();
        }

        private void musica3(object sender, MouseButtonEventArgs e)
        {
            this.mediaPlayer.Stop();
            this.mediaPlayer.Open(new Uri("Musica/premio3.wav", UriKind.Relative));
            this.mediaPlayer.Play();
        }

        private void musica4(object sender, MouseButtonEventArgs e)
        {
            this.mediaPlayer.Stop();
            this.mediaPlayer.Open(new Uri("Musica/premio4.wav", UriKind.Relative));
            this.mediaPlayer.Play();
        }

        private void musica5(object sender, MouseButtonEventArgs e)
        {
            this.mediaPlayer.Stop();
            this.mediaPlayer.Open(new Uri("Musica/premio5.wav", UriKind.Relative));
            this.mediaPlayer.Play();
        }

        private void musicaEasterEgg(object sender, MouseButtonEventArgs e)
        {
            this.mediaPlayer.Stop();
            this.mediaPlayer.Open(new Uri("Musica/premioEasterEgg.wav", UriKind.Relative));
            this.mediaPlayer.Play();
        }

        private void escribirJuador(string n, int p)
        {
            using (StreamWriter file = new StreamWriter("ranking.csv", true))
            {
                var line = string.Format("{0} ; {1}", n, p);
                file.WriteLine(line);
                file.Close();
            }
        }
        private void leerFicheroRanking()
        {
            
            List<string> nombres = new List<string>();//nombres
            List<string> puntuaciones = new List<string>(); //puntuacion
            using (var file = new StreamReader("ranking.csv"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    nombres.Add(line.Split(';')[0]);
                    puntuaciones.Add(line.Split(';')[1]);
                }
            }

            cargarRanking(nombres, puntuaciones);

        }

        private void cargarRanking(List<string> nombres, List<string> puntuaciones)
        {
            ordenarRanking(nombres, puntuaciones);

            nombre1.Text = $"{nombres.ElementAt(0)}" + "   " + $"{puntuaciones.ElementAt(0)}";
            nombre2.Text = $"{nombres.ElementAt(1)}" + "   " + $"{puntuaciones.ElementAt(1)}";
            nombre3.Text = $"{nombres.ElementAt(2)}" + "   " + $"{puntuaciones.ElementAt(2)}";
            nombre4.Text = $"{nombres.ElementAt(3)}" + "   " + $"{puntuaciones.ElementAt(3)}";
            nombre5.Text = $"{nombres.ElementAt(4)}" + "   " + $"{puntuaciones.ElementAt(4)}";
            nombre6.Text = $"{nombres.ElementAt(5)}" + "   " + $"{puntuaciones.ElementAt(5)}";

        }

        private void ordenarRanking(List<string> nombres, List<string> puntuaciones)
        {
            string nombreaux;
            string puntosaux;
            int i, j;

            for (i = 0; i<nombres.Count;i++)
            { 
                for(j = 0; j< nombres.Count-i-1; j++)
                {
                    if (Int32.Parse(puntuaciones.ElementAt(j+1)) > Int32.Parse(puntuaciones.ElementAt(j)))
                    {
                        puntosaux = puntuaciones.ElementAt(j + 1);
                        nombreaux = nombres.ElementAt(j + 1);

                        puntuaciones[j + 1] = puntuaciones.ElementAt(j);
                        nombres[j + 1] = nombres.ElementAt(j);

                        puntuaciones[j] = puntosaux;
                        nombres[j] = nombreaux;

                    }
                }

            }

        }
    }
}
