using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dahl.Data.Common;

namespace Dahl.Data.Tests.Core
{
    public class RepositoryTestCore : Dahl.Data.Common.RepositoryBase
    {
        protected override IDatabase CreateDatabase()
        {
            Database = new Dahl.Data.SqlServer.Database {
                ConnectionString = GetConnectionString( "App.SqlServer" )
            };

            return Database;
        }

        public List<DataProvider> ProviderList
        {
            get { return Database.ProviderList; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Dahl.Data.Tests.Common.Models.Course GetCourse( string name )
        {
            const string sqlCmd = "select * from Courses where [Name] like @name";
            var parms = new Dahl.Data.SqlServer.CommandParameter
            {
                CreateParameter( "@name", name )
            };

            var result = Database.ExecuteQuery<Tests.Common.Models.Course>( sqlCmd, parms )?.ToList();
            if ( result != null && !result.Any() )
                return null;

            return result?.ToList()[0];
        }

        ///-----------------------------------------------------------------------------------------
        /// <summary>
        /// Run #1 : took 3,376 ms to insert 500,000
        /// </summary>
        /// <returns></returns>
        public int InsertUsers()
        {
            Stopwatch sw = new Stopwatch();
            const string sqlCmd = "Insert DbDemo.Dbo.Ssn ( SsnId, Ssn1, Ssn2, Ssn3 ) " +
                                  "values( @ssnId, @ssn1, @ssn2, @ssn3 ) "             +

                                  "Insert DbDemo.Dbo.Users ( FirstName, LastName, SsnId ) " +
                                  "values (@firstName, @lastName, @ssnId )";

            var userList = CreateUserList( 9999, 1, 1, 1 );

            int count = 0;
            sw.Restart();
            foreach ( var user in userList )
            {
                var parms = new Dahl.Data.SqlServer.CommandParameter
                {
                    CreateParameter( "@firstName", $"First{user.fk_Ssn.Ssn1:D3}-{user.fk_Ssn.Ssn2:d2}-{user.fk_Ssn.Ssn3:D4}" ),
                    CreateParameter( "@lastName",  $"Last{user.fk_Ssn.Ssn1:D3}-{user.fk_Ssn.Ssn2:d2}-{user.fk_Ssn.Ssn3:D4}" ),
                    CreateParameter( "@ssnId",  user.fk_Ssn.SsnId ),
                    CreateParameter( "@ssn1",   user.fk_Ssn.Ssn1 ),
                    CreateParameter( "@ssn2",   user.fk_Ssn.Ssn2 ),
                    CreateParameter( "@ssn3",   user.fk_Ssn.Ssn3 )
                };
                count += Database.ExecuteCommand( sqlCmd, parms );
                if ( Database.LastError.Code != 0 )
                    break;
            }
            sw.Stop();
            Trace.WriteLine( $"InsertUsers -- Records Inserted: {count} --- Time to insert: {sw.ElapsedMilliseconds} ms" );
            return count;
        }

        ///-----------------------------------------------------------------------------------------
        /// <summary>
        /// Run #1 : ObjectReader
        ///     BulkInsertUsers --- CreateUserList(500000,1,1,1) executed in 1386 ms
        ///     BulkUpdate bulkCopy.WriteToServer took 4,385 ms to execute
        ///     BulkUpdate Merge statement took 3,634 ms to execute
        ///     BulkUpdate Merge statement took 6,871 ms to execute
        ///     BulkInsertUsers --- Database.BulkUpdate(userList) executed in 24,577 ms
        ///     
        /// Run #1 : EntityReader
        ///     BulkInsertUsers --- CreateUserList(500000,1,1,1) executed in 1541 ms
        ///     BulkUpdate bulkCopy.WriteToServer took 2,856 ms to execute
        ///     BulkUpdate Merge statement took 2,714 ms to execute
        ///     BulkUpdate Merge statement took 2,780 ms to execute
        ///     BulkInsertUsers --- Database.BulkUpdate(userList) executed in 17,175 ms
        ///     
        /// Run #2 : EntityReader
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        public int BulkInsertUsers()
        {
            const int listSize = 100000;
            Stopwatch sw = new Stopwatch();
            Common.Models.UsersBulkMapper bulkMapper = new Common.Models.UsersBulkMapper();

            sw.Restart();
            var userList = CreateUserList( listSize, 1, 1, 1 );
            sw.Stop();
            Trace.WriteLine( $"BulkInsertUsers --- CreateUserList({listSize},1,1,1) executed in {sw.ElapsedMilliseconds} ms" );

            sw.Restart();
            Database.BulkUpdate( userList, new Dahl.Data.Tests.Common.Models.UsersBulkMapper() );
            sw.Stop();
            Trace.WriteLine( $"BulkInsertUsers --- Database.BulkUpdate(userList) executed in {sw.ElapsedMilliseconds} ms" );

            //var resultList = Database.ExecuteQuery<Models.Users>( $"select * from #tmp_{bulkMapper.TmpTableName}" );
            //Trace.WriteLine( $"BulkUpdate --- Time to insert: {sw.ElapsedMilliseconds} ms" );
            return 0; // resultList.Count();
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int DeleteUsers()
        {
            const string sqlCmd = "delete DbDemo.Dbo.Users " +
                                  "delete DbDemo.Dbo.Ssn ";

            return Database.ExecuteCommand( sqlCmd );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Dahl.Data.Tests.Common.Models.Users> LoadUsers()
        {
            const string sqlCmd = "select * from Users u " +
                                  "inner join Ssn s on s.SsnId = u.SsnId";

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var list = Database.ExecuteQuery<Tests.Common.Models.Users>( sqlCmd, new Common.Models.UsersMap() );
            sw.Stop();
            Assert.IsNotNull( list );

            var users = list.ToList();
            Trace.WriteLine( $"LoadUsers(IDatabase database) loaded {users.Count} educators taking {sw.ElapsedMilliseconds:N0} ms." );
            return users;
        }

        public List<Common.Models.Users> LoadUsers2()
        {
            const string sqlCmd = "select * from Users " +
                                  "select * from Ssn ";

            Stopwatch sw = new Stopwatch();

            sw.Restart();
            var users = Database.ExecuteQuery( sqlCmd, new Common.Models.UsersMap() ).ToList();
            sw.Stop();
            Trace.WriteLine( $"LoadUsers(IDatabase database) ExecuteQuery returned {users.Count} user taking {sw.ElapsedMilliseconds:N0} ms." );

            sw.Restart();
            var list2 = Database.Read( new Common.Models.SsnMap() )
                                .ToDictionary(x => x.SsnId, x => x );
            Trace.WriteLine( $"LoadUsers(IDatabase database) Read returned {list2.Count} ssn's taking {sw.ElapsedMilliseconds:N0} ms." );

            sw.Restart();
            foreach ( var item in users )
            {
                if ( list2.TryGetValue( item.SsnId, out Common.Models.Ssn fk ) )
                    item.fk_Ssn = fk;
            }
            sw.Stop();
            Trace.WriteLine( $"Assignment took {sw.ElapsedMilliseconds:N0} ms." );

            Assert.IsNotNull( users );
            return users;
        }

        public List<Common.Models.Users> LoadOneUser()
        {
            const string sqlCmd = "select * from Users where LastName like @lastName ";
            var parms = new Dahl.Data.SqlServer.CommandParameter
            {
                CreateParameter( "@lastName", "Last001-01-0002" )
            };

            Stopwatch sw = new Stopwatch();

            sw.Restart();
            var users = Database.ExecuteQuery( sqlCmd, parms, new Common.Models.UsersMap() ).ToList();
            sw.Stop();
            Trace.WriteLine( $"LoadUsers(IDatabase database) ExecuteQuery returned {users.Count} user taking {sw.ElapsedMilliseconds:N0} ms." );

            Assert.IsNotNull( users );
            return users;
        }

        /// ----------------------------------------------------------------------------------------
        ///  <summary>
        ///  Creates a list of users with new ssn
        ///  </summary>
        ///  <param name="count"></param>
        /// <param name="ssn1"></param>
        /// <param name="ssn2"></param>
        /// <param name="ssn3"></param>
        /// <returns></returns>
        public List<Common.Models.Users> CreateUserList( int count, short ssn1, short ssn2, short ssn3 )
        {
            List<Common.Models.Users> userList = new List<Common.Models.Users>( count );
            for ( int i = 0; i < count; i++ )
            {
                Common.Models.Users user = new Common.Models.Users
                {
                    FirstName = $"First{ssn1:D3}-{ssn2:d2}-{ssn3:D4}",
                    LastName = $"Last{ssn1:D3}-{ssn2:d2}-{ssn3:D4}",
                    fk_Ssn = new Common.Models.Ssn
                    {
                        SsnId = Guid.NewGuid(),
                        Ssn1  = ssn1,
                        Ssn2  = ssn2,
                        Ssn3  = ssn3++
                    }
                };
                userList.Add( user );
                if ( ssn3 > 9999 )
                {
                    ssn3 = 1;
                    ssn2++;
                    if ( ssn2 > 99 )
                    {
                        ssn2 = 1;
                        ssn1++;
                    }
                }
            }

            return userList;
        }
    }
}
