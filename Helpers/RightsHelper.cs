using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _3CX_API_20.Helpers
{
    public class RightsHelper
    {
        /// <summary>
        /// Zwraca słownik (Dictionary), w którym kluczem jest nazwa flagi (np. "CanSeeGroupCalls"), 
        /// a wartością – jej ustawienie (true/false).
        /// </summary>
        /// <summary>
        /// Wyszukuje w tablicy "value" obiekt o polu "Id" == wantedId,
        /// a następnie zwraca słownik klucz-flaga (tylko wartości bool).
        /// Gdy nie znajdzie takiego obiektu, zwraca null.
        /// </summary>
        public static Dictionary<string, bool>? GetRightsFlagsForId(string jsonString, string wantedId)
        {
            using var doc = JsonDocument.Parse(jsonString);

            // Zakładamy, że na poziomie root jest "@odata.context" i "value"
            var root = doc.RootElement;
            var valueArray = root.GetProperty("value"); // jest to tablica obiektów

            foreach (var item in valueArray.EnumerateArray())
            {
                // Każdy item powinien mieć "Id"
                var itemId = item.GetProperty("Number").GetString();

                if (itemId == wantedId)
                {
                    // Jeśli znaleźliśmy właściwy obiekt, pobieramy "Rights"
                    var rightsElement = item.GetProperty("Rights");

                    // Zbudujemy słownik, gdzie nazwa flagi -> bool
                    var flags = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

                    foreach (var property in rightsElement.EnumerateObject())
                    {
                        // Jeśli pole jest typu bool
                        if (property.Value.ValueKind == JsonValueKind.True ||
                            property.Value.ValueKind == JsonValueKind.False)
                        {
                            flags[property.Name] = property.Value.GetBoolean();
                        }
                    }

                    return flags;
                    // Kończymy, bo znaleźliśmy usera z żądanym ID
                }
            }

            // Jeśli pętla się zakończyła i nie trafiliśmy na wantedId, zwracamy null
            return null;
        }
    }
}
