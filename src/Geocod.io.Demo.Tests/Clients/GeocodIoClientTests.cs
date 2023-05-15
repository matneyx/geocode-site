using System.Net;
using Geocod.io.Demo.Clients;
using Geocod.io.Demo.Endpoints;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Shouldly;
using Xunit;

namespace Geocod.io.Demo.Tests.Clients;



public class GeocodIoClientTests
{
    [Fact]
    public async void Can_generate_list_of_GeocodIoResults_from_list_of_GeocodIoAddresses()
    {
        var mockRestClient = new Mock<IRestClient>();

        var client = new GeocodIoClient(mockRestClient.Object);

        var geocodIoAddresses = new List<GeocodIoAddress>()
        {
            new() {Address = "660 Pennsylvania Ave SE", City = "Washington", State = "DC", Zip = "20003"},
            new() { Address = "1718 14th St NW", City="Washington", State = "DC", Zip="20009"},
            new() {Address = "1309 5th St NE", Zip = "20002"},
        };

        var stringList = geocodIoAddresses.Select(ga => ga.ToString()).ToArray();

        var request = new RestRequest("geocode", Method.Post);
        request.AddJsonBody(JsonConvert.SerializeObject(stringList));

        var mockResponse = new RestResponse
        {
            StatusCode = HttpStatusCode.OK,
            Content =
                "{'results':[" +
                "{'query':'660 Pennsylvania Ave SE, Washington, DC 20003'," +
                    "'response':{'input':{'address_components':{'number':'660','street':'Pennsylvania','suffix':'Ave','postdirectional':'SE','formatted_street':'Pennsylvania Ave SE','city':'Washington','state':'DC','zip':'20003','country':'US'},'formatted_address':'660 Pennsylvania Ave SE, Washington, DC 20003'}," +
                    "'results':[" +
                        "{'address_components':{'number':'660','street':'Pennsylvania','suffix':'Ave','postdirectional':'SE','formatted_street':'Pennsylvania Ave SE','city':'Washington','county':'District of Columbia','state':'DC','zip':'20003','country':'US'},'formatted_address':'660 Pennsylvania Ave SE, Washington, DC 20003','location':{'lat':38.885172,'lng':-76.996565},'accuracy':1,'accuracy_type':'rooftop','source':'City of Washington'}]}}," +
                "{'query':'1718 14th St NW, Washington, DC 20009'," +
                    "'response':{'input':{'address_components':{'number':'1718','street':'14th','suffix':'St','postdirectional':'NW','formatted_street':'14th St NW','city':'Washington','state':'DC','zip':'20009','country':'US'},'formatted_address':'1718 14th St NW, Washington, DC 20009'}," +
                    "'results':[" +
                        "{'address_components':{'number':'1718','street':'14th','suffix':'St','postdirectional':'NW','formatted_street':'14th St NW','city':'Washington','county':'District of Columbia','state':'DC','zip':'20009','country':'US'},'formatted_address':'1718 14th St NW, Washington, DC 20009','location':{'lat':38.913274,'lng':-77.032266},'accuracy':1,'accuracy_type':'rooftop','source':'City of Washington'}]}}," +
                "{'query':'1309 5th St NE, ,  20002'," +
                    "'response':{'input':{'address_components':{'number':'1309','street':'5th','suffix':'St','state':'NE','zip':'20002','country':'US'},'formatted_address':'1309 5th St, NE, 20002'}," +
                    // This address has four potential matches. The method should chose the most likely one.
                    "'results':[" +
                        "{'address_components':{'number':'1309','street':'5th','suffix':'St','postdirectional':'NE','formatted_street':'5th St NE','city':'Washington','county':'District of Columbia','state':'DC','zip':'20002','country':'US'},'formatted_address':'1309 5th St NE, Washington, DC 20002','location':{'lat':38.908698,'lng':-76.997878},'accuracy':0.9,'accuracy_type':'range_interpolation','source':'TIGER/Line® dataset from the US Census Bureau'}," +
                        // This is the one we want; it's intentionally out of order from an actual response, to test that the method sorts the results.
                        "{'address_components':{'number':'1309','street':'5th','suffix':'St','postdirectional':'NE','formatted_street':'5th St NE','city':'Washington','county':'District of Columbia','state':'DC','zip':'20002','country':'US'},'formatted_address':'1309 5th St NE, Washington, DC 20002','location':{'lat':38.908724,'lng':-76.997653},'accuracy':0.9,'accuracy_type':'rooftop','source':'City of Washington'}," +
                        "{'address_components':{'number':'1309','street':'5th','suffix':'St','postdirectional':'NE','formatted_street':'5th St NE','city':'Washington','county':'District of Columbia','state':'DC','zip':'20002','country':'US'},'formatted_address':'1309 5th St NE, Washington, DC 20002','location':{'lat':38.908857,'lng':-76.998133},'accuracy':0.8,'accuracy_type':'range_interpolation','source':'TIGER/Line® dataset from the US Census Bureau'}," +
                        "{'address_components':{'number':'1309','street':'5th','suffix':'St','postdirectional':'NW','formatted_street':'5th St NW','city':'Washington','county':'District of Columbia','state':'DC','zip':'20001','country':'US'},'formatted_address':'1309 5th St NW, Washington, DC 20001','location':{'lat':38.907613,'lng':-77.0187},'accuracy':0.2,'accuracy_type':'rooftop','source':'City of Washington'}]}}]}"
        };

        mockRestClient.Setup(rc => rc.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(mockResponse));

        var response = await client.GeocodeList(geocodIoAddresses);

        response.ShouldNotBeNull();
        response.Count().ShouldBe(3);
        response.First().FormattedAddress.ShouldBe("660 Pennsylvania Ave SE, Washington, DC 20003");

        var thirdAddress = response.ElementAt(2);
        thirdAddress.FormattedAddress.ShouldBe("1309 5th St NE, Washington, DC 20002");
        thirdAddress.Accuracy.ShouldBe(0.9);
        thirdAddress.AccuracyType.ShouldBe("rooftop");
    }
}
