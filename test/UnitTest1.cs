using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using filmstudion.api;
using System.Net.Http.Json;
using System.Linq;
using filmstudion.api.Data;
using filmstudion.api.Helpers;
using filmstudion.api.Models;
using filmstudion.api.Services;
using Microsoft.AspNetCore.Mvc;
using filmstudion.api.Controllers;

namespace test;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient httpClient;

    public IntegrationTests(WebApplicationFactory<Startup> factory)
    {
        httpClient = factory.CreateClient();
    }

    //Kollar så att responsecode 200 returneras på GET
    [Fact]
    public async Task TestThatEndPointReturnsStatuscodeOK()
    {
        //Arrange
        var response = await httpClient.GetAsync($"https://localhost:5001/Films");
        //Act
        var expectedResponseCode = System.Net.HttpStatusCode.OK;
        var actualResponseCode = response.StatusCode;
        //Assert
        Assert.Equal(expectedResponseCode, actualResponseCode);
    }

    //Kollar så att rätt antal förinmatade filmer returneras.
    [Fact]
    public async Task TestThatEndpointReturnsCorrectNrOfFilms()
    {
        //Arrange
        var response = await httpClient.GetFromJsonAsync<IEnumerable<Film>>($"https://localhost:5001/Films");
        //Act
        var actualNrOfFilms = response.Count();
        var expectedNrOfFilms = 3;
        //Assert
        Assert.Equal(expectedNrOfFilms, actualNrOfFilms);
    }

    //Försöker ta bort film som inte finns.
    [Fact]
    public void TestThatMethodReturnsFalseWhenTryingToDeleteNoneExistingCopy()
    {
        //Arrange
        FilmCopyRepository fcr = new(null);
        //Act
        var expectedResult = false;
        var actualResult = fcr.DeleteFilmCopyFromId("doesn't exist");
        //Assert
        Assert.Equal(expectedResult, actualResult);
    }

    //Försöker skapa en film utan tillstånd
    [Fact]
    public async Task TestCreateFilmWithotAuthorization()
    {
        //Arrange
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "Films/create");
        var formModel = new Dictionary<string, string>
        {
            {"Name", "End Of All"},
            {"ReleaseDate", "2022-03-09T15:31:11.266Z"},
            {"Country", "USA"},
            {"Director", "James Brask"},
            {"NumberOfCopies", "0"}
        };
        postRequest.Content = new FormUrlEncodedContent(formModel);
        //Act
        var response = await httpClient.SendAsync(postRequest);
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    //Testar så att korrekt information om registrerad filmstudio returneras.
    [Fact]
    public async Task TestThatResponseContainsCorrectInformation()
    {
        //Arrange
        var response = await httpClient.GetAsync("/FilmStudios");
        //Act
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        //Assert
        Assert.Contains("studio1", responseString);
        Assert.Contains("Göteborg", responseString);
        Assert.Contains("pres1", responseString);
    }

    //Testar så att lista med filmer returneras
    [Fact]
    public async Task TestThatReturnedListIsOfTypeFilm()
    {
        //Arrange
        var response = await httpClient.GetFromJsonAsync<IEnumerable<Film>>($"https://localhost:5001/Films");
        //Act
        var result = response.ToList();
        //Assert
        Assert.IsType<List<Film>>(result);
    }

}