using ClosedXML.Excel;
using ConverToSql.Utils;

Console.WriteLine("Welcome to ConverToSql!");

var filePath = string.Empty;
try
{
    filePath = args.First();
}
catch (Exception e)
{
    Console.WriteLine("Failed to get the file, the file path may be invalid or the file may be in use by another process!");
    return;
}



var tableName = string.Empty;
try
{
    tableName = args[Array.IndexOf(args, args.First(x => x.Equals("-t"))) + 1];
}
catch (Exception e)
{
    Console.WriteLine("Failed to get the table name, you must provide a valid parameter (example: -t \"Tb_Users\")!");
    return;
}

Console.WriteLine($"Generating SQL INSERT query from file: {filePath}");
Console.WriteLine($"Table Name: {tableName}");

var xls = new XLWorkbook(args.First());
var sheet = xls.Worksheets.First();
var totalRows = sheet.Rows().Count();
var totalColumns = sheet.Columns().Count();
var headerCells = new List<string>();

var query = new List<string>();

for (int r = 1; r <= totalRows; r++)
{
    if (r == 1)
    {
        for (int c = 1; c <= totalColumns; c++)
        {
            headerCells.Add(sheet.Cell(r, c).Value.ToString());
        }

        query.Add($"INSERT INTO {tableName} ([{string.Join("], [", headerCells)}]) \nVALUES \t");

        continue;
    }

    var currentLine = new List<string>();

    for (int c = 1; c <= totalColumns; c++)
    {
        currentLine.Add(sheet.Cell(r, c).Value.ToString());
    }

    query.Add($"\t({string.Join(", ", currentLine.GenerateValueFromList())})" + (r != totalRows ? ", " : ";"));
}

var extension = Path.GetExtension(filePath);
await File.WriteAllLinesAsync(filePath.Replace(extension, ".sql"), query);

Console.WriteLine($"Finished, SQL query file saved in: {filePath.Replace(extension, ".sql")}");