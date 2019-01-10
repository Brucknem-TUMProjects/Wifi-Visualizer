using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Matrix {

    private double[,] matrix;
    
    public Matrix(double[,] matrix)
    {
        CheckSquare(matrix.GetLength(0) * matrix.GetLength(1));
        this.matrix = matrix;
    }

    public Matrix(params double[] entries)
    {
        int size = CheckSquare(entries.Length);

        matrix = new double[size, size];
        for(int j = 0; j < size; j++)
        {
            for(int i = 0; i < size; i++)
            {
                matrix[i, j] = entries[j * size + i];
            }
        }
    }

    private int CheckSquare(int length)
    {
        double sqr = Mathf.Sqrt(length);
        if (sqr % 1.0 != 0)
        {
            throw new ArgumentException("Not square!");
        }
        return (int)sqr;
    }

    //Erstellt dieselbe Matrix, bloß ohne die column-te Spalte und row-te Zeile
    private double[,] make_matrix(double[,] matrix, int column, int row)
    {
        //Das sit die neue MAtrix, dessen DImensionen jeweils um eins kleiner sind als von der
        //ursprünglichen
        double[,] new_matrix = new double[(matrix.GetLength(0) - 1), (matrix.GetLength(1) - 1)];

        //Da man jeweils eine bestimmte Zeile und spalte ausschließt, besteht die neue Matrix aus 4 Bereichen
        // der alten Matrix. Diese vier Bereiche werden jetzt zusammengesetzt

        //Erster Teil der Matrix
        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
                new_matrix[x, y] = matrix[x, y];
        }
        //Zweiter 
        for (int y = (column + 1); y < matrix.GetLength(1); y++)
        {
            for (int x = 0; x < row; x++)
                new_matrix[x, (y - 1)] = matrix[x, y];
        }
        //dritter
        for (int y = 0; y < column; y++)
        {
            for (int x = (row + 1); x < matrix.GetLength(0); x++)
                new_matrix[(x - 1), y] = matrix[x, y];
        }
        //vierter
        for (int y = (column + 1); y < matrix.GetLength(1); y++)
        {
            for (int x = (row + 1); x < matrix.GetLength(0); x++)
                new_matrix[(x - 1), (y - 1)] = matrix[x, y];
        }

        return new_matrix;
    }

    public double Determinante
    {
        get
        {
            return DeterminanteRec(matrix);
        }
    }

    //Wenn die Matrix größer als (2x2) ist, ruft sich det rekrusiv auf, bis det (2x2) ist
    private double DeterminanteRec(double[,] matrix)
    {
        //Wenn Matrix nicht quadratisch ist
        if (matrix.GetLength(0) != matrix.GetLength(1))
            return 0;

        //Wenn matrix (2x2) ist, wird die MAtrix wie eine (2x2) über Kreuz entwickelt
        if (matrix.GetLength(0) == 2 && matrix.GetLength(1) == 2)
        {
            return ((matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]));
        }

        //Matrix ist größer als (2x2)

        double ret_det = 0;                                 //Determinante als Rückgabewert
        bool negative = false;                              //Boole'sche Variable zur bestimmung
                                                            // des positiven oder negativen Vorzeichens

        for (int i = 0; i < matrix.GetLength(1); i++)       //Entwicklung nach der ersten Zeile
        {
            if (negative)                                    //Wenn vorfaktor negativ ist
            {
                //Rekrusiver Aufruf mit übergabe neuer Matrix ohne erster Zeile und i-ter Spalte
                ret_det += (-1) * matrix[1, i] * DeterminanteRec(make_matrix(matrix, i, 1));
                negative = false;                           //nächstes Glied ist positiv
            }
            else if (!negative)
            {
                ret_det += matrix[1, i] * DeterminanteRec(make_matrix(matrix, i, 1));
                negative = true;
            }
        }

        return ret_det;
    }
}
