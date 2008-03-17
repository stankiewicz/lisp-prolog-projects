using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna.utils {
	class Common {
		public static bool TryMapDomainToRegion(Domain domain, Region region, double s, byte[,] bitmap, out Params parameters) {
			parameters = new Params();
			parameters.A = ((double)region.Size - 1) / ((double)domain.Size - 1);
			parameters.K = region.X - domain.X * parameters.A;
			parameters.D = parameters.A;
			parameters.L = region.Y - domain.Y * parameters.A;

			double[] B = new double[4];
			double[,] A = new double[,] { 
                  { domain.Left, domain.Bottom, domain.Left * domain.Bottom, 1, bitmap[region.Left, region.Bottom] - s * bitmap[domain.Left, domain.Bottom] },
                  { domain.Right, domain.Bottom, (domain.Right)*(domain.Bottom), 1, bitmap[region.Right, region.Bottom] - s * bitmap[domain.Right, domain.Bottom]},
                  { domain.Left, domain.Top, domain.Left*(domain.Top), 1, bitmap[region.Left, region.Top] - s * bitmap[domain.Left, domain.Top]},
                  { domain.Right, domain.Top, (domain.Right)*(domain.Top), 1, bitmap[region.Right, region.Top] - s * bitmap[domain.Right, domain.Top]}
           };
			if (LinearEquationSolver.GaussianElimination(A, B)) {
				parameters.E = B[0];
				parameters.G = B[1];
				parameters.H = B[2];
				parameters.M = B[3];
				return true;
			} else {
				throw new Exception("Macierz osobliwa");
			}
		}

		/// <summary>
		/// Stara metoda
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="region"></param>
		/// <param name="s"></param>
		/// <param name="bitmap"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		private bool TryMapDomainToRegion2(Domain domain, Region region, double s, int[,] bitmap, out double[] parameters) {

			/* w tym miejscu wydaje mi sie ze trzeba wyliczyc 8 rownan z 8 niewiadomymi. niewiadome to
			 * aij, kij, dij, lij, eij,gij,hij,mij. - rownania  pod koniec 2giej strony.
			 * 
			 * 
			 */
			double[,] matrix = new double[9, 8];

			for (int i = 0; i < 9; ++i)
				for (int j = 0; j < 8; ++j)
					matrix[i, j] = 0;

			//dla przypomnienia - matrix [kolumna, wiersz]

			// lewy gorny rog 
			matrix[0, 0] = domain.X;
			matrix[1, 0] = 1;
			matrix[8, 0] = region.X;

			matrix[2, 1] = domain.Y;
			matrix[3, 1] = 1;
			matrix[8, 1] = region.Y;

			matrix[4, 2] = domain.X;
			matrix[5, 2] = domain.Y;
			matrix[6, 2] = domain.X * domain.Y;
			matrix[7, 2] = 1;
			matrix[8, 2] = (double)bitmap[region.X, region.Y] - s * (double)bitmap[domain.X, domain.Y];

			matrix[0, 3] = domain.X + domain.Width;
			matrix[1, 3] = 1;
			matrix[8, 3] = region.X + region.Width;

			matrix[2, 4] = domain.Y + domain.Height;
			matrix[3, 4] = 1;
			matrix[8, 4] = region.Y + region.Height;

			// prawy gorny
			matrix[4, 5] = domain.X + domain.Width;
			matrix[5, 5] = domain.Y;
			matrix[6, 5] = (domain.X + domain.Width) * domain.Y;
			matrix[7, 5] = 1;
			matrix[8, 5] = (double)bitmap[region.X + region.Width, region.Y] - s * (double)bitmap[domain.X + domain.Width, domain.Y];

			//lewy dolny
			matrix[4, 6] = domain.X;
			matrix[5, 6] = domain.Y + domain.Height;
			matrix[6, 6] = domain.X * (domain.Y + domain.Height);
			matrix[7, 6] = 1;
			matrix[8, 6] = (double)bitmap[region.X, region.Y + region.Height] - s * (double)bitmap[domain.X, domain.Y + domain.Height];

			// prawy dolny
			matrix[4, 7] = domain.X + domain.Width;
			matrix[5, 7] = domain.Y + domain.Height;
			matrix[6, 7] = (domain.X + domain.Width) * (domain.Y + domain.Height);
			matrix[7, 7] = 1;
			matrix[8, 7] = (double)bitmap[region.X + region.Width, region.Y + region.Height] - s * (double)bitmap[domain.X + domain.Width, domain.Y + domain.Height];

			parameters = new double[8];
			for (int i = 0; i < 8; ++i) {
				parameters[i] = 0;
			}
			return LinearEquationSolver.GaussianElimination(matrix, parameters);
		}
	}
}
