using Dokotera.Exception;
using System.Linq;

namespace Dokotera.Models
{
    public class Simplex
    {
        public string[] variable { get; set; }

        public string[] baseDeuxPhases = new string[0];

        public string[] bazy = new string[0];
        public double[] Z {  get; set; }
        public double[][] inequation { get; set; }
        public double[] sommeArtificielle { get; set; }

        public string[] makingBase ()
        {
            string[] newTable = new string[inequation.Length + 2];
            newTable[newTable.Length-2] = "Z";
            newTable[newTable.Length-1] = "A";
            int indice = 0;
            /*for (int i = 0; i < inequation.Length; i++)
            {
                newTable[i] = variable[variable.Length - indice];
                indice--;
            }*/
            for(int i = 0; i < inequation.Length; i++)
            {
                for(int j = inequation[i].Length - 3; j >= inequation[i].Length - (3 + (inequation.Length*2) - 1); j--)
                {
                    if (inequation[i][j] == 1)
                    {
                        newTable[indice] = variable[j];
                    }
                    if (j == inequation[i].Length - (3 + (inequation.Length * 2) - 1)) indice++;
                }
            }
            return newTable;
        }

        public double[][] matriceDeuxPhase()
        {
            Array.Resize(ref baseDeuxPhases, makingBase().Length);
            baseDeuxPhases = makingBase();
            double[][] matriceDeuxPhase = new double[0][];
            for (int i = 0; i < inequation.Length; i++)
            {
                Array.Resize(ref matriceDeuxPhase, matriceDeuxPhase.Length + 1);
                matriceDeuxPhase[i] = inequation[i];
            }
            Array.Resize(ref matriceDeuxPhase, matriceDeuxPhase.Length + 1);
            matriceDeuxPhase[matriceDeuxPhase.Length - 1] = Z;
            Array.Resize(ref matriceDeuxPhase, matriceDeuxPhase.Length + 1);
            matriceDeuxPhase[matriceDeuxPhase.Length - 1] = sommeArtificielle;
            return matriceDeuxPhase;
        }

        int indexMinA(double[][] matrix)
        {
            int lastIndex = matrix.Length-1;
            int index = 0;
            for (int i = 0; i < matrix[lastIndex].Length; i++)
            {
                if (i < matrix[lastIndex].Length - 1 && matrix[lastIndex][i] < matrix[lastIndex][index]) index = i;
            }
            return index;
        }

        int getPivotdeuxPhases(double[][] matrix, int indexMinA)
        {
            int lastIndex = matrix[0].Length - 1;
            int indexZ = matrix.Length - 2;
            int index = 0;
            double valeur = 0;
            for (int i = 0; i < matrix.Length - 1; i++)
            {
                if (matrix[i][indexMinA] > 0 && i != indexZ)
                {
                    if (i == 0 || valeur == 0)
                    {
                        valeur = matrix[i][lastIndex] / matrix[i][indexMinA];
                        index = i;
                    }
                    else if (i > 0 && matrix[i][lastIndex] / matrix[i][indexMinA] < valeur)
                    {
                        valeur = matrix[i][lastIndex] / matrix[i][indexMinA];
                        index = i;
                    }
                }
            }
            return index;
        }

        int getPivotMin(double[][] matrix, int indexMinZ)
        {
            int lastIndex = matrix[0].Length - 1;
            int index = 0;
            double valeur = 0;
            for (int i = 0; i < matrix.Length - 1; i++)
            {
                if (matrix[i][indexMinZ] > 0)
                {
                    if (i == 0 || valeur == 0)
                    {
                        valeur = matrix[i][lastIndex] / matrix[i][indexMinZ];
                        index = i;
                    }
                    else if (i > 0 && matrix[i][lastIndex] / matrix[i][indexMinZ] < valeur)
                    {
                        valeur = matrix[i][lastIndex] / matrix[i][indexMinZ];
                        index = i;
                    }
                }
            }
            return index;
        }

        bool checkIfSommeartnull(double[][] matrix)
        {
            int lastIndex = matrix.Length - 1;
            int lastIndexline = matrix[lastIndex].Length - 1;
            for (int i = 0; i<matrix[lastIndex].Length-1; i++) {
                if(matrix[lastIndex][i] < 0 && matrix[lastIndex][lastIndexline] == 0) throw new Iresoluble("Simplex Iresoluble");
                else if(matrix[lastIndex][i] < 0) return false;
            }
            return true;
        }

