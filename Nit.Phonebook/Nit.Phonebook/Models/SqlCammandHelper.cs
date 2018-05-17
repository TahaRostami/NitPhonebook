using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nit.Phonebook.Models
{
    public class SqlCammandHelper
    {
        //@Name
        public const string CMD_INSERT_INTO_EMPLOYEE_UNDER_CONSTRAINTS = @"
                                 IF Not EXISTS (SELECT * FROM Employee WHERE Name = @Name)
                                   insert into Employee values(@Name) ;  
               ";

        //@Name,@Id
        public const string CMD_UPDATE_INTO_EMPLOYEE_UNDER_CONSTRAINTS = @"
                                 IF Not EXISTS (SELECT * FROM Employee WHERE Name = @Name)
                                   update Employee set Name = @Name  where Id=@Id ;
               ";

        //Number , IsInternal
        public const string CMD_INSERT_INTO_PHONENUMBER_UNDER_CONSTRAINTS = @"
                                 IF Not EXISTS (SELECT * FROM PhoneNumber WHERE Number = @Number)
                                   insert into PhoneNumber values(@Number,@IsInternal) ;  
               ";


        //Number,IsInternal,Id
        public const string CMD_UPDATE_INTO_PHONENUMBER_UNDER_CONSTRAINTS = @"
                                 IF Not EXISTS (SELECT * FROM PhoneNumber WHERE Number = @Number)
                                   update PhoneNumber set Number = @Number , IsInternal = @IsInternal where Id=@Id ;
               ";

    }
}
