using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Exceptions;
using OnlineAuction.BLL.Infrastructure.AutoMapper;
using OnlineAuction.BLL.Interfaces;
using OnlineAuction.BLL.Services;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;


namespace OnlineAuction.BLL.Tests.Services
{
    [TestFixture]
    public class BidsServiceTests
    {
        private IBidsService _service;
        private Mock<IUnitOfWork> _mockUnitWork;
        private List<Bid> _bids;
        private List<Lot> _lots;
        private List<UserProfile> _users;

        [OneTimeSetUp]
        public void Init()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _service = new BidsService(_mockUnitWork.Object);
            _lots = new List<Lot>()
            {
                new Lot() { LotId = 1, InitialPrice = 1 },
                new Lot() { LotId = 2, InitialPrice = 1 },
                new Lot() { LotId = 3, InitialPrice = 1 }
            };
            _bids = new List<Bid>()
            {
                new Bid() {BidId = 1, LotId = 1, PlacedUserId = 1, Price = 10, Date = DateTime.Now, Lot = new Lot(), PlacedUser = new UserProfile() },
                new Bid() {BidId = 2, LotId = 2, PlacedUserId = 2, Price = 15, Date = DateTime.Now, Lot = new Lot(), PlacedUser = new UserProfile() },
                new Bid() {BidId = 3, LotId = 2, PlacedUserId = 2, Price = 20, Date = DateTime.Now, Lot = new Lot(), PlacedUser = new UserProfile() },
                new Bid() {BidId = 4, LotId = 2, PlacedUserId = 2, Price = 25, Date = DateTime.Now, Lot = new Lot(), PlacedUser = new UserProfile() },
                new Bid() {BidId = 5, LotId = 3, PlacedUserId = 3, Price = 30, Date = DateTime.Now, Lot = new Lot(), PlacedUser = new UserProfile() },
            };
            _users = new List<UserProfile>()
            {
                new UserProfile() { UserProfileId = 1, Name = "User" },
                new UserProfile() { UserProfileId = 2, Name = "User1" },
                new UserProfile() { UserProfileId = 3, Name = "User2" }
            };
            AutoMapperServicesConfiguration.Configure();
        }

