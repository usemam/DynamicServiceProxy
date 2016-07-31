using System;

using DynamicServiceProxy.Contracts;

using NUnit.Framework;

namespace DynamicServiceProxy.Client.Tests
{
	[TestFixture]
    public class InvocationTests
	{
	    private IUserService _service;

	    [OneTimeSetUp]
	    public void Setup()
	    {
	        var proxy = new ClientUserService();
	        this._service = proxy.Interface;
	    }

	    [Test]
	    public void Implemented_method_works_fine()
	    {
	        var result = this._service.Authenticate("test", "test");
			Assert.IsNotNull(result);
	    }

	    [Test]
	    public void Not_implemented_method_throws_exception()
	    {
	        const int userId = 1;
	        const int officeId = 2;
	        Assert.Throws<NotImplementedException>(
				() => this._service.ChangeUserOffice(userId, officeId));
	    }
	}
}