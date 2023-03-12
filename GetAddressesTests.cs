//-----------------------------------------------------------------------
// <copyright file="GetAddressesTests.cs" company="Procare Software, LLC">
//     Copyright © 2021-2023 Procare Software, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Procare.Address.IntegrationTests;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using static Procare.Address.IntegrationTests.GetAddressesResponse;

public class GetAddressesTests
{
    private readonly AddressService service = new AddressService(new Uri("https://address.dev-procarepay.com"));

    [Fact]
    public async Task GetAddresses_With_Owm_ShouldResultIn_OneMatchingAddress()
    {
        var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "1 W Main St", City = "Medford", StateCode = "OR" }).ConfigureAwait(false);

		Assert.NotNull(result);
		Assert.Equal(1, result.Count);
        Assert.NotNull(result.Addresses);
        Assert.Equal(result.Count, result.Addresses!.Count);
    }

    [Fact]
    public async Task GetAddresses_With_AmbiguousAddress_ShouldResultIn_MultipleMatchingAddresses()
    {
		var address = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "123 Main St", City = "Ontario", StateCode = "CA" }).ConfigureAwait(false);

		// TODO: Complete the test
		//throw new NotImplementedException();
        
		Assert.NotNull(address);
		Assert.True(address.Count > 1);
		Assert.True(address.Count == 15);
		Assert.NotNull(address.Addresses);
		Assert.Equal(address.Count, address.Addresses!.Count);
        Assert.Contains("123 W MAIN ST", address.Addresses.Select(x => x.Line1));
		Assert.Contains("ONTARIO", address.Addresses.Select(x => x.City));
		Assert.Contains("CA", address.Addresses.Select(x => x.StateCode));
		Assert.DoesNotContain("CO", address.Addresses.Select(x => x.StateCode));


	} 

	[Fact]
	public async Task CheckforInvalidCityName()
	{
		var city = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "123 Main St", City = "DenverInvalid" }).ConfigureAwait(false);

		//checking for valid address and invalid City name
		
		Assert.True(city.Count == 0);
		Assert.NotNull(city);
		Assert.Equal(city.Count, city.Addresses!.Count);
		

	}
}
