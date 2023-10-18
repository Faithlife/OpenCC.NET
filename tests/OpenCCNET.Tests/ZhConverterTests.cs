using NUnit.Framework;

namespace OpenCCNET.Tests
{
	[TestFixture]
	internal class ZhConverterTests
	{
		[OneTimeSetUp]
		public void SetUp()
		{
			ZhConverter.Initialize();
		}

		[Test]
		public void HansToFromTw()
		{
			const string simplified = "圣经研究的观点林林总总";
			const string traditional = "聖經研究的觀點林林總總";
			Assert.AreEqual(traditional, ZhConverter.HansToTW(simplified));
			Assert.AreEqual(traditional, ZhConverter.HansToTW(traditional));
			Assert.AreEqual(simplified, ZhConverter.TWToHans(traditional));
			Assert.AreEqual(simplified, ZhConverter.TWToHans(simplified));
		}
	}
}
