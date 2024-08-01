using Microsoft.Data.Sqlite;

namespace api.Models
{
   public class SwiftMessageRepository
{
    private readonly string _connectionString;

    public SwiftMessageRepository(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS SwiftMessages (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Header TEXT,
                ApplicationHeader TEXT,
                TextBody TEXT,
                Trailer TEXT,
                TrailerEnd TEXT
            );
        ";
        command.ExecuteNonQuery();
    }

    public void SaveSwiftMessage(SwiftMessage message)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO SwiftMessages (Header, ApplicationHeader, TextBody, Trailer, TrailerEnd)
            VALUES ($header, $applicationHeader, $textBody, $trailer, $trailerEnd);
        ";
        command.Parameters.AddWithValue("$header", message.Header ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$applicationHeader", message.ApplicationHeader ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$textBody", message.TextBody ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$trailer", message.Trailer ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$trailerEnd", message.TrailerEnd ?? (object)DBNull.Value);

        command.ExecuteNonQuery();
    }
    }
}