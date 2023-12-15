using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HR_Project.Application.Operations
{
	public class NameOperation
	{
		public static string CharacterRegulatory(string name)
		{
			// Türkçe karakterleri İngilizce alfabesine çevir
			string normalized = name.Normalize(NormalizationForm.FormD);
			string removedDiacritics = Regex.Replace(normalized, @"\p{M}", "");

			// Özel karakterleri kaldır
			string removedSpecialChars = Regex.Replace(removedDiacritics, @"[^a-zA-Z0-9\s-]", "");

			// Boşlukları ve noktaları "-" ile değiştir
			string replacedSpacesAndDots = Regex.Replace(removedSpecialChars, @"[\s.]+", "-");

			// Türkçe karakterleri küçük harfe çevir
			string lowercased = replacedSpacesAndDots.ToLower();

			return lowercased;
		}
	}
}
