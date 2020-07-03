using DWDW_Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DWDW_Service.UnitOfWorks
{

    public class UnitOfWork
    {
        private readonly DbContext dbContext;
        //private readonly DbConnection dbConnection;

        public UnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
            //dbConnection = dbContext.Database.GetDbConnection();
        }
        public IDbContextTransaction CreateTransaction()
        {
            return dbContext.Database.BeginTransaction();
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
                return new DeviceRepository(dbContext);
            }
        }

        public ILocationRepository LocationRepository
        {
            get
            {
                return new LocationRepository(dbContext);
            }
        }
        public IRecordRepository RecordRepository
        {
            get
            {
                return new RecordRepository(dbContext);
            }
        }
        public IRoleRepository RoleRepository
        {
            get
            {
                return new RoleRepository(dbContext);
            }
        }

        public IRoomDeviceRepository RoomDeviceRepository
        {
            get
            {
                return new RoomDeviceRepository(dbContext);
            }
        }
        public IRoomRepository RoomRepository
        {
            get
            {
                return new RoomRepository(dbContext);
            }
        }
        public IShiftRepository ShiftRepository
        {
            get
            {
                return new ShiftRepository(dbContext);
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                return new UserRepository(dbContext);
            }
        }

    }
}
