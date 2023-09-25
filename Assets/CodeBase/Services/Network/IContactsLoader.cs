using System;
using Model.DTO.Storages;

namespace Services.Network
{
    public interface IContactsLoader
    {
        void Load(Action<EmployeesInfoStorage> loaded);
    }
}