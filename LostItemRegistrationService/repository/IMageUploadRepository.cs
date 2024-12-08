

using LostItemRegistrationService.model;

namespace LostItemRegistrationService.repository
{
	public interface IMageUploadRepository
	{
		Task<Image> UploadImage(Image image);
	}
}
