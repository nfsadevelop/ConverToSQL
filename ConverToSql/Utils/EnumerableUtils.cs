namespace ConverToSql.Utils
{
    public static class EnumerableUtils
    {
        public static IEnumerable<string?> GenerateValueFromList(this IEnumerable<string?> list)
        {
            foreach (var item in list)
            {
                if (item == null)
                    yield return "NULL";

                else if (item.Equals("NULL"))
                    yield return item;

                else if (int.TryParse(item, out var intItem))
                    yield return intItem.ToString();

                else if (double.TryParse(item, out var doubleItem))
                    yield return doubleItem.ToString();

                else if (DateTime.TryParse(item, out var dateTimeItem))
                    yield return dateTimeItem.ToString("yyyy-MM-dd hh:mm:ss");

                else yield return $"'{item}'";
            }
        }
    }
}
