﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTestingDemo.Data.Contexts;
using UnitTestingDemo.Data.EntityModels;

namespace UnitTestingDemo.Data.Tests
{
    [TestClass]
    public class BookRepositoryTestsWithInMemoryDB
    {
        private readonly AuthorEntity shakespeare = new AuthorEntity
        {
            Id = 1,
            FirstName = "William",
            LastName = "Shakespeare",
            Books = new List<BookEntity>
            {
                new BookEntity
                {
                    Id = 1,
                    Title = "Hamlet",
                    Author = new AuthorEntity
                    {
                        Id = 1,
                        FirstName = "William",
                        LastName = "Shakespeare"
                    }
                },
                new BookEntity
                {
                    Id = 2,
                    Title = "A Midsummer Night's Dream",
                    Author = new AuthorEntity
                    {
                        Id = 1,
                        FirstName = "William",
                        LastName = "Shakespeare"
                    }
                }
            }
        };

        [TestMethod]
        public void GetAll_WithItems_returnsAll()
        {
            //arrange
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase("GetAllTest")
                .Options;
            using (var context = new BookContext(options))
            {
                context.Authors.Add(shakespeare);
                context.SaveChanges();
            }

            //act
            IList<BookEntity> result;
            using (var context = new BookContext(options))
            {
                var bookRepository = new BookRepository(context);
                result = bookRepository.GetAll();
            }

            //assert
            Assert.AreEqual(shakespeare.Books.Count(), result.Count);
        }
    }
}
