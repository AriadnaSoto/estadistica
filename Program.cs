using System;
using System.Collections;
using System.Globalization;
using System.IO;

public class main
{
    public static void Main(string[] args){
      
        Console.WriteLine("Ingresa tú Nombre: ");
        string nomU = Console.ReadLine();
          TextWriter nombre ;
          nombre = new StreamWriter(nomU+".txt");  
          //falta almacenar la informacion en el archivo   
    
        do
        {
            Console.WriteLine("1.-Ingresar datos \n2.-Consultar movimiento de {nombre} ");
            int options=Convert.ToInt32(Console.ReadLine());
            switch (options)
            {
                case 1 :
                ingresarDatos.addData();
                break;   
                case 2 :
                Consultar.serch();
                break;
           
            }

         } while (true);
         
    Console.ReadKey();     
         
    }
}

public class ingresarDatos{
     public static void addData(){
        string num; 
        List<int>datos = new List<int>();       
        while ((num=Console.ReadLine())!="f")
        {
            Console.WriteLine("Ingresa los valores ");
            datos.Add(Convert.ToInt32(num));    
        }
       
        
    }        
}   

public class Consultar{
    public static void serch(){

    }

}


