using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CsvRecord
{
    public List<object> Fields { get; private set; }

    public CsvRecord()
    {
        Fields = new List<object>();
    }

    public void Parse(string line, char delimiter = ',')
    {
        Fields.Clear();
        var fieldBuilder = new StringBuilder();
        bool insideQuotes = false;
        int i = 0;

        while (i < line.Length)
        {
            char c = line[i];

            if (c == '"')
            {
                if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    fieldBuilder.Append('"');
                    i++;
                }
                else
                {
                    insideQuotes = !insideQuotes; 
                }
            }
            else if (c == delimiter && !insideQuotes)
            {
                AddField(fieldBuilder.ToString());
                fieldBuilder.Clear();
            }
            else
            {
                fieldBuilder.Append(c);
            }

            i++;
        }

        AddField(fieldBuilder.ToString());
    }

    private void AddField(string field)
    {
        field = field.Trim();
        if (int.TryParse(field, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intVal))
        {
            Fields.Add(intVal);
        }
        else if (double.TryParse(field, NumberStyles.Float, CultureInfo.InvariantCulture, out double dblVal))
        {
            Fields.Add(dblVal);
        }
        else
        {
            Fields.Add(field);
        }
    }
}


namespace _13__CSV_Line_Parser_with_Quotes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string csvLine = "123,\"John, Smith\",456.78,\"He said \"\"Hello\"\" to me\"";
            char delimiter = ',';

            var record = new CsvRecord();
            record.Parse(csvLine, delimiter);

            Console.WriteLine($"Field count: {record.Fields.Count}");
            Console.WriteLine("Parsed fields:");

            foreach (var field in record.Fields)
            {
                Console.WriteLine($" - {field} (Type: {field.GetType().Name})");
            }
        }
    }
}




