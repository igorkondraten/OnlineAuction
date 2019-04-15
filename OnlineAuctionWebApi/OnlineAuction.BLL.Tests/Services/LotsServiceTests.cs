using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Exceptions;
using OnlineAuction.BLL.Interfaces;
using OnlineAuction.BLL.Services;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.BLL.Tests.Services
{
    [TestFixture]
    public class LotsServiceTests
    {
        private ILotsService _service;
        private Mock<IUnitOfWork> _mockUnitWork;
        private List<Lot> _lots;

        [OneTimeSetUp]
        public void Init()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _service = new LotsService(_mockUnitWork.Object);
            _lots = new List<Lot>
            {
                new Lot() {
                    BeginDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                    EndDate = DateTime.Now.Add(TimeSpan.FromDays(2)),
                    InitialPrice = 10,
                    Name = "Lot 1",
                    LotId = 1,
                    UserId = 1,
                    CategoryId = 1
                },
                new Lot() {
                    BeginDate = DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                    EndDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                    InitialPrice = 10,
                    Name = "Lot 2",
                    LotId = 2,
                    UserId = 2,
                    CategoryId = 1
                }
            };
        }

        [Test]
        public async Task CreateLotAsync_CreateLotWithoutImage_CreatedLotReturned()
        {
            var lot = new LotDTO()
            {
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                InitialPrice = 10,
                Name = "Lot 3",
                LotId = 2,
                UserName = "User1",
                Category = new CategoryDTO() { CategoryId = 1 }
            };
            _mockUnitWork.Setup(x => x.UserProfiles.FindAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), 10, 0))
                .ReturnsAsync((Items: new List<UserProfile>() { new UserProfile() }, TotalCount: _lots.Count));
            _mockUnitWork.Setup(x => x.Categories.GetAsync(lot.Category.CategoryId)).ReturnsAsync(new Category());
            _mockUnitWork.Setup(x => x.Lots.GetAsync(lot.LotId)).ReturnsAsync(new Lot() {
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                InitialPrice = 10,
                Name = "Lot 3",
                LotId = 2,
                UserId = 1,
                CategoryId = 1
            });
            _mockUnitWork.Setup(x => x.Lots.Create(It.IsAny<Lot>()));

            var createdLot = await _service.CreateLotAsync(lot);

            Assert.That(createdLot, Is.Not.Null);
            Assert.That(createdLot.LotId, Is.EqualTo(lot.LotId));
        }

        [Test]
        public async Task CreateLotAsync_CreateLotWithPastDate_ValidationExceptionThrown()
        {
            var lot = new LotDTO()
            {
                BeginDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                EndDate = DateTime.Now,
                InitialPrice = 10,
                Name = "Lot 3",
                LotId = 2,
                UserName = "User1",
                Category = new CategoryDTO() { CategoryId = 1 }
            };

            Assert.ThrowsAsync<ValidationException>(() => _service.CreateLotAsync(lot), "Incorrect date.");
        }

        [Test]
        public async Task EditLotAsync_EditExistingLot_LotUpdated()
        {
            var newLot = new LotDTO()
            {
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                InitialPrice = 10,
                Name = "Lot 3",
                LotId = 2,
                UserName = "User1",
                Category = new CategoryDTO() { CategoryId = 1 }
            };
            var oldLot = new Lot()
            {
                BeginDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(10),
                InitialPrice = 10,
                Name = "Lot 3",
                LotId = 2,
                UserId = 1,
                CategoryId = 1
            };
            _mockUnitWork.Setup(x => x.Lots.GetAsync(oldLot.LotId)).ReturnsAsync(oldLot);
            _mockUnitWork.Setup(x => x.Categories.GetAsync(newLot.Category.CategoryId)).ReturnsAsync(new Category());
            _mockUnitWork.Setup(x => x.Lots.Update(It.IsAny<Lot>()));

            await _service.EditLotAsync(newLot);

            _mockUnitWork.Verify(x => x.Lots.Update(It.IsAny<Lot>()));
        }

        [Test]
        public async Task EditLotAsync_EditFinishedLot_ValidationExceptionThrown()
        {
            var newLot = new LotDTO()
            {
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                InitialPrice = 10,
                Name = "Lot 3",
                LotId = 2,
                UserName = "User1",
                Category = new CategoryDTO() { CategoryId = 1 }
            };
            var oldLot = _lots[1];
            _mockUnitWork.Setup(x => x.Lots.GetAsync(oldLot.LotId)).ReturnsAsync(oldLot);
            _mockUnitWork.Setup(x => x.Categories.GetAsync(newLot.Category.CategoryId)).ReturnsAsync(new Category());
            _mockUnitWork.Setup(x => x.Lots.Update(It.IsAny<Lot>()));

            Assert.ThrowsAsync<ValidationException>(() => _service.EditLotAsync(newLot),
                "Can't edit auction begin date after it has started.");
        }

        [Test]
        public async Task DeleteLotAsync_DeleteExistingLot_LotDeleted()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.Lots.GetAsync(id)).ReturnsAsync(_lots.Find(x => x.LotId == id));
            _mockUnitWork.Setup(x => x.Lots.Delete(id));

            await _service.DeleteLotAsync(id);

            _mockUnitWork.Verify(x => x.Lots.Delete(id), Times.Once);
        }

        [Test]
        public async Task DeleteLotAsync_DeleteNotExistingLot_NotFoundExceptionThrown()
        {
            var id = 10;
            _mockUnitWork.Setup(x => x.Lots.GetAsync(id)).ReturnsAsync(_lots.Find(x => x.LotId == id));
            _mockUnitWork.Setup(x => x.Lots.Delete(id));

            Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteLotAsync(id));
        }

        [Test]
        public async Task GetAllLotsAsync_Get2LotsWithPagination_LotsReturned()
        {
            _mockUnitWork.Setup(x => x.Lots.GetAllAsync(10, 0)).ReturnsAsync((Items: _lots, TotalCount: _lots.Count));

            var results = await _service.GetAllLotsAsync(10, 0);

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Lots.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task FindLotsAsync_FindLots_LotReturned()
        {
            var keywords = "Lot 1";
            _mockUnitWork.Setup(x => x.Lots.FindAsync(It.IsAny<Expression<Func<Lot, bool>>>(), 10, 0))
                .ReturnsAsync((Items: _lots.Where(x => x.Name.Contains(keywords)), TotalCount: _lots.Count));

            var result = await _service.FindLotsAsync(keywords, 10, 0);

            Assert.That(result.Lots, Is.Not.Null);
            Assert.That(result.Lots.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetLotsByUserAsync_GetLotsOfUser_1LotReturned()
        {
            var userProfileId = 1;
            _mockUnitWork.Setup(x => x.Lots.FindAsync(It.IsAny<Expression<Func<Lot, bool>>>(), 10, 0))
                .ReturnsAsync((Items: _lots.Where(x => x.UserId == userProfileId), TotalCount: _lots.Count));

            var result = await _service.GetLotsByUserAsync(userProfileId, 10, 0);

            Assert.That(result.Lots, Is.Not.Null);
            Assert.That(result.Lots.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetLotAsyncTest()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.Lots.GetAsync(id)).ReturnsAsync(_lots[0]);

            var result = await _service.GetLotAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.LotId, Is.EqualTo(id));
        }
    }
}