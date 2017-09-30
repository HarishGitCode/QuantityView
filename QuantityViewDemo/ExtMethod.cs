/*
 * From https://stackoverflow.com/a/11654221 by Jeff Mercado
 * 
 */

using System.Collections.Generic;
using System.Text;

namespace QuantityViewDemo
{
	internal static class ExtMethod
	{
	    public static string StringBuilderChars(this IEnumerable<char> charSequence)
		{
			var sb = new StringBuilder();
			foreach (var c in charSequence)
			{
				sb.Append(c);
			}
			return sb.ToString();
		}

	    public static Java.Lang.ICharSequence ToJavaCharSequence(this IEnumerable<char> charSequence)
            => new Java.Lang.String(charSequence.StringBuilderChars());
	}
}
