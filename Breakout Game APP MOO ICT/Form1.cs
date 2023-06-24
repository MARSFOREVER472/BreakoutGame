﻿using System; // Librería principal.
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

        Random rnd = new Random(); // Variable que se mueve automáticamente la bola.

        // Método inicial.
        public Form1()
        {
            InitializeComponent(); // Inicializa todos los componentes del juego.
            setupGame(); // Llamado del método privado.
        }

        // Método de ajustes a los componentes del juego.
        private void setupGame()
        {
            score = 0; // Puntuación inicial del juego.
            ballx = 5; // Tamaño inicial de la bola.
            bally = 5; // Altura inicial de la bola.
            playerSpeed = 12; // Velocidad inicial del jugador.
            txtScore.Text = "Score: " + score;

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
                gameOver("Has ganado la partida! :)"); // Llama al método gameOver diciendo que ganó la partida.
            }

            if (ball.Top > 580) // Si la bola alcanzó el límite inferior del juego.
            {
                gameOver("Lo siento, perdió la partida! :("); // Llama al método gameOver diciendo que perdió la partida.
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
        }
    }
}
