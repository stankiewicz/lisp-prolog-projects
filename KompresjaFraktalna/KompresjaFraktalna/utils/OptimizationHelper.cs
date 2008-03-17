using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace KompresjaFraktalna.utils {
	/// <summary>
	/// Ta klasa, ze wzglêdu na wielow¹tkowy dostêp najlepiej jeœli nie bêdzie statyczna.
	/// </summary>
	class OptimizationHelper {
		Dictionary<ushort, byte[,]> tableCache = new Dictionary<ushort, byte[,]>();

		/// <summary>
		/// Zwracana tablica dwuwymiarowa o rozmiarze 'size' mo¿e byæ niepusta.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public byte[,] getTable(ushort size) {
			Debug.Assert(size > 0, "Rozmiar tablicy jest za ma³y");
			byte[,] table;
			bool res = tableCache.TryGetValue(size, out table);
			if (!res) {
				table = new byte[size, size];
				tableCache[size] = table;
			}
			return table;
		}
	}
}
