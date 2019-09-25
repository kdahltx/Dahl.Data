using Dahl.Data.Common;
using Dahl.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dahl.Data.Tests.Common
{
    [TestCategory( "Common.EntityReaderTests" )]
    [TestClass]
    public class EntityReaderTests
    {
        [TestMethod]
        public void TestPropertyAccessor()
        {
            IPropertyAccessor[] accessors = typeof( TestClass ).GetAccessorList();
            foreach ( var item in _TestClassList )
            {
                foreach ( var accessor in accessors )
                {
                    //Type pt = accessor.PropertyInfo.PropertyType;
                    //if ( pt == typeof( string ) )
                    //    accessor.SetValue( testClass, Guid.NewGuid().ToString( "n" ).Substring( 0, 9 ) );
                    //else if ( pt == typeof( int ) )
                    //    accessor.SetValue( testClass, new Random().Next( 0, int.MaxValue ) );

                    var propertyName = accessor.PropertyInfo.Name;
                    var propertyValue = accessor.GetValue( item );
                    Trace.Write( $"{propertyName}:{propertyValue}," );
                }
                Trace.WriteLine( "" );
            }
        }

        [TestMethod]
        public void EntityListToDataTable()
        {
            var x = _TestClassList.ToDataTable();
            Trace.WriteLine( x );
        }

        [TestMethod]
        public void DataTableToEntityList()
        {
            var dt = _TestClassList.ToDataTable();
            var el = dt.ToList<TestClass>();

            foreach ( var item in el )
                Trace.WriteLine( $"{item.Id},{item.FirstName},{item.LastName},{item.Age}" );
        }

        protected static List<TestClass> _TestClassList = new List<TestClass>
        {
            { new TestClass { Id=1,  FirstName="FirstName01", LastName="LastName01", Age = 50} },
            { new TestClass { Id=2,  FirstName="FirstName02", LastName="LastName02", Age = 51} },
            { new TestClass { Id=3,  FirstName="FirstName03", LastName="LastName03", Age = 52} },
            { new TestClass { Id=4,  FirstName="FirstName04", LastName="LastName04", Age = 53} },
            { new TestClass { Id=5,  FirstName="FirstName05", LastName="LastName05", Age = 54} },
            { new TestClass { Id=6,  FirstName="FirstName06", LastName="LastName06", Age = 55} },
            { new TestClass { Id=7,  FirstName="FirstName07", LastName="LastName07", Age = 56} },
            { new TestClass { Id=8,  FirstName="FirstName08", LastName="LastName08", Age = 57} },
            { new TestClass { Id=9,  FirstName="FirstName09", LastName="LastName09", Age = 58} },
            { new TestClass { Id=10, FirstName="FirstName10", LastName="LastName10", Age = 60} },
        };
    }

    public class TestClass
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
