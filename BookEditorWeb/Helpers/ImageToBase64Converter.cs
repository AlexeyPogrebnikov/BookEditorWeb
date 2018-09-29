using System.Drawing;
using System.IO;

namespace BookEditorWeb.Helpers
{
	public static class ImageToBase64Converter
	{
		public static string Convert(string imagePath)
		{
			using (Image image = Image.FromFile(imagePath))
			{
				using (var stream = new MemoryStream())
				{
					image.Save(stream, image.RawFormat);
					byte[] imageBytes = stream.ToArray();
					string base64String = System.Convert.ToBase64String(imageBytes);
					return base64String;
				}
			}
		}
	}
}