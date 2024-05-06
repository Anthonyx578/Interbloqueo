// See https://aka.ms/new-console-template for more information
//Definimos que vamos a usar propiedades del sistema
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

class Program
{
    //Definimos las cerraduras para evitar el bloqueo de los hilos
    static object lock1 = new object();
    static object lock2 = new object();
    
    //solucion de asignacion
    static object lock3= new object();
    static object lock4= new object();

    //Ejecucion de la funcion principal
    static void Main()
    {
        //Se inicia el hilo uno
        Thread thread1 = new Thread(() =>
        {
            
            //Aqui el hilo 1 adquiere la cerradura, haciendo que esta no se pueda usar por ningun otro hilo
            lock (lock1)
            {
                //Se imprime en pantalla que el hilo 1 esta bloqueado en la cerradura lock1
                Console.WriteLine("Thread 1: Bloqueado en lock1");
                //Se sumula un tiempo de proceso para seguir la ejecucion
                Thread.Sleep(1000);
                Console.WriteLine("Thread 1: Adquiriendo cerradura 2...");
                //Aqui el hilo 1 quiere adquirir la cerradura 2 pero este no va a poder
                lock (lock2)
                {
                    //Nunca se va a llegar a este punto
                    Console.WriteLine("Thread 1: Bloqueado en lock2");
                }
            }
        });
        //Al mismo tiempo se inicia el hilo dos 
        Thread thread2 = new Thread(() =>
        {
            //Este obtiene la cerradura 2
            lock (lock2)
            //lock(lock1)
            {
                //Se muestra un mensaje de obtencion
                Console.WriteLine("Thread 2: Bloqueado en lock2");
                //se simula un proceso ejecutandose
                Thread.Sleep(1000);
                Console.WriteLine("Thread 2: Adquiriendo cerradura 1...");
                //Aqui el hilo 2 quiere adquirir la cerradura 1 pero como este lo esta 
                //usando el hilo 1 y su proceso no finaliza no va poder acceder
                lock(lock1)
                //lock (lock2)
                {
                    //Nunca se va a llegar a este punto
                    //Console.WriteLine("Thread 2: Bloqueado en lock2");
                    Console.WriteLine("Thread 2: Bloqueado en lock1");
                }
            }
        });
        //Asignamos nombres a los hilos
        thread1.Name = "Hilo1";
        thread2.Name = "Hilo2";
        //Iniciamos hilos
        thread1.Start();
        thread2.Start();
        //nos aseguramos que el programa espere a estos 
        thread1.Join();
        thread2.Join();

        Console.WriteLine("Fin del programa");
    }
}