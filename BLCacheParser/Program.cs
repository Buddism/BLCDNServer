using ICSharpCode.SharpZipLib.BZip2;

namespace BLCDNParser;
static class CDNCacheParser
{
	public static void Main(string[] input)
	{
		if (input.Length == 0)
		{
			Console.WriteLine("did not recieve an input cache.db");
			Console.ReadLine();
			return;
		}

		string file = input[0];

		Cache cache = new(file);
		if(!cache.openedSuccessfully)
		{
			Console.WriteLine("not a valid sqlite database?");
			Console.ReadLine();
			return;
		}

		//create the blobs in the folder with the executable
		System.IO.Directory.CreateDirectory("blobs");

		int badCompressionCount = 0;
		int items_parsed = 0;

		object fileWriteLock = new();

		List<CacheResult> items = cache.getRows();
		Parallel.For(0, items.Count, i =>
		{
			CacheResult item = items[i];

			using MemoryStream compressedStream = new();
			using MemoryStream inputStream = item.blob;
			inputStream.Position = 0;

			BZip2.Compress(inputStream, compressedStream, false, 9);

			lock (fileWriteLock)
			{
				items_parsed++;
				float percentComplete = (float)items_parsed / (float)items.Count * 100.0f;
				Console.Write($"\r{percentComplete:0.00}%");

				string fileName = "blobs/" + item.hash[..40] + ".bz2";
				using FileStream fs = new(fileName, FileMode.Create, FileAccess.Write);
				fs.Write(compressedStream.GetBuffer(), 0, compressedStream.GetBuffer().Length);

				if (compressedStream.Length == 14)
					badCompressionCount++;
			}
		});

		Console.Write("\r");

		if (badCompressionCount > 0)
			Console.WriteLine("Might have failed to compress: {0} files", badCompressionCount);
	}
}
