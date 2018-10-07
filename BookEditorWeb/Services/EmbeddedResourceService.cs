using System.IO;
using System.Reflection;

namespace BookEditorWeb.Services
{
	public class EmbeddedResourceService : IEmbeddedResourceService
	{
		public byte[] GetBinaryContent(string name)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream(name))
			{
				var bytes = new byte[stream.Length];
				stream.Read(bytes, 0, bytes.Length);
				return bytes;
			}
		}
	}
}