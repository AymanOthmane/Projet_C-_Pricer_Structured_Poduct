using System;
using MathNet.Numerics.Distributions;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics;
using ToolBox;
using Autocall;

namespace Program{

    public class ControleCenter{
        public static void Main(string[] args)
        {
            string filePath = Directory.GetCurrentDirectory() + @"\Interface Excel.xlsx";
            var data = Data.GetDataFromExcel(filePath);

            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
    
            double notionnel = (double)data[0];

            bool frequenceAnnuelle = (bool)data[1];

            int maturite = Convert.ToInt32(data[2]);

            string typeProduit = (string)data[3];

            double coupon = (double)data[4];

            double dernierPrix = (double)data[5];

            double vol30Jours = (double)data[6]/100;

            DateTime dateStrike = (DateTime)data[7];

            if (Convert.ToString(data[3]) == "ATHENA")
            {
                Athena product = new Athena(dateStrike, dernierPrix, notionnel, 0.04, maturite, vol30Jours, coupon, 0.00396825396825396825396825396825, frequenceAnnuelle);
                double prix = product.Pricing();
                Console.WriteLine("PRICING is " + prix);
                if (prix < 0.975){
                    Console.WriteLine("Reoffer à 2.5% : Accepté");
                    Console.WriteLine("Marge du desk: " + Math.Round((0.975 - prix)*notionnel,2) + " EUR");
                    if(Math.Round((0.975 - prix)*notionnel,2)> 10000){
                        Console.WriteLine("Le produit est accépté " );
                    }
                    else{
                        Console.WriteLine("Le produit n'est pas accépté" );
                    }
                }
            }
            

            if (Convert.ToString(data[3]) == "PHOENIX") //ooo
            {
                Phoenix product = new Phoenix(dateStrike, dernierPrix, dernierPrix, 0.7, 0.8, 0.04, 0, maturite, vol30Jours, coupon, 0.00396825396825396825396825396825, frequenceAnnuelle);
                double prix = product.Pricing();
                Console.WriteLine("PRICING is " + prix);
                if (prix < 0.975){
                    Console.WriteLine("Reoffer à 2.5% : Accepté");
                    Console.WriteLine("Marge du desk: " + Math.Round((0.975 - prix)*notionnel,2) + " EUR");
                    if (prix < 0.975){
                    Console.WriteLine("Reoffer à 2.5% : Accepté");
                    Console.WriteLine("Marge du desk: " + Math.Round((0.975 - prix)*notionnel,2) + " EUR");
                    if(Math.Round((0.975 - prix)*notionnel,2)> 10000){
                        Console.WriteLine("Le produit est accépté " );
                    }
                    else{
                        Console.WriteLine("Le produit n'est pas accépté" );
                    }
                }
                }
            }
            
        }
    }
}

// ////////////////////////////////////////////////////////// ////////////////////////////////////////////////////////// //////////////////////////////////////////////////////////:
// using Autocall;

// using System;

// using ToolBox;
 
// namespace Program

// {

//     public class ControleCenter

//     {

//         public static void Main(string[] args)

//         {

//             string filePath = Directory.GetCurrentDirectory() + @"\Interface Excel.xlsx";

//             var data = Data.GetDataFromExcel(filePath);
 
//             // Vérifier si les données ont été correctement lues

//             if (data.Count == 15)

//             {

//                 // Extraction des données

//                 double notionnel = (double)data[0];

//                 bool frequenceAnnuelle = (bool)data[1];

//                 int maturite = (int)data[2];

//                 string typeProduit = (string)data[3];

//                 double coupon = (double)data[4];

//                 DateTime dateStrike = (DateTime)data[5];

//                 string nomSousJacent = (string)data[6];

//                 string tickerBloomberg = (string)data[7];

//                 string codeISIN = (string)data[8];

//                 double dernierPrix = (double)data[9];

//                 double vol30Jours = (double)data[10];
 
//                 // Calcul du prix du produit en fonction des données

//                 double prixProduit = 0;

//                 if (typeProduit == "ATHENA")

//                 {

//                     Athena product = new Athena(dateStrike, dernierPrix, notionnel, 0.04, maturite, vol30Jours, coupon, 0.00396825396825396825396825396825, frequenceAnnuelle);

//                     prixProduit = product.Pricing();

//                 }

//                 else if (typeProduit == "PHOENIX")

//                 {

//                     Phoenix product = new Phoenix(dateStrike, dernierPrix, notionnel, coupon, coupon, 0.04, 0, maturite, vol30Jours, coupon, 0.00396825396825396825396825396825, frequenceAnnuelle);

//                     prixProduit = product.Pricing();

//                 }
 
//                 // Calcul de la prime fixée

//                 double primeFixee = 0.0; // Remplacez cette valeur par la prime fixée par votre client
 
//                 // Vérification de l'adéquation des critères

//                 bool adequation = VerifierAdequationCritere(prixProduit, primeFixee, notionnel, coupon, dateStrike, maturite);
 
//                 // Affichage du résultat

//                 if (adequation)

//                 {

//                     Console.WriteLine("Les critères sont en adéquation avec le produit proposé.");

//                 }

//                 else

//                 {

//                     Console.WriteLine("Les critères ne sont pas en adéquation avec le produit proposé.");

//                 }

//             }

//             else

//             {

//                 Console.WriteLine("Une erreur s'est produite lors de la lecture des données.");

//             }

//         }
 
//         // Méthode pour vérifier l'adéquation des critères

//         public static bool VerifierAdequationCritere(double prixProduit, double primeFixee, double notionnel, double coupon, DateTime dateStrike, int maturite)

//         {

//             // Vérifiez ici si les critères sont en adéquation avec le produit proposé

//             // Vous pouvez comparer le prix du produit avec la prime fixée et les autres critères

//             // Par exemple, vérifiez si le prix du produit + la prime fixée correspond au notionnel

//             // Vérifiez également d'autres critères comme la date de strike, la maturité, etc.

//             // Vous pouvez ajuster ces vérifications en fonction des besoins spécifiques de votre client
 
//             double prixTotal = prixProduit + primeFixee;
 
//             // Exemple de vérification simple : le prix total doit correspondre au notionnel

//             return prixTotal == notionnel;

//         }

//     }

// }
