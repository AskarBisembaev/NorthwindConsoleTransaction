using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthwindConsoleTransaction
{
	class Program
	{
		static async Task Main(string[] args)
		{
			string connectionString = "Data Source = .\\sqlexpress; Initial Catalog=Northwind; Integrated Security = SSPI";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlTransaction transaction = connection.BeginTransaction();

				SqlCommand command = connection.CreateCommand();
				command.Transaction = transaction;

				try
				{
					command.CommandText = "INSERT INTO Categories (CategoryName) VALUES('Tim')";
					command.ExecuteNonQuery();
					command.CommandText = "INSERT INTO Employees (LastName, FirstName) VALUES('Vasya', 'Pupkin')";
					command.ExecuteNonQuery();

					transaction.Commit();
					Console.WriteLine("Данные добавлены в базу данных");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					transaction.Rollback();
				}
			}
			Console.Read();
		}
	}
}