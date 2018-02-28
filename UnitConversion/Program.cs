using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.IO;

namespace UnitConversion
{
    class Program
    {
        static void Main(string[] args)
        {
        
            string line = "";
            string toNormalizedUnitFile = "toNormalized.txt";
            Dictionary<string, decimal> normalizedUnitScaleRate=new Dictionary<string, decimal>();

            if (args.Length<2)
            {
                System.Console.WriteLine("Please enter the Input file name and Outupt file name.\n The usage is UnitConversion inputfile.txt, result.txt. You can put any input or result text file name. ");
   
            }
            else
            {
                // Read value of scale rate in which all units converted to normalized "g/L"
                try
                {
                    string measureUnit = "";
                    decimal valueToNormalized = 0;
                    Char delimiter = ' ';
                    

                    StreamReader toNormalizedScaleRate = File.OpenText(toNormalizedUnitFile);
                    line = toNormalizedScaleRate.ReadLine();

                    while (line!=null&&line.Length>0)
                    {
                        string[] substrings = line.Split(delimiter);
                        if(substrings.Length!=2)
                        {
                            Console.WriteLine("The line: '{0}' of the toNormalized file is not correct",line);
                        }
                        else
                        {
                            if (substrings[0].Length == 0 || substrings[1].Length == 0)
                                Console.WriteLine("The line: '{0}' of the toNormalized file is not correct", line);
                            else
                            {
                                try
                                {
                                    valueToNormalized = Decimal.Parse(substrings[1]);
                                    // add this items to dictionary 
                                    normalizedUnitScaleRate.Add(substrings[0],valueToNormalized);

                                }
                                catch (FormatException error)
                                {
                                    Console.WriteLine("The line: '{0}' of the toNormalized file is not correct.", line);
                                }
                            }
                        }
                        line = toNormalizedScaleRate.ReadLine();
                    }
                    toNormalizedScaleRate.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine("An error occured while reading toNormalized.txt file: '{0}' ", e);
                }



                // read input file and write output file 
                try
                {
                    StreamReader inputFile = File.OpenText(args[0]);
                    StreamWriter outputFile = new StreamWriter(args[1]);

                    Char delimiter = ' ';
                    decimal currentData = 0;
                    string currentUnit = "";
                    decimal intendedData = 0;
                    string intendedUnit = "";

                    //each unit to normalized unit
                    decimal currentUnitScaleRate = 1;
                    decimal intendedUnitScaleRate = 1;

                    line = inputFile.ReadLine();

                    //Read the input file till reaching the end of file
                    while (line!=null&&line.Length>0)
                    {

                        string[] substrings = line.Split(delimiter);
                        if (substrings.Length != 3)
                        {
                            outputFile.WriteLine(currentData.ToString() + " " + currentUnit + " error ");
                            Console.WriteLine("The line: '{0}' of the input file is not correct", line);
                            

                        }
                        else
                        {
                            if (substrings[0].Length == 0 || substrings[1].Length == 0 || substrings[2].Length == 0)
                            {
                                outputFile.WriteLine(currentData.ToString() + " " + currentUnit + " error ");
                                Console.WriteLine("The line: '{0}' of the toNormalized file is not correct", line);
                                

                            }
                            else
                            {
                                //read current data of the line
                                try
                                {
                                    currentData = Decimal.Parse(substrings[0]);
                                }
                                catch (FormatException error)
                                {
                                    Console.WriteLine("The line: '{0}' of the toNormalized file is not correct.", line);
                                }

                                currentUnit = substrings[1];
                                intendedUnit = substrings[2];

                                //Lookup dictionary to calculate the scale rate to convert the current data from current unit to intended unit

                                // lookup current unit
                                if (normalizedUnitScaleRate.TryGetValue(currentUnit, out currentUnitScaleRate))
                                {
                                    if (normalizedUnitScaleRate.TryGetValue(intendedUnit, out intendedUnitScaleRate))
                                    {

                                        //formular for this conversion 
                                        intendedData = (1 / intendedUnitScaleRate) / (1 / currentUnitScaleRate) * currentData;
                                        outputFile.WriteLine(Math.Round(intendedData, 6).ToString() + " " + intendedUnit);

                                    }
                                    else
                                    {
                                        outputFile.WriteLine(currentData.ToString() + " " + currentUnit + " error ");
                                    }
                                }
                                else
                                {
                                    outputFile.WriteLine(currentData.ToString() + " " + currentUnit + " error ");
                                }


                            }
                        }
                       
                        line = inputFile.ReadLine();
                        
                    }
                    inputFile.Close();
                    outputFile.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine("An error occured while acessing the input file: '{0}'", e);
                }
                Console.WriteLine("The conversion is completed.");

            }

        }
    }
}
