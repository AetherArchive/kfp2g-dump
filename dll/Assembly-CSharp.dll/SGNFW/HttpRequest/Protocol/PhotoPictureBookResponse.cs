using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoPictureBookResponse : Response
	{
		public List<PictureBookPhoto> photoList;
	}
}