        public double[][] algoSimplexedeuxphases(double[][] matrix)
        {
            int column = indexMinA(matrix);
            int line = getPivotdeuxPhases(matrix, column);
            Console.WriteLine("Colonne de Pivot : " + variable[column]);
            baseDeuxPhases[line] = variable[column];
            double pivot = matrix[line][column];
            bool test = false;
            for (int i = 0; i < matrix[line].Length; i++)
            {
                double value = 0;
                if (test)
                {
                    value = pivot * matrix[line][i];
                }
                else
                {
                    value = matrix[line][i] / pivot;
                }
                matrix[line][i] = value;
            }
            for (int i = 0; i < matrix.Length; i++)
            {
                if (i != line)
                {
                    double fraction = matrix[i][column] / matrix[line][column];
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        double temp = matrix[i][j];
                        double lignePivot = matrix[line][j];
                        temp = temp - fraction * lignePivot;
                        matrix[i][j] = temp;
                    }
                }
            }
            try
            {
                if (!checkIfSommeartnull(matrix))
                {
                    matrix = algoSimplexedeuxphases(matrix);
                }
            }
            catch (Iresoluble ex)
            {
                Console.WriteLine(ex.Message);
            }
            return matrix;
        }

        static int[] getArtificial(double[][] matrix)
        {
            int lastIndex = matrix.Length - 1;
            int indexZ = matrix[0].Length - 2;
            int[] indice = new int[0];
            int index = 0;
            for (int i = 0; i < matrix[lastIndex].Length - 1; i++)
            {
                if (matrix[lastIndex][i] == 1 || i == indexZ)
                {
                    Array.Resize(ref indice, indice.Length + 1);
                    indice[index] = i;
                    index++;
                }
            }
            return indice;
        }

        bool contenir(int[] tableau, int valeur)
        {
            foreach (int i in tableau)
            {
                if (i == valeur)
                {
                    return true;
                }
            }
            return false;
        }

        public double[][] withoutArtificial(double[][] matrix)
        {
            int[] skipColummns = getArtificial(matrix);
            int skipRow = matrix.Length - 1;
            double[][] newArray = new double[matrix.Length - 1][];
            int k = 0;
            for (int i = 0; i < matrix.Length; i++)
            {
                if (i == skipRow)
                {
                    continue;
                }
                Array.Resize(ref bazy, bazy.Length + 1);
                bazy[k] = baseDeuxPhases[i];
                k++;
                int newColumn = 0;
                newArray[i < skipRow ? i : i - 1] = new double[matrix[i].Length - skipColummns.Length];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (contenir(skipColummns, j))
                    {
                        continue;
                    }
                    newArray[i < skipRow ? i : i - 1][newColumn++] = matrix[i][j];
                }
            }
            int lastRow = newArray.Length - 1;
            for (int i = 0; i < newArray[lastRow].Length; i++)
            {
                double change = 0;
                if (newArray[lastRow][i] != 0) change = newArray[lastRow][i] * -1;
                newArray[lastRow][i] = change;
            }
            return newArray;
        }

        bool checkIfMinimiser(double[][] matrix)
        {
            int lastIndex = matrix.Length - 1;
            for (int i = 0; i < matrix[lastIndex].Length - 1; i++)
            {
                if (matrix[lastIndex][i] < 0) return false;
            }
            return true;
        }

        public double[][] algoSimplexeMin(double[][] matrix)
        {
            int column = indexMinA(matrix);
            int line = getPivotMin(matrix, column);
            bazy[line] = variable[column];
            double pivot = matrix[line][column];
            bool test = false;
            for (int i = 0; i < matrix[line].Length; i++)
            {
                double value = 0;
                if (test)
                {
                    value = pivot * matrix[line][i];
                }
                else
                {
                    value = matrix[line][i] / pivot;
                }
                matrix[line][i] = value;
            }
            for (int i = 0; i < matrix.Length; i++)
            {
                if (i != line)
                {
                    double fraction = matrix[i][column] / matrix[line][column];
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        double temp = matrix[i][j];
                        double lignePivot = matrix[line][j];
                        temp = temp - fraction * lignePivot;
                        matrix[i][j] = temp;
                    }
                }
            }

            try
            {
                if (!checkIfMinimiser(matrix)) matrix = algoSimplexeMin(matrix);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return matrix;
        }

    }
}
