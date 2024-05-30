using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class IngresarDatos
{
    public static void AddData()
    {
        Console.WriteLine("Ingresa los valores (separados por espacio, 'f' para finalizar)");
        List<double> datos = new List<double>();

        string input = Console.ReadLine();
        while (input != "f")
        {
            string[] parts = input.Split(' ');
            foreach (string part in parts)
            {
                if (double.TryParse(part, out double num))
                {
                    datos.Add(num);
                }
            }
            input = Console.ReadLine();
        }

        datos.Sort();
        Dictionary<double, int> conteo = new Dictionary<double, int>();
        foreach (double elemento in datos)
        {
            if (conteo.ContainsKey(elemento))
            {
                conteo[elemento]++;
            }
            else
            {
                conteo[elemento] = 1;
            }
        }

        if (datos.Count < 30)
        {
            Console.WriteLine(" ---- DATOS NO AGRUPADOS --- ");
            Console.WriteLine($"|{"Xi",6}|{"F",6}|{"Fr",8}|{"Fa",8}|{"Fra",8}|");
            Console.WriteLine(new string('-', 36));
            double Fa = 0;
            foreach (var par in conteo)
            {
                double b = datos.Count;
                double Fr = par.Value;
                Fa += par.Value;
                double Fra = Fa / b;

                Console.WriteLine($"|{par.Key,6}|{par.Value,6}|{Math.Round(Fr / b, 2),8}|{Math.Round(Fa),8}|{Math.Round(Fra, 2),8}|");
            }

            Console.WriteLine();
            Console.WriteLine("MEDIA : " + (Math.Round(CalcularMedia(datos), 2)) + "\nMEDIANA :" + (Math.Round(CalcularMediana(datos), 2)) + "\nMODA : " + CalcularModa(datos) + "\nVARIANZA: " + (Math.Round(CalcularVarianza(datos), 2)) + "\nDesviacion Estandar: " + (Math.Round(CalcularDE(datos), 2)) + " \n");
        }
        else
        {
            Console.WriteLine(" --- DATOS AGRUPADOS --- ");
            datos.Sort();
            double minValor = datos[0];
            double maxValor = datos[datos.Count - 1];
            double n = datos.Count;
            double rango = (maxValor - minValor);
            double k = Math.Round(1 + 3.32 * Math.Log10(n));
            double I = rango / k;

            Console.WriteLine($"RANGO:  {rango}  \nCLASES:  {k} \nINTERVALO: {Math.Round(I, 2)}");
            Console.WriteLine();

            List<Tuple<double, double>> intervalos = new List<Tuple<double, double>>();
            for (double start = minValor; start < maxValor; start += I)
            {
                intervalos.Add(new Tuple<double, double>(start, Math.Min(start + I, maxValor)));
            }

            Dictionary<Tuple<double, double>, int> frecuencias = new Dictionary<Tuple<double, double>, int>();
            foreach (var intervalo in intervalos)
            {
                frecuencias[intervalo] = datos.Count(x => x >= intervalo.Item1 && x < intervalo.Item2);
            }

            Console.WriteLine($"|{"Intervalo",10}|{"Xi",8}|{"F",6}|{"Fr",8}|{"Fa",8}|{"Fra",8}|");
            Console.WriteLine(new string('-', 60));
            double Fa = 0;
            foreach (var intervalo in intervalos)
            {
                double Xi = (intervalo.Item1 + intervalo.Item2) / 2.0;
                int F = frecuencias[intervalo];
                double Fr = (double)F / n;
                Fa += F;
                double Fra = Fa / n;

                Console.WriteLine($"|{Math.Round(intervalo.Item1, 2),5} -{Math.Round(intervalo.Item2, 2),5}|{Math.Round(Xi, 2),8}|{F,6}|{Math.Round(Fr, 2),8}|{Fa,8}|{Math.Round(Fra, 2),8}|");
            }

            List<Tuple<double, double, int>> intervalosConFrecuencia = intervalos.Select(intervalo => new Tuple<double, double, int>(intervalo.Item1, intervalo.Item2, frecuencias[intervalo])).ToList();
            Console.WriteLine($"MEDIA: {Math.Round(CalcularMediaAgrupada(intervalosConFrecuencia, datos.Count), 2)} \nMEDIANA: {Math.Round(CalcularMedianaAgrupada(intervalosConFrecuencia, datos.Count), 2)} \nMODA:  {Math.Round(CalcularModaAgrupada(intervalosConFrecuencia), 2)} \nVARIANZA  {Math.Round(CalcularVarianzaAgrupada(intervalosConFrecuencia, datos.Count), 2)} \nDESVIACIÓN ESTANDAR: {Math.Round(CalcularDEAgrupados(intervalosConFrecuencia, datos.Count), 2)}");
        }
    }

    private static double CalcularMedia(List<double> datos)
    {
        double suma = datos.Sum();
        return suma / datos.Count;
    }

    private static double CalcularModa(List<double> datos)
    {
        Dictionary<double, int> conteo = new Dictionary<double, int>();
        foreach (double elemento in datos)
        {
            if (conteo.ContainsKey(elemento))
            {
                conteo[elemento]++;
            }
            else
            {
                conteo[elemento] = 1;
            }
        }

        double moda = 0;
        int maxFrecuencia = 0;
        foreach (var par in conteo)
        {
            if (par.Value > maxFrecuencia)
            {
                moda = par.Key;
                maxFrecuencia = par.Value;
            }
        }
        return moda;
    }

    private static double CalcularMediana(List<double> datos)
    {
        datos.Sort();
        int n = datos.Count;
        if (n % 2 == 0)
        {
            return (datos[n / 2 - 1] + datos[n / 2]) / 2.0;
        }
        else
        {
            return datos[(n - 1) / 2];
        }
    }

    private static double CalcularVarianza(List<double> datos)
    {
        double media = CalcularMedia(datos);
        double suma = 0;
        foreach (double valor in datos)
        {
            suma += Math.Pow((valor - media), 2);
        }
        return suma / datos.Count;
    }

    private static double CalcularDE(List<double> datos)
    {
        return Math.Sqrt(CalcularVarianza(datos));
    }

    private static double CalcularMediaAgrupada(List<Tuple<double, double, int>> frecuencias, int n)
    {
        double suma = 0;
        foreach (var intervalo in frecuencias)
        {
            double Xi = (intervalo.Item1 + intervalo.Item2) / 2.0;
            suma += Xi * intervalo.Item3;
        }
        return suma / n;
    }

    private static double CalcularMedianaAgrupada(List<Tuple<double, double, int>> frecuencias, int n)
    {
        int N_2 = n / 2;
        double F_acum = 0;
        Tuple<double, double, int> intervaloMediana = null;

        foreach (var intervalo in frecuencias)
        {
            F_acum += intervalo.Item3;
            if (F_acum >= N_2)
            {
                intervaloMediana = intervalo;
                break;
            }
        }

        double L = intervaloMediana.Item1;
        double F_ant = F_acum - intervaloMediana.Item3;
        double f = intervaloMediana.Item3;
        double c = intervaloMediana.Item2 - intervaloMediana.Item1;

        return L + ((N_2 - F_ant) / f) * c;
    }

    private static double CalcularModaAgrupada(List<Tuple<double, double, int>> frecuencias)
    {
        var maxFrecuencia = frecuencias.OrderByDescending(i => i.Item3).First();
        double L = maxFrecuencia.Item1;
        double d1 = maxFrecuencia.Item3 - frecuencias.First(i => i.Item1 == maxFrecuencia.Item1 - (maxFrecuencia.Item2 - maxFrecuencia.Item1)).Item3;
        double d2 = maxFrecuencia.Item3 - frecuencias.First(i => i.Item1 == maxFrecuencia.Item2).Item3;
        double c = maxFrecuencia.Item2 - maxFrecuencia.Item1;

        return L + (d1 / (d1 + d2)) * c;
    }

    private static double CalcularVarianzaAgrupada(List<Tuple<double, double, int>> frecuencias, int n)
    {
        double mediaAgrupada = CalcularMediaAgrupada(frecuencias, n);
        double suma = 0;
        foreach (var intervalo in frecuencias)
        {
            double Xi = (intervalo.Item1 + intervalo.Item2) / 2.0;
            suma += intervalo.Item3 * Math.Pow(Xi - mediaAgrupada, 2);
        }
        return suma / n;
    }
    private static double CalcularDEAgrupados(List<Tuple<double, double, int>> frecuencias, int n)
    {
        return Math.Sqrt(CalcularVarianzaAgrupada(frecuencias, n));
    }
}

public class Consultar
{
    public static void Serch()
    {
        Console.WriteLine("Ingresa el nombre del archivo a consultar: ");
        string nomU = Console.ReadLine();
        TextReader nombre;
        nombre = new StreamReader(nomU + ".txt");
        string line;
        while ((line = nombre.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
        nombre.Close();
    }
}

public class MainClass
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Ingresa tu Nombre: ");
        string nomU = Console.ReadLine();
        TextWriter nombre;
        nombre = new StreamWriter(nomU + ".txt");

        while (true)
        {
            Console.WriteLine("1.-Ingresar datos \n2.-Consultar movimiento de: " + nomU);
            int options = Convert.ToInt32(Console.ReadLine());
            switch (options)
            {
                case 1:
                    IngresarDatos.AddData();
                    break;
                case 2:
                    Consultar.Serch();
                    break;
                default:
                    Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                    break;
            }
        }
    }
}
