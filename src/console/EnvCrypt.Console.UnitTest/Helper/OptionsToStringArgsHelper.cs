using System;
using System.Collections.Generic;
using System.Reflection;
using CommandLine;

namespace EnvCrypt.Console.UnitTest.Helper
{
    public static class OptionsToStringArgsHelper
    {
        public static string[] GetArgs(object fromOptionsObject)
        {
            var optionsType = fromOptionsObject.GetType();
            var verbAttribute = optionsType.GetCustomAttribute<VerbAttribute>();
            if (verbAttribute == null)
            {
                throw new Exception("verb attribute not present on class " + optionsType.FullName);
            }

            var ret = Create(verbAttribute);

            foreach (var typeProp in optionsType.GetProperties())
            {
                var attribute = typeProp.GetCustomAttribute<OptionAttribute>();
                if (attribute == null)
                {
                    continue;
                }
                var objForProp = typeProp.GetValue(fromOptionsObject);
                AddNewEntry(ret, attribute, objForProp);
            }

            return ret.ToArray();
        }


        private static List<string> Create(VerbAttribute verb)
        {
            return new List<string>()
            {
                verb.Name
            };
        }


        private static void AddNewEntry(List<string> list, OptionAttribute option, object value)
        {
            if (value == null)
            {
                return;
            }
            var valueString = value.ToString();
            if (string.IsNullOrWhiteSpace(valueString))
            {
                return;
            }

            list.Add("--" + option.LongName);
            list.Add(valueString);
        }
    }
}
