using CMS_DL;
using CMS_BL;
using NSubstitute;
using AutoMapper;
using CMS_Common;

namespace CMS_UnitTests
{
    public class BaseBLTests
    {
        [Test]
        public async Task GetRecordById_ProductImageExists_ProductImageModel()
        {
            // Arrange
            var recordId = RandomID.GenerateRandomNumber();
            var fakeBaseDL = Substitute.For<IBaseDL<ProductImage>>();
            var fakeMapper = Substitute.For<IMapper>();
            fakeBaseDL.GetRecordByID(recordId).Returns(new ProductImage
            {
                ProductImageID = recordId,
                ProductImageSlug = "Phuc/img",
                IsDefault = StatusImage.MainImage,
                ProductID = 665747365
            });
            ProductImageModel expectedResult = new ProductImageModel
            {
                ProductImageID = recordId,
                ProductImageSlug = "Phuc/img",
                IsDefault = StatusImage.MainImage,
                ProductID = 665747365
            };
            fakeMapper.Map<ProductImageModel>(Arg.Any<ProductImage>()).Returns(expectedResult);

            var baseBL = new BaseBL<ProductImage, ProductImageModel>(fakeBaseDL, fakeMapper);

            // Act
            ProductImageModel result = await baseBL.GetRecordbyID(recordId);

            // Assert
            Assert.IsTrue(expectedResult.ProductImageID == result.ProductImageID
                          && expectedResult.ProductImageSlug == result.ProductImageSlug
                          && expectedResult.IsDefault == result.IsDefault
                          && expectedResult.ProductID == result.ProductID);
        }
        [Test]
        public async Task GetRecordById_ProductImageNotExists_ReturnsNull()
        {
            // Arrange
            var recordId = RandomID.GenerateRandomNumber();
            var fakeBaseDL = Substitute.For<IBaseDL<ProductImage>>();
            var fakeMapper = Substitute.For<IMapper>();
            fakeBaseDL.GetRecordByID(recordId).Returns((ProductImage)null);

            var baseBL = new BaseBL<ProductImage, ProductImageModel>(fakeBaseDL, fakeMapper);

            // Act
            ProductImageModel result = await baseBL.GetRecordbyID(recordId);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetRecordById_RecordIdInValid_ReturnsNull()
        {
            // Arrange
            var recordId = -1;
            var fakeBaseDL = Substitute.For<IBaseDL<ProductImage>>();
            var fakeMapper = Substitute.For<IMapper>();
            fakeBaseDL.GetRecordByID(recordId).Returns((ProductImage)null);

            var baseBL = new BaseBL<ProductImage, ProductImageModel>(fakeBaseDL, fakeMapper);

            // Act
            ProductImageModel result = await baseBL.GetRecordbyID(recordId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Insert_ProductImageValidData_ReturnSuccess()
        {
            // Arrange
            var fakeId = 123;
            var newProductImage = new ProductImageModel
            {
                ProductImageID = fakeId,
                ProductImageSlug = "Phuc/123",
                ProductID = 67555678,
                IsDefault = StatusImage.SecondaryImage
            };

            var fakeBaseDL = Substitute.For<IBaseDL<ProductImage>>();
            var fakeMapper = Substitute.For<IMapper>();
            var baseBL = new BaseBL<ProductImage, ProductImageModel>(fakeBaseDL, fakeMapper);


            // Act
            await baseBL.CreateRecord(newProductImage);

            // Assert
            //
            await fakeBaseDL.Received(1).CreateRecord(Arg.Is<ProductImage>(p => p.ProductImageID == fakeId));
        }

        [Test]
        public async Task Insert_ProductImageNotConstraintProductID_ReturnFalse() {
        // Arrange
        // Act
        // Asset
        } 
    }

}
