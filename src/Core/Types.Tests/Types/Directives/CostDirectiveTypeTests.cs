using System.Linq;
using System.Threading.Tasks;
using HotChocolate.Configuration;
using Xunit;

namespace HotChocolate.Types
{
    public class CostDirectiveTypeTests
    {
        [Fact]
        public void AnnotateCostToObjectFieldCodeFirst()
        {
            // arrange
            // act
            var schema = Schema.Create(
                t =>
                {
                    t.RegisterQueryType(new ObjectType(
                        o => o.Name("Query")
                            .Field("field")
                            .Argument("a", a => a.Type<StringType>())
                            .Type<StringType>()
                            .Cost(5)));

                    t.Use(next => context => Task.CompletedTask);
                });

            var query = schema.GetType<ObjectType>("Query");
            IDirective directive = query.Fields["field"].Directives
                .Single(t => t.Name == "cost");
            CostDirective obj = directive.ToObject<CostDirective>();
            Assert.Equal(5, obj.Complexity);
        }

        [Fact]
        public void AnnotateCostToObjectFieldCodeFirstOneMultiplier()
        {
            // arrange
            // act
            var schema = Schema.Create(
                t =>
                {
                    t.RegisterQueryType(new ObjectType(
                        o => o.Name("Query")
                            .Field("field")
                            .Argument("a", a => a.Type<StringType>())
                            .Type<StringType>()
                            .Cost(5, "a")));

                    t.Use(next => context => Task.CompletedTask);
                });

            var query = schema.GetType<ObjectType>("Query");
            IDirective directive = query.Fields["field"].Directives
                .Single(t => t.Name == "cost");
            CostDirective obj = directive.ToObject<CostDirective>();
            Assert.Equal(5, obj.Complexity);
            Assert.Collection(obj.Multipliers,
                t => Assert.Equal("a", t));
        }

        [Fact]
        public void AnnotateCostToObjectFieldCodeFirstTwoMultiplier()
        {
            // arrange
            // act
            var schema = Schema.Create(
                t =>
                {
                    t.RegisterQueryType(new ObjectType(
                        o => o.Name("Query")
                            .Field("field")
                            .Argument("a", a => a.Type<StringType>())
                            .Type<StringType>()
                            .Cost(5, "a", "b")));

                    t.Use(next => context => Task.CompletedTask);
                });

            var query = schema.GetType<ObjectType>("Query");
            IDirective directive = query.Fields["field"].Directives
                .Single(t => t.Name == "cost");
            CostDirective obj = directive.ToObject<CostDirective>();
            Assert.Equal(5, obj.Complexity);
            Assert.Collection(obj.Multipliers,
                t => Assert.Equal("a", t),
                t => Assert.Equal("b", t));
        }

        [Fact]
        public void AnnotateCostToInterfaceFieldCodeFirst()
        {
            // arrange
            // act
            var schema = Schema.Create(
                t =>
                {
                    t.RegisterQueryType(new ObjectType(
                        o => o.Name("Query")
                            .Field("field")
                            .Argument("a", a => a.Type<StringType>())
                            .Type<StringType>()));

                    t.RegisterType(new InterfaceType(
                        o => o.Name("IQuery")
                            .Field("field")
                            .Argument("a", a => a.Type<StringType>())
                            .Type<StringType>()
                            .Cost(5)));

                    t.Use(next => context => Task.CompletedTask);
                });

            var queryInterface = schema.GetType<InterfaceType>("IQuery");
            IDirective directive = queryInterface.Fields["field"].Directives
                .Single(t => t.Name == "cost");
            CostDirective obj = directive.ToObject<CostDirective>();
            Assert.Equal(5, obj.Complexity);
        }

        [Fact]
        public void AnnotateCostToInterfaceFieldCodeFirstOneMultiplier()
        {
            // arrange
            // act
            var schema = Schema.Create(
                t =>
                {
                    t.RegisterQueryType(new ObjectType(
                        o => o.Name("Query")
                            .Field("field")
                            .Argument("a", a => a.Type<StringType>())
                            .Type<StringType>()));

                    t.RegisterType(new InterfaceType(
                        o => o.Name("IQuery")
                            .Field("field")
                            .Argument("a", a => a.Type<StringType>())
                            .Type<StringType>()
                            .Cost(5, "a")));

                    t.Use(next => context => Task.CompletedTask);
                });

            var queryInterface = schema.GetType<InterfaceType>("IQuery");
            IDirective directive = queryInterface.Fields["field"].Directives
                .Single(t => t.Name == "cost");
            CostDirective obj = directive.ToObject<CostDirective>();
            Assert.Equal(5, obj.Complexity);
            Assert.Collection(obj.Multipliers,
                t => Assert.Equal("a", t));
        }

