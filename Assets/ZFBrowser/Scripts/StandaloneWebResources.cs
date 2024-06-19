using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

/** 
 * Implements fetching BrowserAssets for standalone builds.
 * 
 * During build, everything in BrowserAssets is packaged into a single file with the following format:
 *   brString {FileHeader}
 *   i32 numEntries
 *   {numEntries} IndexEntry objects
 *   data //The data section is a series of filedata chunks, as laid out in the index.
 */
public class StandaloneWebResources : WebResources {
	public struct IndexEntry {
		public string name;
		public long offset;
		public int length;
	}

	private const string FileHeader = "zfbRes_v1";

	protected Dictionary<string, IndexEntry> toc = new Dictionary<string, IndexEntry>();
	protected string dataFile;

	public StandaloneWebResources(string dataFile) {
		this.dataFile = dataFile;
	}

	public const string DefaultPath = "Resources/browser_assets";

	public void LoadIndex() {
		using (var data = new BinaryReader(File.OpenRead(dataFile))) {
			var header = data.ReadString();
			if (header != FileHeader) throw new Exception("Invalid web resource file");

			var num = data.ReadInt32();

			for (int i = 0; i < num; ++i) {
				var entry = new IndexEntry() {
					name = data.ReadString(),
					offset = data.ReadInt64(),
					length = data.ReadInt32(),
				};
				toc[entry.name] = entry;
			}
		}
	}

	public override byte[] GetData(string path) {
		IndexEntry entry;
		if (!toc.TryGetValue(path, out entry)) {
			return null;
		}

		using (var file = File.OpenRead(dataFile)) {
			file.Seek(entry.offset, SeekOrigin.Begin);
			var data = new byte[entry.length];
			var readLen = file.Read(data, 0, entry.length);
			if (readLen != data.Length) throw new Exception("Insufficient data for file");

			return data;
		}
	}

	public void WriteData(Dictionary<string, byte[]> files) {
		var entries = new Dictionary<string, IndexEntry>();

		using (var file = File.OpenWrite(dataFile)) {
			var writer = new BinaryWriter(file, Encoding.UTF8 /*, true (Mono too old)*/);
			writer.Write(FileHeader);
			writer.Write(files.Count);

			var tocStart = file.Position;

			foreach (var kvp in files) {
				writer.Write(kvp.Key);
				writer.Write(0L);
				writer.Write(0);
			}
			//we'll come back and fill it in right later

			foreach (var kvp in files) {
				var data = kvp.Value;
				var entry = new IndexEntry {
					name = kvp.Key,
					length = kvp.Value.Length,
					offset = file.Position,
				};

				writer.Write(data);
				entries[kvp.Key] = entry;
			}

			//now go back and write the correct data.
			writer.Seek((int)tocStart, SeekOrigin.Begin);

			foreach (var kvp in files) {
				var entry = entries[kvp.Key];
				writer.Write(kvp.Key);
				writer.Write(entry.offset);
				writer.Write(entry.length);
			}
		}
	}

}

}
