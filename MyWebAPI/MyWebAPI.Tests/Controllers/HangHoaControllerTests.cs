
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Controllers;
using MyWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class HangHoaControllerTests
{
    private readonly HangHoaController _controller;
    private readonly AppDbContext _context;

    public HangHoaControllerTests()
    {
        // Use an in-memory database for testing
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new AppDbContext(options);
        _controller = new HangHoaController(_context);

        // Seed the database with some data
        if (!_context.HangHoas.Any())
        {
            _context.HangHoas.Add(new HangHoa { MaHH = Guid.NewGuid(), TenHH = "Hang Hoa 1", DonGia = 1000, MaLoai = 1 });
            _context.HangHoas.Add(new HangHoa { MaHH = Guid.NewGuid(), TenHH = "Hang Hoa 2", DonGia = 2000, MaLoai = 2 });
            _context.SaveChanges();
        }
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfHangHoa()
    {
        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var hangHoas = Assert.IsType<List<HangHoa>>(okResult.Value);
        Assert.Equal(2, hangHoas.Count);
    }

    [Fact]
    public void GetById_ReturnsOkResult_WithHangHoa_WhenFound()
    {
        // Arrange
        var existingHangHoa = _context.HangHoas.First();
        var existingId = existingHangHoa.MaHH;

        // Act
        var result = _controller.GetById(existingId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var hangHoa = Assert.IsType<HangHoa>(okResult.Value);
        Assert.Equal(existingId, hangHoa.MaHH);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = _controller.GetById(nonExistentId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetById_ReturnsBadRequest_WhenInvalidGuid()
    {
        // Arrange
        var invalidId = Guid.Empty; // Using an invalid Guid

        // Act
        var result = _controller.GetById(invalidId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }


    [Fact]
    public void Create_ReturnsOkResult_WithHangHoa()
    {
        // Arrange
        var newHangHoa = new HangHoa { TenHH = "New Hang Hoa", DonGia = 3000, MaLoai = 3 };

        // Act
        var result = _controller.Create(newHangHoa);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var hangHoa = Assert.IsType<HangHoa>(okResult.Value);
        Assert.Equal("New Hang Hoa", hangHoa.TenHH);
        Assert.NotEqual(Guid.Empty, hangHoa.MaHH); // Ensure a new ID was generated
        Assert.Equal(3, _context.HangHoas.Count()); // Check if added to the database
    }

    [Fact]
    public void Update_ReturnsOkResult_WhenFoundAndUpdated()
    {
        // Arrange
        var existingHangHoa = _context.HangHoas.First();
        var updatedHangHoa = new HangHoa { MaHH = existingHangHoa.MaHH, TenHH = "Updated Hang Hoa", DonGia = 4000, MaLoai = 1 };

        // Act
        var result = _controller.Update(existingHangHoa.MaHH, updatedHangHoa);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var hangHoa = Assert.IsType<HangHoa>(okResult.Value);
        Assert.Equal("Updated Hang Hoa", hangHoa.TenHH);
        Assert.Equal(4000, hangHoa.DonGia);

        // Verify the update in the database
        var dbHangHoa = _context.HangHoas.Find(existingHangHoa.MaHH);
        Assert.Equal("Updated Hang Hoa", dbHangHoa.TenHH);
        Assert.Equal(4000, dbHangHoa.DonGia);
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updatedHangHoa = new HangHoa { MaHH = nonExistentId, TenHH = "Updated Hang Hoa", DonGia = 4000, MaLoai = 1 };

        // Act
        var result = _controller.Update(nonExistentId, updatedHangHoa);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Update_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var existingHangHoaId = _context.HangHoas.First().MaHH;
        var differentId = Guid.NewGuid();
        var updatedHangHoa = new HangHoa { MaHH = differentId, TenHH = "Updated Hang Hoa", DonGia = 4000, MaLoai = 1 };

        // Act
        var result = _controller.Update(existingHangHoaId, updatedHangHoa);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Remove_ReturnsOkResult_WhenFoundAndRemoved()
    {
        // Arrange
        var hangHoaToRemove = _context.HangHoas.First();
        var idToRemove = hangHoaToRemove.MaHH;

        // Act
        var result = _controller.Remove(idToRemove);

        // Assert
        Assert.IsType<OkResult>(result);
        Assert.Equal(1, _context.HangHoas.Count()); // Check if removed from the database
        Assert.Null(_context.HangHoas.Find(idToRemove)); // Verify it's no longer in the database
    }

    [Fact]
    public void Remove_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = _controller.Remove(nonExistentId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Remove_ReturnsBadRequest_WhenInvalidGuid()
    {
        // Arrange
        var invalidId = Guid.Empty; // Using an invalid Guid

        // Act
        var result = _controller.Remove(invalidId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}