        [Fact]
        public void AnnotateCostToInterfaceFieldCodeFirstTwoMultiplier()
        {
            // arrange
            // act
            var schema = Schema.Create(
                t =>
                {
                    t.RegisterQueryType(new ObjectType(
                        o => o.Name("Query")
                            .Field("field")
                            .Argument("a", a => a.Type<StringType>())
                            .Type<StringType>()));

                    t.RegisterType(new InterfaceType(
                        o => o.Name("IQuery")
                            .Field("field")
                            .Argument("a", a => a.Type<StringType>())
                            .Type<StringType>()
                            .Cost(5, "a", "b")));

                    t.Use(next => context => Task.CompletedTask);
                });

            var queryInterface = schema.GetType<InterfaceType>("IQuery");
            IDirective directive = queryInterface.Fields["field"].Directives
                .Single(t => t.Name == "cost");
            CostDirective obj = directive.ToObject<CostDirective>();
            Assert.Equal(5, obj.Complexity);
            Assert.Collection(obj.Multipliers,
                t => Assert.Equal("a", t),
                t => Assert.Equal("b", t));
        }

        [Fact]
        public void AnnotateCostToObjectFieldSchemaFirst()
        {
            // arrange
            // act
            var schema = Schema.Create(
                @"type Query {
                    field(a: Int): String
                        @cost(complexity: 5 multipliers: [""a""])
                }",
                t => t.Use(next => context => Task.CompletedTask));

            var query = schema.GetType<ObjectType>("Query");
            IDirective directive = query.Fields["field"].Directives
                .Single(t => t.Name == "cost");
            CostDirective obj = directive.ToObject<CostDirective>();
            Assert.Equal(5, obj.Complexity);
            Assert.Collection(obj.Multipliers,
                t => Assert.Equal("a", t));
        }

        [Fact]
        public void AnnotateCostToInterfaceFieldSchemaFirst()
        {
            // arrange
            // act
            var schema = Schema.Create(
                @"
                type Query {
                    field(a: Int): String
                        @cost(complexity: 5 multipliers: [""a""])
                }

                interface IQuery {
                    field(a: Int): String
                        @cost(complexity: 5 multipliers: [""a""])
                }
                ",
                t => t.Use(next => context => Task.CompletedTask));

            var queryInterface = schema.GetType<InterfaceType>("IQuery");
            IDirective directive = queryInterface.Fields["field"].Directives
                .Single(t => t.Name == "cost");
            CostDirective obj = directive.ToObject<CostDirective>();
            Assert.Equal(5, obj.Complexity);
            Assert.Collection(obj.Multipliers,
                t => Assert.Equal("a", t));
        }

        [Fact]
        public void CreateCostDirective()
        {
            // arrange
            SchemaContext schemaContext = SchemaContextFactory.Create();

            var schemaConfiguration = new SchemaConfiguration(
                sp => { },
                schemaContext.Types,
                schemaContext.Resolvers,
                schemaContext.Directives);

            var typeFinalizer = new TypeFinalizer(schemaConfiguration);
            typeFinalizer.FinalizeTypes(schemaContext, null);

            // assert
            DirectiveType directive = schemaContext.Directives
                .GetDirectiveTypes()
                .FirstOrDefault(t => t.Name == "cost");

            // assert
            Assert.NotNull(directive);
            Assert.IsType<CostDirectiveType>(directive);
            Assert.Equal("cost", directive.Name);
            Assert.Collection(directive.Arguments,
                t =>
                {
                    Assert.Equal("complexity", t.Name);
                    Assert.IsType<IntType>(
                        Assert.IsType<NonNullType>(t.Type).Type);
                },
                t =>
                {
                    Assert.Equal("multipliers", t.Name);
                    Assert.IsType<StringType>(
                        Assert.IsType<NonNullType>(
                            Assert.IsType<ListType>(t.Type).ElementType).Type);
                });
            Assert.Collection(directive.Locations,
                t => Assert.Equal(DirectiveLocation.FieldDefinition, t));
        }
    }
}
