using System.Text.Json;
using System.Xml.Linq;

namespace Task_1_JSON_to_XML
{
    internal class Program
    {
        public static XElement JsonToXml(JsonElement element, string nameElement) //создаем метод для преобразования JSON в XML
        {
            nameElement = nameElement.Replace(" ", "_"); // Проверяем имя элемента. Если в нем есть пробелы меняем их на _

            if (element.ValueKind == JsonValueKind.Object) // проверяем типа объекта. Сравниваем тип элемента с эталонным значением из множества enum JsonValueKind
            {
                XElement xml = new XElement(nameElement);
                foreach (JsonProperty prop in element.EnumerateObject()) // пребираем все свойства объекта
                {
                    xml.Add(JsonToXml(prop.Value, prop.Name)); //при помощи рекурсии преобразуем свойства в XML и добавляем к текущему элементу 
                }
                return xml;
            }

            if (element.ValueKind == JsonValueKind.Array) // проверяем типа объекта
            {
                XElement xml = new XElement(nameElement);
                foreach (JsonElement _element in element.EnumerateArray())  
                {
                    xml.Add(JsonToXml(_element, nameElement));
                }
                return xml;
            }
            else
            {
                return new XElement(nameElement, element.ToString()); // Если элемент не является объектом или массивом создаем обычный элемент
            }
        }

        static void Main(string[] args)
        {
            var jsonMain = """
                 {
                    "Current": {
                        "Time": "2023-06-18T20:35:06.722127+04:00",
                        "Temperature": 29,
                        "Weather code": 1,
                        "Wind speed": 2.1,
                        "Wind direction": 1
                    },
                    "History": [
                        {
                            "Time": "2023-06-17T20:35:06.77707+04:00",
                            "Temperature": 29,
                            "Weather code": 2,
                            "Wind speed": 2.4,
                            "Wind direction": 1
                        },
                        {
                            "Time": "2023-06-16T20:35:06.777081+04:00",
                            "Temperature": 22,
                            "Weather code": 2,
                            "Wind speed": 2.4,
                            "Wind direction": 1
                        },
                        {
                            "Time": "2023-06-15T20:35:06.777082+04:00",
                            "Temperature": 21,
                            "Weathercode": 4,
                            "Wind speed": 2.2,
                            "Wind direction": 1
                        }
                    ]
                }
                """;

            using var jsonDocument = JsonDocument.Parse(jsonMain);
            var root = jsonDocument.RootElement;
            XDeclaration xmlDec = new XDeclaration("version 1.0", "UTF-8", null); // Добавляем декларацию
            XElement xml = JsonToXml(root, "root");        
            
            Console.WriteLine($"{xmlDec}\n{xml}");
            Console.ReadLine();
        }
    }
}
