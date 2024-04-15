using System; // Librería principal.
using System.Collections.Generic; // Librería genérica de colecciones.
using System.ComponentModel; // Librería de los componentes de la consola.
using System.Data; // Librería de datos.
using System.Drawing; // Librería de dibujo.
using System.Linq; // Librería lineal.
using System.Text; // Librería de textos.
using System.Threading.Tasks; // Librería de paralelismo y concurrencia.
using System.Windows.Forms; // Librería de Windows Forms.

namespace Breakout_Game_APP_MOO_ICT
{
    // Clase Parcial de toda su interfaz gráfica.
    public partial class Form1 : Form
    {
        // Variables booleanas.

        bool goLeft; // Si es que va hacia la izquierda.
        bool goRight; // Si es que va hacia la derecha.
        bool isGameOver; // Si es que terminó la partida.

        // Variables numéricas enteras.

        int score; // Puntuación total.
        int ballx; // Ancho de la bola.
        int bally; // Altura de la bola.
        int playerSpeed; // Velocidad del jugador.

        Random rnd = new Random(); // Variable que puede realizar cualquier modificación de manera aleatoria.

        PictureBox[] blockArray; // Bloques mediante una lista de arreglos.

        // Método inicial.
        public Form1()
        {
            InitializeComponent(); // Inicializa todos los componentes del juego.
            PlaceBlocks(); // Llamado del método privado.
        }

