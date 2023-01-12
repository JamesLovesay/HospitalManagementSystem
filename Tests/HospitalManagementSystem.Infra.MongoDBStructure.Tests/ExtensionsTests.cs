using HospitalManagementSystem.Infra.MongoDBStructure.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace HospitalManagementSystem.Infra.MongoDBStructure.Tests
{
    public class ExtensionsTests
    {

        #region Add Filter With Flag
        [Fact]
        public void AddFilter_AddsFilterForTrueFlag()
        {
            var filters = new List<FilterDefinition<TestDocument>>();
            Expression<Func<TestDocument, bool>> field = d => d.IsActive;
            bool? flag = true;

            filters.AddFilter(field, flag);

            var expectedFilter = Builders<TestDocument>.Filter.Ne(field, default);
            Assert.Equal(expectedFilter.ToBsonDocument(), filters.Single().ToBsonDocument());
        }

        [Fact]
        public void AddFilter_AddsFilterForFalseFlag()
        {
            var filters = new List<FilterDefinition<TestDocument>>();
            Expression<Func<TestDocument, bool>> field = d => d.IsActive;
            bool? flag = false;

            filters.AddFilter(field, flag);

            var expectedFilter = Builders<TestDocument>.Filter.Eq(field, default);
            Assert.Equal(expectedFilter.ToBsonDocument(), filters.Single().ToBsonDocument());
        }

        [Fact]
        public void AddFilter_DoesNotAddFilterForNullFlag()
        {
            var filters = new List<FilterDefinition<TestDocument>>();
            Expression<Func<TestDocument, bool>> field = d => d.IsActive;
            bool? flag = null;

            filters.AddFilter(field, flag);

            Assert.Empty(filters);
        }

        #endregion

        #region Add Filter With String

        [Fact]
        public void AddFilter_AddsFilterForStringValue()
        {
            var filters = new List<FilterDefinition<TestDocument>>();
            Expression<Func<TestDocument, string>> field = d => d.Name;
            string value = "John";

            filters.AddFilter(field, value);

            var expectedFilter = Builders<TestDocument>.Filter.Eq(field, value);
            Assert.Equal(expectedFilter.ToBsonDocument(), filters.Single().ToBsonDocument());
        }

        [Fact]
        public void AddFilter_DoesNotAddFilterForEmptyString()
        {
            var filters = new List<FilterDefinition<TestDocument>>();
            Expression<Func<TestDocument, string>> field = d => d.Name;
            string value = "";

            filters.AddFilter(field, value);

            Assert.Empty(filters);
        }

        [Fact]
        public void AddFilter_DoesNotAddFilterForNullString()
        {
            var filters = new List<FilterDefinition<TestDocument>>();
            Expression<Func<TestDocument, string>> field = d => d.Name;
            string value = null;

            filters.AddFilter(field, value);

            Assert.Empty(filters);
        }

        #endregion

        #region Add Filter With List


        [Fact]
        public void AddFilter_AddsFilterForListValues()
        {
            var filters = new List<FilterDefinition<TestDocument>>();
            Expression<Func<TestDocument, int>> field = d => d.Age;
            var values = new List<int> { 21, 25, 30 };

            filters.AddFilter(field, values);

            var expectedFilter = Builders<TestDocument>.Filter.In(field, values);
            Assert.Equal(expectedFilter.ToBsonDocument(), filters.Single().ToBsonDocument());
        }

        [Fact]
        public void AddFilter_DoesNotAddFilterForEmptyList()
        {
            var filters = new List<FilterDefinition<TestDocument>>();
            Expression<Func<TestDocument, int>> field = d => d.Age;
            var values = new List<int>();

            filters.AddFilter(field, values);

            Assert.Empty(filters);
        }

        [Fact]
        public void AddFilter_DoesNotAddFilterForNullList()
        {
            var filters = new List<FilterDefinition<TestDocument>>();
            Expression<Func<TestDocument, int>> field = d => d.Age;
            List<int> values = null;

            filters.AddFilter(field, values);

            Assert.Empty(filters);
        }

        #endregion

        #region Consolidate Filters

        [Fact]
        public void Consolidate_ReturnsEmptyFilterForEmptyList()
        {
            var filters = new List<FilterDefinition<TestDocument>>();

            var consolidatedFilter = filters.Consolidate();

            var expectedFilter = Builders<TestDocument>.Filter.Empty;

            Assert.Equal(consolidatedFilter.ToBsonDocument(), expectedFilter.ToBsonDocument());
        }


        [Fact]
        public void Consolidate_ReturnsSingleFilterForListWithOneItem()
        {
            var filters = new List<FilterDefinition<TestDocument>>
            {
                Builders<TestDocument>.Filter.Eq(d => d.Age, 21)
            };

            var consolidatedFilter = filters.Consolidate();

            Assert.Equal(filters[0].ToBsonDocument(), consolidatedFilter.ToBsonDocument());
        }

        [Fact]
        public void Consolidate_ReturnsAndFilterForListWithMultipleItems()
        {
            var filters = new List<FilterDefinition<TestDocument>>
            {
                Builders<TestDocument>.Filter.Eq(d => d.Age, 21),
                Builders<TestDocument>.Filter.Eq(d => d.Name, "John")
            };

            var consolidatedFilter = filters.Consolidate();

            var expectedFilter = Builders<TestDocument>.Filter.And(filters);
            Assert.Equal(expectedFilter.ToBsonDocument(), consolidatedFilter.ToBsonDocument());
        }

        #endregion

        private class TestDocument
        {
            public bool IsActive { get; set; }

            public string? Name { get; set; }
            public int Age { get; set; }
        }
    }
}
