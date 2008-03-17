using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace KompresjaFraktalna.utils {
	/// <summary>
	/// Ta klasa, ze wzgl�du na wielow�tkowy dost�p najlepiej je�li nie b�dzie statyczna.
	/// </summary>
	class OptimizationHelper {
		Dictionary<ushort, byte[,]> tableCache = new Dictionary<ushort, byte[,]>();

		/// <summary>
		/// Zwracana tablica dwuwymiarowa o rozmiarze 'size' mo�e by� niepusta.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public byte[,] getTable(ushort size) {
			Debug.Assert(size > 0, "Rozmiar tablicy jest za ma�y");
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
