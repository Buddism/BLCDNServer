using System.Data;
using Microsoft.Data.Sqlite;

namespace BLCDNParser;
struct CacheResult
{
	readonly public string hash;
	readonly public MemoryStream blob;
	public CacheResult(string hash, MemoryStream blob)
	{
		this.hash = hash;
		this.blob = blob;
	}
};

internal class Cache
{
	readonly public SqliteConnection conn;
	readonly public bool openedSuccessfully;
	public Cache(string fileName)
	{
		conn = new SqliteConnection(string.Format("Data Source={0}", fileName));

		if (conn.State != ConnectionState.Open)
		{
			conn.Open();
			openedSuccessfully = true;
		}
		else openedSuccessfully = false;
	}

	/// <summary>
	/// reads a row of the sqlite db (string hash, MemoryStream blob)
	/// </summary>
	/// <returns>CacheResult, null if no rows left</returns>
	public List<CacheResult> getRows()
	{
		using SqliteCommand command = conn.CreateCommand();
		command.CommandText =
		@"
			SELECT hash, data
			FROM blobs
		";

		using SqliteDataReader reader = command.ExecuteReader();
		List<CacheResult> result = new();

		while (reader.Read())
		{
			string hash = reader.GetString(0)[..41];
			MemoryStream blob = read_blob(reader, 1);

			result.Add(new CacheResult(hash, blob));
		}

		return result;
	}

	public MemoryStream? getHashBlob(string hash)
	{
		using SqliteCommand command = conn.CreateCommand();
		command.CommandText =
		@"
			SELECT data
			FROM blobs
			WHERE hash = $hash
		";

		var pHash = command.CreateParameter();
		pHash.ParameterName = "hash";
		command.Parameters.Add(pHash);

		pHash.Value = hash;

		var reader = command.ExecuteReader();

		if (!reader.Read())
			return null;

		return read_blob(reader, 0);
	}
	private MemoryStream read_blob(SqliteDataReader reader, int column)
	{
		MemoryStream stream = new();
		byte[] buffer = new byte[1024 * 8];
		long fieldOffset = 0;

		long bytesRead;
		while ((bytesRead = reader.GetBytes(column, fieldOffset, buffer, 0, buffer.Length)) > 0)
		{
			stream.Write(buffer, 0, (int)bytesRead);
			fieldOffset += bytesRead;
		}

		return stream;
	}
}
