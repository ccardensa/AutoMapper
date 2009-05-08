using System.Collections.Generic;
using NUnit.Framework;
using NBehave.Spec.NUnit;

namespace AutoMapper.UnitTests
{
	namespace BidirectionalRelationships
	{
		[Ignore]
		public class When_mapping_to_a_destination_with_a_bidirectional_parent_one_to_many_child_relationship : AutoMapperSpecBase
		{
			private ParentDto _dto;

			protected override void Establish_context()
			{
				Mapper.CreateMap<ParentModel, ParentDto>();
				Mapper.CreateMap<ChildModel, ChildDto>();
				Mapper.AssertConfigurationIsValid();
			}

			protected override void Because_of()
			{
				var parent = new ParentModel {ID = "PARENT_ONE"};

				parent.AddChild(new ChildModel { ID = "CHILD_ONE" });

				_dto = Mapper.Map<ParentModel, ParentDto>(parent);
			}

			[Test]
			public void Should_preserve_the_parent_child_relationship_on_the_destination()
			{
				_dto.Children[0].Parent.ShouldBeTheSameAs(_dto);
			}

			private class ParentModel
			{
				public ParentModel()
				{
					Children = new List<ChildModel>();
				}

				public string ID { get; set; }
				
				public IList<ChildModel> Children { get; private set; }

				public void AddChild(ChildModel child)
				{
					child.Parent = this;
					Children.Add(child);
				}
			}

			private class ChildModel
			{
				public string ID { get; set; }
				public ParentModel Parent { get; set; }
			}

			private class ParentDto
			{
				public string ID { get; set; }
				public IList<ChildDto> Children { get; protected set; }
			}

			private class ChildDto
			{
				public string ID { get; set; }
				public ParentDto Parent { get; protected set; }
			}
		}
		public class When_mapping_to_a_destination_with_a_bidirectional_parent_one_to_one_child_relationship : AutoMapperSpecBase
		{
            private FooBar _dto;

			protected override void Establish_context()
			{
                Mapper.CreateMap<Foo, FooBar>();
				Mapper.AssertConfigurationIsValid();
			}

			protected override void Because_of()
			{
			    var foo = new Foo
			    {
			        Bar = new Bar
			        {
			            Value = "something"
			        }
			    };
			    foo.Bar.Foo = foo;
			    _dto = Mapper.Map<Foo, FooBar>(foo);
			}

			[Test]
            [Explicit]
            public void Should_preserve_the_parent_child_relationship_on_the_destination()
			{
				_dto.BarFooBarValue.ShouldEqual("something");
			}

			private class Foo
			{
                public Bar Bar { get; set; }
			}

			private class Bar
			{
                public Foo Foo { get; set; }
                public string Value { get; set; }
			}

			private class FooBar
			{
                public string BarFooBarValue { get; set; }
			}
		}
	}
}