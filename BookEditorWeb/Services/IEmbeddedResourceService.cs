namespace BookEditorWeb.Services
{
	public interface IEmbeddedResourceService
	{
		byte[] GetBinaryContent(string name);
	}
}