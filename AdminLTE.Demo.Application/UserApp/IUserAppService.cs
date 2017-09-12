using AdminLTE.Demo.Application.UserApp.Dtos;
using AdminLTE.Demo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminLTE.Demo.Application.UserApp
{
    public interface IUserAppService
    {
        User CheckUser(string userName, string password);

        List<UserDto> GetUserByDepartment(Guid departmentId, int startPage, int pageSize, out int rowCount);

        UserDto InsertOrUpdate(UserDto dto);

        void DeleteBatch(List<Guid> ids);

        void Delete(Guid id);

        UserDto Get(Guid id);
    }
}