        [Test]
        public async Task CreateBidAsync_CreateValidBid_BidCreated()
        {
            var bid = new BidDTO()
            {
                BidId = 6,
                Date = new DateTime(2019, 1, 1, 12, 0, 0),
                PlacedUserName = "User",
                LotId = 1,
                Price = 20
            };
            _mockUnitWork.Setup(x => x.UserProfiles.FindAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), 10, 0))
                .ReturnsAsync((Items: _users, TotalCount: 1));
            _mockUnitWork.Setup(x => x.Lots.GetAsync(bid.LotId)).ReturnsAsync(new Lot()
            {
                LotId = 1,
                BeginDate = new DateTime(2018, 1, 1, 12, 0, 0),
                EndDate = new DateTime(2020, 1, 1, 12, 0, 0)
            });
            _mockUnitWork.Setup(x => x.Bids.GetAsync(bid.BidId)).ReturnsAsync(new Bid() { BidId = 6 });

            var createdBid = await _service.CreateBidAsync(bid);

            Assert.That(createdBid, Is.Not.Null);
            Assert.That(createdBid.BidId, Is.EqualTo(bid.BidId));
            _mockUnitWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void CreateBidAsync_CreateBidInNotActiveAuction_ValidationExceptionThrown()
        {
            var bid = new BidDTO()
            {
                BidId = 6,
                Date = new DateTime(2019, 1, 1, 12, 0, 0),
                PlacedUserName = "User",
                LotId = 1,
                Price = 20
            };
            _mockUnitWork.Setup(x => x.UserProfiles.FindAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), 10, 0))
                .ReturnsAsync((Items: _users, TotalCount: _lots.Count));
            _mockUnitWork.Setup(x => x.Lots.GetAsync(bid.LotId)).ReturnsAsync(new Lot()
            {
                LotId = 1,
                BeginDate = new DateTime(2018, 1, 1, 12, 0, 0),
                EndDate = new DateTime(2019, 1, 1, 12, 0, 0)
            });
            _mockUnitWork.Setup(x => x.Bids.GetAsync(bid.BidId)).ReturnsAsync(new Bid() { BidId = 6 });

            Assert.ThrowsAsync<ValidationException>(() => _service.CreateBidAsync(bid), "Auction is not active.");
        }

        [Test]
        public async Task DeleteBidAsyncTest_DeleteExistingBid_BidDeleted()
        {
            var bidId = 3;
            _mockUnitWork.Setup(x => x.Bids.GetAsync(bidId)).ReturnsAsync(_bids.Find(x => x.BidId == bidId));
            _mockUnitWork.Setup(x => x.Bids.Delete(bidId));

            await _service.DeleteBidAsync(bidId);

            _mockUnitWork.Verify(x => x.Bids.Delete(bidId), Times.Once);
        }

        [Test]
        public async Task DeleteBidAsyncTest_DeleteNotExistingBid_NotFoundExceptionThrown()
        {
            var bidId = 10;
            _mockUnitWork.Setup(x => x.Bids.GetAsync(bidId)).ReturnsAsync(_bids.Find(x => x.BidId == bidId));
            _mockUnitWork.Setup(x => x.Bids.Delete(bidId));

            Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteBidAsync(bidId));
        }

        [Test]
        public async Task GetBidsByLotAsync_GetBidsByExistingLotId_3BidsReturned()
        {
            var lotId = 2;
            _mockUnitWork.Setup(x => x.Bids.GetAllByLotAsync(lotId)).ReturnsAsync(_bids.Where(x => x.LotId == lotId));
            _mockUnitWork.Setup(x => x.Lots.GetAsync(lotId)).ReturnsAsync(_lots.Find(x => x.LotId == lotId));

            var results = await _service.GetBidsByLotAsync(lotId);

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetBidsByLotAsync_GetBidsByInvalidLotId_NotFoundExceptionThrown()
        {
            var lotId = 5;
            _mockUnitWork.Setup(x => x.Bids.GetAllByLotAsync(lotId)).ReturnsAsync(_bids.Where(x => x.LotId == lotId));
            _mockUnitWork.Setup(x => x.Lots.GetAsync(lotId)).ReturnsAsync(_lots.Find(x => x.LotId == lotId));

            Assert.ThrowsAsync<NotFoundException>(() => _service.GetBidsByLotAsync(lotId));
        }

        [Test]
        public async Task GetBidsByUserAsyncTest_GetBidsByExistingUserId_3BidsReturned()
        {
            var userProfileId = 2;
            _mockUnitWork.Setup(x => x.Bids.GetAllByUserAsync(userProfileId)).ReturnsAsync(_bids.Where(x => x.PlacedUserId == userProfileId));
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(userProfileId)).ReturnsAsync(_users.Find(x => x.UserProfileId == userProfileId));

            var results = await _service.GetBidsByUserAsync(userProfileId);

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GetBidsByUserAsyncTest_GetBidsByInvalidUserId_NotFoundExceptionThrown()
        {
            var userProfileId = 10;
            _mockUnitWork.Setup(x => x.Bids.GetAllByUserAsync(userProfileId)).ReturnsAsync(_bids.Where(x => x.PlacedUserId == userProfileId));
            _mockUnitWork.Setup(x => x.UserProfiles.GetAsync(userProfileId)).ReturnsAsync(_users.Find(x => x.UserProfileId == userProfileId));

            Assert.ThrowsAsync<NotFoundException>(() => _service.GetBidsByUserAsync(userProfileId));
        }

        [Test]
        public async Task GetBidAsyncTest_GetBidById_BidReturned()
        {
            var bidId = 5;
            _mockUnitWork.Setup(x => x.Bids.GetAsync(5)).ReturnsAsync(_bids.Find(x => x.BidId == bidId));

            var result = await _service.GetBidAsync(bidId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.BidId, Is.EqualTo(bidId));
        }
    }
}