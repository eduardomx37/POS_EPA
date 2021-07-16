using System;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace POS_EPA
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Variables

            /*Integer*/
            int intValue = 0;
            int inputValue;

            /*String*/
            string input = string.Empty;
            string strLanguage = string.Empty;

            //Check language
            string strFullPath = Path.GetFullPath("../../App_Data/Config.txt");
            string strValue = File.ReadAllText(strFullPath);

            intValue = Int16.Parse(strValue);

            #endregion

            Console.WriteLine("\n         *******************************");
            Console.WriteLine("         ...::: ***  Welcome  *** :::...");
            Console.WriteLine("         *******************************");

            //Select your language and save variable
            if (intValue < 1)
            {
                Console.WriteLine("\n\nSelect your language: \n1.To use US \n2.To use MX\nInput one option and press enter:");

                input = Console.ReadLine();

                //Validate
                bool success = int.TryParse(input, out inputValue);
                bool valid = success && 0 <= inputValue && inputValue <= 2;
                while (!valid)
                {
                    Console.WriteLine("\n\n       ***   Invalid Input !. Try again...   ***");
                    Console.Write("\nPlease, select your language: \n1.To use US \n2.To use MX\nInput one option and press enter: ");
                    input = Console.ReadLine();
                    success = int.TryParse(input, out inputValue);
                    valid = success && 0 <= inputValue && inputValue <= 2;
                }

                if (input == "1")
                    Console.WriteLine("\nThank you for choose: US !");
                else
                    Console.WriteLine("\nThank you for choose: MX !");

                //Save variable:
                using (StreamWriter writer = new StreamWriter(strFullPath))
                {
                    writer.WriteLine(input);
                }

                POS_EPA(intValue);


            }
            else
                POS_EPA(intValue);

            Console.ReadLine();
        }

        /// <summary>
        /// POS_EPA
        /// </summary>
        /// <param name="intValue"></param>
        public static void POS_EPA(int intValue)
        {
            #region Variables

            /*Integer*/
            int intValInteger = 0;
            int intValDecimal = 0;
            int dbResiduo;
            //Bills USA & MX
            int intBill100 = 100;
            int intBill50 = 50;
            int intBill20 = 20;
            //Only USA
            int intBill10 = 10;
            int intBill5 = 5;
            int intBill2 = 2;
            int intBill1 = 1;
            //Coins USA
            int intCoin50 = 50;
            int intCoin25 = 25;
            int intCoin10 = 10;
            int intCoin05 = 05;
            int intCoin01 = 01;
            //Coins MX
            int intCoin5 = 5;
            int intCoin2 = 2;
            int intCoin1 = 1;
            int intCoin50mx = 50;
            int intCoin20mx = 20;
            int intCoin10mx = 10;
            int intCoin05mx = 05;

            /*Double*/
            double dbPrice;
            double dbinputValue;
            double dbAmount;
            double dbReturnAmount;
            double dbResult;

            /*String*/
            string input = string.Empty;
            string strLanguage = string.Empty;

            //Check language
            string specifier;
            string strFullPath = Path.GetFullPath("../../App_Data/Config.txt");
            string strValue = File.ReadAllText(strFullPath);

            /*CultureInfo*/
            CultureInfo culture;

            intValue = Int16.Parse(strValue);

            #endregion

            if (intValue == 1)
                strLanguage = "US";
            else
                strLanguage = "MX";

            Console.WriteLine("\n\nThe configuration is ready for use in " + strLanguage + " !");

            //#1
            Console.WriteLine("\n1.- Input price of the item being purchased");
            string inputPrice = Console.ReadLine();

            if (!Double.TryParse(inputPrice, out dbPrice))
                Console.WriteLine("Wrong input");
            else
                dbPrice = double.Parse(inputPrice);

            //Delete lasta line
            ClearLine();

            //Print currency
            specifier = "C";
            culture = CultureInfo.CreateSpecificCulture("en-US");
            Console.WriteLine(dbPrice.ToString(specifier, culture));

            //#2
            Console.WriteLine("\n2.- Input bills and coins");
            string inputPayi = Console.ReadLine();

            if (!Double.TryParse(inputPayi, out dbAmount))
                Console.WriteLine("Wrong input");
            else
                dbAmount = double.Parse(inputPayi);

            //Delete last line
            ClearLine();
            Console.WriteLine(dbAmount.ToString(specifier, culture));

            //Validate
            //bool success = int.TryParse(input, out inputValue);
            bool success = double.TryParse(inputPayi, out dbinputValue);
            bool valid = success && double.Parse(inputPrice) <= dbinputValue;
            while (!valid)
            {
                Console.WriteLine("\n\n       ***   Insufficient money !. Input more money...   ***");
                inputPayi = Console.ReadLine();

                if (!Double.TryParse(inputPayi, out dbAmount))
                {
                    Console.WriteLine("Wrong input");
                    inputPayi = "0";
                }

                //Delete last line
                ClearLine();

                dbAmount = dbAmount + double.Parse(inputPayi);
                inputPayi = dbAmount.ToString();

                success = double.TryParse(inputPayi, out dbinputValue);
                valid = success && double.Parse(inputPrice) <= dbinputValue;

                //Delete last line
                ClearLine();
            }

            if (valid)
            {
                dbReturnAmount = dbAmount - double.Parse(inputPrice);

                //Delete last line
                ClearLine();

                //Total purchased
                Console.WriteLine("\n   ***   Total purchased ....." + dbPrice.ToString(specifier, culture));

                //Total pay
                Console.WriteLine("\n   ***   Total pay ..........." + dbAmount.ToString(specifier, culture));

                //Change due
                Console.WriteLine("\n   ***   Change due .........." + dbReturnAmount.ToString(specifier, culture));

                //Change due
                Console.WriteLine("\n   ***   Detail:");

                string[] strValues = dbReturnAmount.ToString().Split('.');

                if (Convert.ToInt32(strValues[0].ToString()) != 0)
                    intValInteger = Convert.ToInt32(strValues[0].ToString());

                if (strValues.Length > 1)
                {
                    intValDecimal = Convert.ToInt32(strValues[1].ToString());

                    if (intValDecimal.ToString().Length == 1)
                        intValDecimal = int.Parse(intValDecimal.ToString() + "0");

                }
                
                dbResult = 0;

                intBill100 = intValInteger / 100;
                dbResiduo = intValInteger % 100;

                intBill50 = dbResiduo / 50;
                dbResiduo = dbResiduo % 50;

                intBill20 = dbResiduo / 20;
                dbResiduo = dbResiduo % 20;

                if (intValue == 1)
                {
                    intBill10 = dbResiduo / 10;
                    dbResiduo = dbResiduo % 10;

                    intBill5 = dbResiduo / 5;
                    dbResiduo = dbResiduo % 5;

                    intBill2 = dbResiduo / 2;
                    dbResiduo = dbResiduo % 2;

                    intBill1 = dbResiduo / 1;
                    dbResiduo = dbResiduo % 1;

                    intCoin50 = intValDecimal / 50;
                    dbResiduo = intValDecimal % 50;

                    intCoin25 = dbResiduo / 25;
                    dbResiduo = dbResiduo % 25;

                    intCoin10 = dbResiduo / 10;
                    dbResiduo = dbResiduo % 10;

                    intCoin05 = dbResiduo / 05;
                    dbResiduo = dbResiduo % 05;

                    intCoin01 = dbResiduo / 01;
                    dbResiduo = dbResiduo % 01;

                    //Cantifad minima de billetes
                    dbResult = intBill100 + intBill50 + intBill20 + intBill10 + intBill5 + intBill2 + intBill1 + intCoin50 + intCoin25 + intCoin10 + intCoin05 + intCoin01;

                    Console.WriteLine("                 Bills $100     : {0}", intBill100);
                    Console.WriteLine("                 Bills $50      : {0}", intBill50);
                    Console.WriteLine("                 Bills $20      : {0}", intBill20);
                    Console.WriteLine("                 Bills $10      : {0}", intBill10);
                    Console.WriteLine("                 Bills $5       : {0}", intBill5);
                    Console.WriteLine("                 Bills $2       : {0}", intBill2);
                    Console.WriteLine("                 Bills $1       : {0}", intBill1);
                    Console.WriteLine("                 Coins ¢.50     : {0}", intCoin50);
                    Console.WriteLine("                 Coins ¢.25     : {0}", intCoin25);
                    Console.WriteLine("                 Coins ¢.10     : {0}", intCoin10);
                    Console.WriteLine("                 Coins ¢.05     : {0}", intCoin05);
                    Console.WriteLine("                 Coins ¢.01     : {0}", intCoin01);
                }
                else if (intValue == 2)
                {
                    intCoin10 = dbResiduo / 10;
                    dbResiduo = dbResiduo % 10;

                    intCoin5 = dbResiduo / 5;
                    dbResiduo = dbResiduo % 5;

                    intCoin2 = dbResiduo / 2;
                    dbResiduo = dbResiduo % 2;

                    intCoin1 = dbResiduo / 1;
                    dbResiduo = dbResiduo % 1;

                    intCoin50mx = intValDecimal / 50;
                    dbResiduo = intValDecimal % 50;

                    intCoin20mx = dbResiduo / 20;
                    dbResiduo = dbResiduo % 20;

                    intCoin10mx = dbResiduo / 10;
                    dbResiduo = dbResiduo % 10;

                    intCoin05mx = dbResiduo / 05;
                    dbResiduo = dbResiduo % 05;

                    //Cantifad minima de billetes
                    dbResult = intBill100 + intBill50 + intBill20 + intCoin10 + intCoin5 + intCoin2 + intCoin1 + intCoin50mx + intCoin20mx + intCoin10mx + intCoin05mx;

                    Console.WriteLine("                 Bills $100     : {0}", intBill100);
                    Console.WriteLine("                 Bills $50      : {0}", intBill50);
                    Console.WriteLine("                 Bills $20      : {0}", intBill20);
                    Console.WriteLine("                 Coins $10      : {0}", intCoin10);
                    Console.WriteLine("                 Coins $5       : {0}", intCoin5);
                    Console.WriteLine("                 Coins $2       : {0}", intCoin2);
                    Console.WriteLine("                 Coins $1       : {0}", intCoin1);
                    Console.WriteLine("                 Coins ¢.50     : {0}", intCoin50mx);
                    Console.WriteLine("                 Coins ¢.20     : {0}", intCoin20mx);
                    Console.WriteLine("                 Coins ¢.10     : {0}", intCoin10mx);
                    Console.WriteLine("                 Coins ¢.05     : {0}", intCoin05mx);
                }

                Console.WriteLine("\nThe smallest number of bills and coins: {0}", dbResult);
            }
        }

        //Delete last line
        public static void ClearLine(int lines = 1)
        {
            for (int i = 1; i <= lines; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }
    }
}
