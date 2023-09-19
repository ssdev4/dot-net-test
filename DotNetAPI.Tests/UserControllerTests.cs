using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using DotNetAPI.Controllers;
using DotNetAPI.DAL;
using DotNetAPI.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class UserControllerTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public UserControllerTests()
    {
        // Set up a service provider with a test database context
        _serviceProvider = new ServiceCollection()
            .AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryTestDatabase"))
            .AddScoped<IUnitOfWork, UnitOfWork>() // Add your repository
            .AddScoped<IUserRepository, UserRepository>() // Add your repository
            .BuildServiceProvider();
    }

    public void Dispose()
    {
        // Dispose of the test database context
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async void Test_CreateUser()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var controller = new UserController(scope.ServiceProvider.GetRequiredService<IUnitOfWork>());

        // Act
        var newUser = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com"
        };
        await controller.CreateUser(newUser);

        // Assert
        using var contextScope = _serviceProvider.CreateScope();
        var dbContext = contextScope.ServiceProvider.GetRequiredService<AppDbContext>();
        var savedUser = await dbContext.Users.FirstOrDefaultAsync(u => u.FirstName == "John");

        Assert.NotNull(savedUser);
        Assert.Equal("Doe", savedUser.LastName);
        Assert.Equal("johndoe@example.com", savedUser.Email);
    }

    [Fact]
    public async Task Test_UpdateUser()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var controller = new UserController(scope.ServiceProvider.GetRequiredService<IUnitOfWork>());

        // Create a user to update
        var newUser = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com"
        };
        await repository.AddAsync(newUser);
        await dbContext.SaveChangesAsync();

        // Act
        var updatedUser = new User
        {
            Id = newUser.Id,
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            Email = "updated@example.com"
        };
        var result = await controller.UpdateUser(updatedUser.Id, updatedUser);

        // Assert
        var retrievedUser = await repository.GetByIdAsync(updatedUser.Id);

        Assert.NotNull(retrievedUser);
        Assert.Equal("UpdatedFirstName", retrievedUser.FirstName);
        Assert.Equal("UpdatedLastName", retrievedUser.LastName);
        Assert.Equal("updated@example.com", retrievedUser.Email);
    }

    [Fact]
    public async Task Test_RetrieveUsersWithQueryParameters()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var controller = new UserController(scope.ServiceProvider.GetRequiredService<IUnitOfWork>());

        // Create some sample users
        var users = new List<User>
    {
        new User { FirstName = "John", LastName = "Smith", Email = "johndoe@example.com" },
        new User { FirstName = "Jane", LastName = "Smith", Email = "janesmith@example.com" },
        new User { FirstName = "Alice", LastName = "Johnson", Email = "alicejohnson@example.com" }
    };

        await dbContext.Users.AddRangeAsync(users);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await controller.GetUsers(lastName: "Smith", ascending: false);
        var resultType = result.Result as OkObjectResult;
        var usersInDatabase = await dbContext.Users.ToListAsync();

        Assert.NotNull(resultType);
        // Assert
        var userEntities = Assert.IsType<List<User>>(resultType.Value);
        Assert.Equal(2, userEntities.Count); // two users with the last name "Smith"
        Assert.Equal("Smith", userEntities[0].LastName);
        Assert.Equal("John", userEntities[0].FirstName);
        Assert.Equal("Jane", userEntities[1].FirstName); 
    }

    [Fact]
    public async Task Test_DeleteUser()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var controller = new UserController(scope.ServiceProvider.GetRequiredService<IUnitOfWork>());

        // Create a user to delete
        var newUser = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com"
        };
        await repository.AddAsync(newUser);
        await dbContext.SaveChangesAsync();

        // Act
        await controller.DeleteUser(newUser.Id);

        // Assert
        var deletedUser = await repository.GetByIdAsync(newUser.Id);

        Assert.Null(deletedUser); // The user should not exist after deletion
    }

}