        // Método de ajustes a los componentes del juego.
        private void setupGame()
        {
            isGameOver = false;
            score = 0; // Puntuación inicial del juego.
            ballx = 5; // Tamaño inicial de la bola.
            bally = 5; // Altura inicial de la bola.
            playerSpeed = 12; // Velocidad inicial del jugador.
            txtScore.Text = "Score: " + score;

            // Definimos la posición inicial de la bola.

            ball.Left = 353;
            ball.Top = 334;

            // Definimos la posición inicial de la plataforma.

            player.Left = 316;
            player.Top = 502;

            // Se inicializa con contador de tiempo.

            gameTimer.Start();

            // Crearemos un ciclo foreach para los bloques de colores.

            foreach(Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks") // Si son bloques.
                {
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)); // Ahora le agregaremos colores aleatorios a los bloques del juego.
                }
            }
        }

        // Crearemos otro método privado para desplegar un mensaje que finalizó del juego.

        private void gameOver(string message)
        {
            isGameOver = true; // Terminó el juego.
            gameTimer.Stop(); // Paraliza el contador o se acaba el tiempo en este método.

            txtScore.Text = "Score: " + score + " " + message; // Se imprime un mensaje mediante texto con un string.
        }

        // Método que permite ordenar los bloques de manera eficiente.

        private void PlaceBlocks()
        {
            blockArray = new PictureBox[15]; // Se añade automáticamente la cantidad de bloques (PictureBox) mediante arreglos.

            // Declaramos las variables por defecto para el juego.

            int a = 0;
            int top = 50;
            int left = 100;

            // Utilizaremos el ciclo for pra definir las dimensiones de los bloques.

            for (int i = 0; i < blockArray.Length; i++)
            {
                blockArray[i] = new PictureBox(); // Se añade bloques de tipo PictureBox en el juego.
                blockArray[i].Height = 32; // Altura máxima del ordenamiento de bloques.
                blockArray[i].Width = 100; // Ancho máximo del ordenamiento de bloques.
                blockArray[i].Tag = "blocks"; // Se define como blocks a los bloques ordenados.
                blockArray[i].BackColor = Color.White; // Los bloques por defecto serán blancos.

                // Haremos un recorrido completo con el if para que la bola pueda colisionar directamente con los bloques.

                if (a == 5) // Si el límite del juego es igual a 5.
                {
                    top = top + 50;
                    left = 100;
                    a = 0;
                }

                if (a < 5) // Si el límite del juego es menor a 5.
                {
                    a++;
                    blockArray[i].Left = left;
                    blockArray[i].Top = top;
                    this.Controls.Add(blockArray[i]);
                    left = left + 130;
                }
            }
            setupGame();
        }

        // Crearemos otro método para eliminar de manera más detallada los bloques mediante arreglos.

        private void removeBlocks()
        {
            foreach (PictureBox x in blockArray)
            {
                this.Controls.Remove(x);
            }
        }

        // Método privado de un evento ejecutable con temporizador.
        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score; // Suma 1 punto cuando choca con cualquier bloque.

            if (goLeft == true && player.Left > 0) // Desde una distancia mayor que 0 hacia la izquierda.
            {
                player.Left -= playerSpeed; // Se descuenta la velocidad y va hacia la izquierda.
            }

            if (goRight == true && player.Right < 700) // Desde una distancia menor que 700 hacia la derecha.
            {
                player.Left += playerSpeed; // Se incrementa la velocidad y va hacia la derecha.
            }

            // Variables para que la pelota pueda rebotar sobre la plataforma hacia ambos lados.

            ball.Left += ballx;
            ball.Top += bally;

            // Crearemos una condición if para que la bola se mueva en un intervalo < 0 y > 775.

            if (ball.Left < 0 || ball.Left > 775) // Para la bola que se mueve de ambos lados con la plataforma.
            {
                ballx = -ballx;
            }

            // Nuevamente crearemos otro if para que pueda rebotar la bola sobre la plataforma.

            if (ball.Top < 0)
            {
                bally = -bally;
            }

            // Crearemos otro if para que la bola pueda rebotar por sobre la plataforma.

            if (ball.Bounds.IntersectsWith(player.Bounds)) // Si la bola choca por sobre la plataforma.
            {
                bally = rnd.Next(5, 12) * -1;

                // Crearemos otro método para hacer que la bola se mueva rápido por sobre la plataforma.

                if (ballx < 0)
                {
                    ballx = rnd.Next(5, 12) * -1;
                }

                // En caso contrario...

                else
                {
                    ballx = rnd.Next(5, 12);
                }
            }

            // Crearemos otro ciclo foreach para que cada bloque pueda ser removido al chocar con la bola.

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks") // Si son bloques de colores aleatorios.
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds)) // Si choca con algunos bloques de colores.
                    {
                        score += 1; // Suma 1 punto.

                        bally = - bally; // Dependiendo de la altura de la bola según la fuerza de gravedad al chocar con los bloques.

                        this.Controls.Remove(x); // Elimina todos los bloques de colores.
                    }


                }
            }

            // Crearemos otro if para mayores detalles tras el inicio del videojuego.

            if (score == 15) // Si el puntaje es igual a 15.
            {
                gameOver("Has ganado la partida! :) Presiona ENTER para continuar"); // Llama al método gameOver diciendo que ganó la partida.
            }

            if (ball.Top > 580) // Si la bola alcanzó el límite inferior del juego.
            {
                gameOver("Lo siento, perdió la partida! :( Presiona ENTER para jugar otra vez"); // Llama al método gameOver diciendo que perdió la partida.
            }
        }

        // Método privado de la tecla hacia abajo.
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left) // Si la tecla presiona hacia la izquierda.
            {
                goLeft = true; // Va hacia la izquierda.
            }

            if (e.KeyCode == Keys.Right) // Si la tecla presiona hacia la derecha.
            {
                goRight = true; // Va hacia la derecha.
            }
        }

        // Método privado de la tecla hacia arriba.
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) // Si la tecla presiona hacia la izquierda.
            {
                goLeft = false; // No va hacia la izquierda.
            }

            if (e.KeyCode == Keys.Right) // Si la tecla presiona hacia la derecha.
            {
                goRight = false; // No va hacia la derecha.
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true) // Si la tecla presiona ENTER cuando acaba el juego.
            {
                removeBlocks(); // Llamado del método anterior para eliminar bloques.
                PlaceBlocks(); // Llamado del método anterior para ordenar bloques.
            }
        }
    }
}
