using Dokotera.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Drawing;

namespace Dokotera.Models
{
    public class Resultat
    {
        public Dictionary<string, List<InfoSymptomesMaladie>> maladies { get; set; }
        public Dictionary<string, object> data { get; set; }
        public double[] valeurSymptomes {  get; set; }

        public void maladiesPatient(SymptomesPattient symptomesPatient)
        {
            this.maladies = symptomesPatient.getListesMaladies();
            List<string> key = maladies.Keys.ToList();
            double[] valSymptomes = new double[0];
            foreach (string keyItem in key) {
                foreach (var item in maladies[keyItem])
                {
                    int index = Array.IndexOf(symptomesPatient.symptomes, item.idSymptomes);
                    if (Array.IndexOf(valSymptomes, symptomesPatient.valeur[index]) < 0)
                    {
                        Array.Resize(ref valSymptomes, valSymptomes.Length + 1);
                        valSymptomes[valSymptomes.Length - 1] = symptomesPatient.valeur[index];
                    }
                }
            }
            this.valeurSymptomes = valSymptomes;
            double[] Z = new double[0];
            TraitementSymptomesService symptomesService = new TraitementSymptomesService();
            List<TraitementSymptomes> traitements = symptomesService.getAllTraitementSymptomesBetween(symptomesPatient.age).ToList();
            string[] symptomes = new string[0];
            string[] comparaison = new string[0];
            Console.WriteLine("taille traitements : "+traitements.Count);
            for(int i = 0; i < traitements.Count; i++)
            {
                Console.WriteLine("taillle anle symptomes : " + symptomes.Length);
                if (i == 0)
                {
                    Array.Resize(ref symptomes, symptomes.Length + 1);
                    Console.WriteLine("idSymptomes : " + traitements[i].idSymptomes);
                    symptomes[symptomes.Length - 1] = traitements[i].idSymptomes;
                }
                else if (i > 0 && traitements[i].idSymptomes != traitements[i-1].idSymptomes)
                {
                    Console.WriteLine("Manomboka tsy mitovy");
                    Array.Resize(ref symptomes, symptomes.Length + 1);
                    Console.WriteLine("idSymptomes : " + traitements[i].idSymptomes);
                    symptomes[symptomes.Length - 1] = traitements[i].idSymptomes;
                }
                if (comparaison.Any(comp => comp == traitements[i].idMedicament)) continue;
                Array.Resize(ref comparaison, comparaison.Length + 1);
                comparaison[comparaison.Length - 1] = traitements[i].idMedicament;
                Array.Resize(ref Z, Z.Length + 1);
                Z[i] = -traitements[i].prixUnitaire;
            }
            //Console.WriteLine("taille Z : " + Z.Length);
            double[][] artificielle = new double[0][];
            //Console.WriteLine("taille anle string[]: " + symptomes.Length);
            Array.Resize(ref artificielle, artificielle.Length + symptomes.Length);
            comparaison = new string[0];
            for (int i = 0; i < symptomes.Length; i++)
            {
                List<double> insert = new List<double>();
                for (int j = 0; j < traitements.Count; j++)
                {
                    if (comparaison.Any(comp => comp == traitements[j].idMedicament)) continue;
                    //Console.WriteLine("Mampiditra ao comparaison : " + traitements[j].idMedicament);
                    int val = symptomesService.getValeurSymptomes(traitements[j].idMedicament, symptomes[i]).valeurTraitemet;
                    //Console.WriteLine("valeurSymp : " + val);
                    insert.Add(val);
                    Array.Resize(ref comparaison, comparaison.Length + 1);
                    comparaison[comparaison.Length-1] = traitements[j].idMedicament;
                }
                comparaison = new string[0];
                artificielle[i] = insert.ToArray();
            }
            string[] variables = variableSimplex(traitements,symptomes.Length);
            for (int i = 0; i < artificielle.Length; i++)
            {
                for (int j = 0; j < variableArtificielle(symptomes.Length)[i].Length; j++)
                {
                    Console.Write(variableArtificielle(symptomes.Length)[i][j] + "  |  ");
                }
                Console.WriteLine("  ");
            }
            for (int i = 0; i < artificielle.Length; i++)
            {
                int k = artificielle[i].Length;
                for (int j = 0; j < variableArtificielle(symptomes.Length)[i].Length; j++)
                {
                    Array.Resize(ref artificielle[i], artificielle[i].Length + 1);
                    artificielle[i][k] = variableArtificielle(symptomes.Length)[i][j];
                    k++;
                }
            }
            int contrainteZero = mitovy(artificielle);
            if (contrainteZero >= 0)
            {
                artificielle = changeSigne(artificielle, contrainteZero);
            }
            double[] sommeArtificielle = SumByColumn(artificielle);
            int l = Z.Length;
            for(int i = 0; i < tohinZ(symptomes.Length).Length; i++)
            {
                Array.Resize(ref Z, Z.Length + 1);
                Z[l] = tohinZ(symptomes.Length)[i];
                l++;
            }
            Simplex solver = new Simplex()
            {
                variable = variables,
                Z = Z,
                inequation = artificielle,
                sommeArtificielle = sommeArtificielle,
            };
            double[][] matrice1 = solver.matriceDeuxPhase();
            double[][] matrice2 = solver.algoSimplexedeuxphases(matrice1);
            double[][] matrice3 = solver.withoutArtificial(matrice2);
            double[][] matrice4 = solver.algoSimplexeMin(matrice3);
            Console.WriteLine("Forme matricielle");
            foreach(string var in variables)
            {
                Console.WriteLine(var);
            }
            for (int i = 0; i < matrice1.Length; i++)
            {
                Console.Write(solver.baseDeuxPhases[i] + "  |  ");
                for (int j = 0; j < matrice1[i].Length; j++)
                {
                    Console.Write(matrice1[i][j] + "  |  ");
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine(" ");
            Console.WriteLine("Minimisation Phase 1");
            for (int i = 0; i < matrice2.Length; i++)
            {
                Console.Write(solver.baseDeuxPhases[i] + "  |  ");
                for (int j = 0; j < matrice2[i].Length; j++)
                {
                    Console.Write(matrice2[i][j] + "  |  ");
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine(" ");
            Console.WriteLine("Sans Variable Artificiel");
            for (int i = 0; i < matrice3.Length; i++)
            {
                Console.Write(solver.bazy[i] + "  |  ");
                for (int j = 0; j < matrice3[i].Length; j++)
                {
                    Console.Write(matrice3[i][j] + "  |  ");
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine(" ");
            Console.WriteLine("Resultat Final");
            for (int i = 0; i < matrice4.Length; i++)
            {
                Console.Write(solver.bazy[i] + "  |  ");
                for (int j = 0; j < matrice4[i].Length; j++)
                {
                    Console.Write(matrice4[i][j] + "  |  ");
                }
                Console.WriteLine(" ");
            }
        }

        string[] variableSimplex(List<TraitementSymptomes> symptomes, int nombreSymptomes) 
        {
            string[] variable = new string[symptomes.Count + nombreSymptomes * 2 + 1];
            variable[variable.Length - 1] = "Z";
            for(int i = variable.Length - 2; i >= symptomes.Count; i--)
            {
                variable[i] = "X" + i;
            }
            for (int i = 0;i < symptomes.Count;i++)
            {
                if (variable.Any(va => va == symptomes[i].idMedicament))
                {
                    Array.Copy(variable, i + 1, variable, i, variable.Length - i - 1);
                    Array.Resize(ref variable, variable.Length - 1);
                    continue;
                }
                Console.WriteLine("Mampiditra an'ny : " + symptomes[i].idMedicament);
                variable[i] = symptomes[i].idMedicament;
            }
            return variable;
        }

        double[][] variableArtificielle(int taille)
        {
            double[][] newDouble = new double[taille][];
            for (int i = 0; i < taille; i++)
            {
                newDouble[i] = new double[taille * 2 + 2];
                for (int j = 0; j < taille * 2 + 2; j++)
                {
                    if (j == i)
                    {
                        newDouble[i][j] = -1;
                    }
                    else if (j == taille + i)
                    {
                        newDouble[i][j] = 1;
                    }
                    else if (j == (taille * 2) + 1)
                    {
                        if (i < valeurSymptomes.Length)
                        {
                            newDouble[i][j] = valeurSymptomes[i];
                        }
                        else newDouble[i][j] = 0;
                    }
                    else
                    {
                        newDouble[i][j] = 0;
                    }
                }
            }
            return newDouble;
        }

        public double[] SumByColumn(double[][] arr)
        {
            double[] result = new double[arr[0].Length];
            for (int i = 0; i < arr[0].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < arr.Length; j++)
                {
                    if (i <= arr[j].Length-3 && i >= arr[j].Length - (3+arr.Length-1))
                    {
                        sum += 0;
                    }
                    else
                    {
                        sum += arr[j][i];
                    }
                }
                result[i] = -sum;
            }
            return result;
        }

        double[] tohinZ(int nombreSymptomes)
        {
            double[] tab = new double[nombreSymptomes*2+2];
            tab[tab.Length - 1] = 0;
            tab[tab.Length - 2] = 1;
            for (int i = 0;i < tab.Length-2;i++)
            {
                tab[i] = 0;
            }
            //double[] tab = new double[6] { 0, 0, 0, 0, 1, 0 };
            return tab;
        }

        double[][] changeSigne(double[][] contrainte, int index)
        {
            for(int i = 0; i < contrainte[index].Length;i++) 
            {
                if (i <= contrainte[index].Length - 3 && i >= contrainte[index].Length - (3 + contrainte.Length - 1))
                {
                    contrainte[index][i] = 0;
                }
                else
                {
                    contrainte[index][i] = -contrainte[index][i];
                }
            }
            return contrainte;
        }

        /****ENCAS OE TSY MISY ANLE MIDINA AMBANIN 0******/
        /*
        string[] variableSimplex(List<TraitementSymptomes> symptomes, int nombreSymptomes) 
        {
            string[] variable = new string[symptomes.Count + nombreSymptomes + 1];
            variable[variable.Length - 1] = "Z";
            for(int i = variable.Length - 2; i >= symptomes.Count; i--)
            {
                variable[i] = "X" + i;
            }
            for (int i = 0;i < symptomes.Count;i++)
            {
                variable[i] = symptomes[i].idMedicament;
            }
            return variable;
        }

        double[][] variableArtificielle(int taille)
        {
            double[][] newDouble = new double[taille][];
            for (int i = 0; i < taille; i++)
            {
                newDouble[i] = new double[taille + 2];
                for (int j = 0; j < taille + 2; j++)
                {
                    if (j == i)
                    {
                        newDouble[i][j] = 1;
                    }
                    else if (j == taille + 1)
                    {
                        newDouble[i][j] = valeurSymptomes[i];
                    }
                    else
                    {
                        newDouble[i][j] = 0;
                    }
                }
            }
            return newDouble;
        }
        double[] tohinZ(int nombreSymptomes)
        {
            double[] tab = new double[nombreSymptomes+2];
            tab[tab.Length - 1] = 0;
            tab[tab.Length - 2] = 1;
            for (int i = 0;i < tab.Length-2;i++)
            {
                tab[i] = 0;
            }
            //double[] tab = new double[6] { 0, 0, 0, 0, 1, 0 };
            return tab;
        }
         */


        int mitovy(double[][] contraintes)
        {
            for(int i = 0; i < contraintes.Length; i++)
            {
                if (contraintes[i][contraintes[i].Length - 1] == 0) return i;
            }
            return -1;

        }
    }
}
