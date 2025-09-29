using System.Data;

using Dapper;

namespace Cracker_Shop.Code_Generator
{
    public static class CodeGenerator
    {
        /// <summary>
            /// Generate next code for any table column in the format PREFIX + padded number.
            /// Example: COM00001, BRN00001
            /// </summary>
            /// <param name="db">IDbConnection</param>
            /// <param name="tableName">Table name</param>
            /// <param name="columnName">Column name</param>
            /// <param name="prefix">Code prefix like COM, BRN</param>
            /// <param name="padding">Number of digits, e.g., 5 → 00001</param>
            /// <returns>Next code string</returns>
            public static async Task<string> GenerateNextCodeAsync(
                IDbConnection db,
                string tableName,
                string columnName,
                string prefix,
                int padding = 5)
            {
                if (db == null) throw new ArgumentNullException(nameof(db));
                if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentException("Table name required");
                if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentException("Column name required");
                if (string.IsNullOrWhiteSpace(prefix)) throw new ArgumentException("Prefix required");

                // Get last code
                string sql = $@"
                SELECT TOP 1 {columnName} 
                FROM {tableName} 
                WHERE {columnName} LIKE @Prefix + '%'
                ORDER BY {columnName} DESC";

                var lastCode = await db.QueryFirstOrDefaultAsync<string>(sql, new { Prefix = prefix });

                int nextNumber = 1;

                if (!string.IsNullOrEmpty(lastCode) && lastCode.StartsWith(prefix))
                {
                    var numPart = lastCode.Substring(prefix.Length);
                    if (int.TryParse(numPart, out int lastNumber))
                        nextNumber = lastNumber + 1;
                }

                return $"{prefix}{nextNumber.ToString($"D{padding}")}";
            }
        }
    


}
