using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReflectionTests
{
    class Program
    {
        static void Main(string[] args)
        {
            string strDllPath = Path.GetFullPath("Countries.dll");
            var assembly = Assembly.LoadFile(strDllPath);
            var countryTypes = assembly.GetTypes();
           foreach(var t in countryTypes)
            {
                Console.WriteLine(t.FullName);
            }
            //PrintInterfaceInfo(ref assembly);
            //PrintCountryDetails(ref countryTypes, ref assembly);
            //PrintTopSecretMethods(ref countryTypes);

        }

        public static void PrintTopSecretMethods(ref Type[] countryTypes)
        {
            if (countryTypes == null)
            {
                Console.WriteLine("No Countries");
                return;
            }
            dynamic topSecretAttr;
            foreach (var t in countryTypes)
            {
                if (t.Name == "TopSecretAttribute")
                {
                    topSecretAttr = t;
                    break;
                }
            }
            foreach (var t in countryTypes)
            {
                if (t.FullName.Contains("Countries"))
                {
                    var members = t.GetMembers();
                    foreach (var member in members)
                    {

                        var customAttrArray = member.CustomAttributes.ToList();
                        foreach (var attr in customAttrArray)
                        {
                            Console.WriteLine($"{t.Name}, {member.Name}, {attr.AttributeType}");
                        }
                        
                    }
                }
            }
        }

        public static void PrintCountryDetails(ref Type[] countryTypes, ref Assembly assembly)
        {
            if (countryTypes == null)
            {
                Console.WriteLine("No Countries");
                return;
                
            }
            foreach (var t in countryTypes)
            {
                if (t.FullName.Contains("Countries.Countries"))

                {
                    object country = assembly.CreateInstance(t.FullName);
                    MethodInfo getNameMethod = t.GetMethod("get_Name");
                    MethodInfo getPopulationMethod = t.GetMethod("get_Population");
                    MethodInfo calculateSizeMethod = t.GetMethod("CalculateSize");
                    if (country != null)
                    {
                        var nameResult = getNameMethod.Invoke(country, null);
                        var populationResult = getPopulationMethod.Invoke(country, null);
                        var sizeResult = calculateSizeMethod.Invoke(country, null);
                        Console.WriteLine($"{t.Name}:");
                        Console.WriteLine($"Name: {nameResult.ToString()}");
                        Console.WriteLine($"Population: {populationResult.ToString()}");
                        Console.WriteLine($"Size: {sizeResult.ToString()}");
                        Console.WriteLine("==========================");
                    }
                    else { Console.WriteLine("No such method/country"); }
                }
            }
        }

        public static void PrintInterfaceInfo(ref Assembly assembly)
        {
            var countryInterface = assembly.GetType("Countries.Interfaces.ICountry");
            var members = countryInterface.GetMembers();
            foreach (var m in members)
            {
                Console.WriteLine($"Member Type: {m.MemberType}, Name: {m.Name}");
            }
        }
    }
}
