using DWDW_Service.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DWDW_Service.UnitOfWorks
{

    public class UnitOfWork
    {
        private DbContext dbContext;
        private DbConnection dbConnection;

        public UnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbConnection = dbContext.Database.GetDbConnection();
        }
        public IDbTransaction CreateTransaction()
        {
            return dbConnection.BeginTransaction();
        }

        public IArrangementRepository ArrangementRepository
        {
            get
            {
                return new ArrangementRepository(dbContext);
            }
        }
        public IDeviceRepository DeviceRepository
        {
            get
            {
                return DeviceRepository ?? new DeviceRepository(dbContext);
            }
        }
        
        public ILocationRepository LocationRepository
        {
            get
            {
                return LocationRepository ?? new LocationRepository(dbContext);
            }
        }
        public IRecordRepository RecordRepository
        {
            get
            {
                return RecordRepository ?? new RecordRepository(dbContext);
            }
        }
        public IRoleRepository RoleRepository
        {
            get
            {
                return RoleRepository ?? new RoleRepository(dbContext);
            }
        }

        public IRoomDeviceRepository RoomDeviceRepository
        {
            get
            {
                return RoomDeviceRepository ?? new RoomDeviceRepository(dbContext);
            }
        }
        public IRoomRepository RoomRepository
        {
            get
            {
                return RoomRepository ?? new RoomRepository(dbContext);
            }
        }
        public IShiftRepository ShiftRepository
        {
            get
            {
                return ShiftRepository ?? new ShiftRepository(dbContext);
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                return UserRepository ?? new UserRepository(dbContext);
            }
        }
       
    }
}
